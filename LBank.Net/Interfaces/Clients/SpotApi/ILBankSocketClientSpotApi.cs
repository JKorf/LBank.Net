using CryptoExchange.Net.Objects;
using System;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Objects.Sockets;
using LBank.Net.Objects.Models;
using CryptoExchange.Net.Interfaces.Clients;
using LBank.Net.Enums;

namespace LBank.Net.Interfaces.Clients.SpotApi
{
    /// <summary>
    /// LBank Spot streams
    /// </summary>
    public interface ILBankSocketClientSpotApi : ISocketApiClient<LBankCredentials>, IDisposable
    {
        /// <summary>
        /// 
        /// <para><a href="XXX" /></para>
        /// </summary>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(string symbol, Action<DataEvent<LBankTradeUpdate>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="interval"></param>
        /// <param name="onMessage"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol, StreamKlineInterval interval, Action<DataEvent<LBankKlineUpdate>> onMessage, CancellationToken ct = default);


        Task<WebSocketResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(string symbol, int depth, Action<DataEvent<LBankOrderBookUpdate>> onMessage, CancellationToken ct = default);

        Task<WebSocketResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(string symbol, Action<DataEvent<LBankTickerUpdate>> onMessage, CancellationToken ct = default);

        Task<WebSocketResult<UpdateSubscription>> SubscribeToOrderUpdatesAsync(string? listenKey, Action<DataEvent<LBankOrderUpdate>> onMessage, CancellationToken ct = default);

        Task<WebSocketResult<UpdateSubscription>> SubscribeToBalanceUpdatesAsync(string? listenKey, Action<DataEvent<LBankBalanceUpdate>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Get the shared socket requests client. This interface is shared with other exchanges to allow for a common implementation for different exchanges.
        /// </summary>
        public ILBankSocketClientSpotApiShared SharedClient { get; }
    }
}
