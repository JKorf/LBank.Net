using System;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.OrderBook;
using Microsoft.Extensions.Logging;
using LBank.Net.Clients;
using LBank.Net.Interfaces.Clients;
using LBank.Net.Objects.Options;
using LBank.Net.Objects.Models;

namespace LBank.Net.SymbolOrderBooks
{
    /// <summary>
    /// Implementation for a synchronized order book. After calling Start the order book will sync itself and keep up to date with new data. It will automatically try to reconnect and resync in case of a lost/interrupted connection.
    /// Make sure to check the State property to see if the order book is synced.
    /// </summary>
    public class LBankSpotSymbolOrderBook : SymbolOrderBook
    {
        private readonly bool _clientOwner;
        private readonly ILBankSocketClient _socketClient;
        private readonly TimeSpan _initialDataTimeout;

        /// <summary>
        /// Create a new order book instance
        /// </summary>
        /// <param name="symbol">The symbol the order book is for</param>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public LBankSpotSymbolOrderBook(string symbol, Action<LBankOrderBookOptions>? optionsDelegate = null)
            : this(symbol, optionsDelegate, null, null)
        {
            _clientOwner = true;
        }

        /// <summary>
        /// Create a new order book instance
        /// </summary>
        /// <param name="symbol">The symbol the order book is for</param>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        /// <param name="logger">Logger</param>
        /// <param name="restClient">Rest client instance</param>
        /// <param name="socketClient">Socket client instance</param>
        public LBankSpotSymbolOrderBook(
            string symbol,
            Action<LBankOrderBookOptions>? optionsDelegate,
            ILoggerFactory? logger,
            ILBankSocketClient? socketClient) : base(logger, "LBank", "Spot", symbol)
        {
            var options = LBankOrderBookOptions.Default.Copy();
            if (optionsDelegate != null)
                optionsDelegate(options);
            Initialize(options);

            _strictLevels = false;
            _sequencesAreConsecutive = true;

            Levels = options?.Limit;
            _initialDataTimeout = options?.InitialDataTimeout ?? TimeSpan.FromSeconds(30);
            _clientOwner = socketClient == null;
            _socketClient = socketClient ?? new LBankSocketClient();
        }

        /// <inheritdoc />
        protected override async Task<CallResult<UpdateSubscription>> DoStartAsync(CancellationToken ct)
        {
            var subResult = await _socketClient.SpotApi.SubscribeToOrderBookUpdatesAsync(Symbol, Levels ?? 50, HandleUpdate).ConfigureAwait(false);
            if (!subResult.Success)
                return CallResult.Fail<UpdateSubscription>(subResult.Error!);

            Status = OrderBookStatus.Syncing;

            var setResult = await WaitForSetOrderBookAsync(_initialDataTimeout, ct).ConfigureAwait(false);
            if (!setResult.Success)
                await subResult.Data.CloseAsync().ConfigureAwait(false);

            return setResult.Success ? CallResult.Ok(subResult.Data) : CallResult.Fail<UpdateSubscription>(setResult.Error!);
        }

        private void HandleUpdate(DataEvent<LBankOrderBookUpdate> @event)
        {
            SetSnapshot(@event.DataTime!.Value.Ticks, @event.Data.Bids, @event.Data.Asks, @event.DataTime, @event.DataTimeLocal);
        }

        /// <inheritdoc />
        protected override void DoReset()
        {
        }

        /// <inheritdoc />
        protected override async Task<CallResult> DoResyncAsync(CancellationToken ct)
        {
            return await WaitForSetOrderBookAsync(_initialDataTimeout, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            if (_clientOwner)
            {
                _socketClient?.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
