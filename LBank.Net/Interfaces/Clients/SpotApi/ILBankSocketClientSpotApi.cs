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
        /// Subscribe to trade updates for a symbol
        /// <para><a href="https://www.lbank.com/docs/#trade-record" /></para>
        /// </summary>
        /// <param name="symbol">The symbols to subscribe to, for example `eth_usdt`</param>
        /// <param name="onMessage">The data handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected and to unsubscribe</returns>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(string symbol, Action<DataEvent<LBankTradeUpdate>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to kline updates for a symbol
        /// <para><a href="https://www.lbank.com/docs/#subscription-of-k-line-data" /></para>
        /// </summary>
        /// <param name="symbol">The symbols to subscribe to, for example `eth_usdt`</param>
        /// <param name="interval">The kline interval</param>
        /// <param name="onMessage">The data handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected and to unsubscribe</returns>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol, StreamKlineInterval interval, Action<DataEvent<LBankKlineUpdate>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to snapshot updates for an order book
        /// <para><a href="https://www.lbank.com/docs/#market-depth" /></para>
        /// </summary>
        /// <param name="symbol">The symbols to subscribe to, for example `eth_usdt`</param>
        /// <param name="depth">Book depth, 10, 50 or 100</param>
        /// <param name="onMessage">The data handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected and to unsubscribe</returns>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(string symbol, int depth, Action<DataEvent<LBankOrderBookUpdate>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to ticker updates for a symbol
        /// <para><a href="https://www.lbank.com/docs/#market" /></para>
        /// </summary>
        /// <param name="symbol">The symbols to subscribe to, for example `eth_usdt`</param>
        /// <param name="onMessage">The data handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected and to unsubscribe</returns>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(string symbol, Action<DataEvent<LBankTickerUpdate>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to user order updates
        /// <para><a href="https://www.lbank.com/docs/#update-subscribed-orders" /></para>
        /// </summary>
        /// <param name="listenKey">The listen key for the user account. If not provided listen key management will be handled internally</param>
        /// <param name="onMessage">The data handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected and to unsubscribe</returns>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToOrderUpdatesAsync(string? listenKey, Action<DataEvent<LBankOrderUpdate>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to user balance updates
        /// <para><a href="https://www.lbank.com/docs/#update-subscribed-asset" /></para>
        /// </summary>
        /// <param name="listenKey">The listen key for the user account. If not provided listen key management will be handled internally</param>
        /// <param name="onMessage">The data handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected and to unsubscribe</returns>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToBalanceUpdatesAsync(string? listenKey, Action<DataEvent<LBankBalanceUpdate>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Get the shared socket requests client. This interface is shared with other exchanges to allow for a common implementation for different exchanges.
        /// </summary>
        public ILBankSocketClientSpotApiShared SharedClient { get; }
    }
}
