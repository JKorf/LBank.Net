using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.SharedApis;
using LBank.Net.Enums;
using LBank.Net.Interfaces.Clients.SpotApi;
using LBank.Net.Objects.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LBank.Net.Clients.SpotApi
{
    internal partial class LBankSocketClientSpotSharedApi
    {
        #region Subscribe Trades

        public SubscribeTradeOptions SubscribeTradeOptions { get; } = new SubscribeTradeOptions(_exchangeName, false);
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(SubscribeTradeRequest request, Action<DataEvent<SharedTrade[]>> handler, CancellationToken ct)
        {
            var validationError = SubscribeTradeOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(_exchangeName, validationError);

            var symbols = request.SymbolName(FormatSymbol);
            var result = await _api.SubscribeToTradeUpdatesAsync(symbols, update => handler(update.ToType<SharedTrade[]>([
                new SharedTrade(
                    ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, update.Symbol),
                    update.Symbol!,
                    new SharedOrderQuantity(update.Data.Quantity, update.Data.QuoteQuantity),
                    update.Data.Price,
                    update.Data.TimestampUtc)
                {
                    Side = update.Data.Side == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell
                }])), ct).ConfigureAwait(false);
            return result;
        }

        #endregion
    }
}
