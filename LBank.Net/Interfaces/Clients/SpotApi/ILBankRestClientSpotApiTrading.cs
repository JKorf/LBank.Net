using CryptoExchange.Net.Objects;
using LBank.Net.Enums;
using LBank.Net.Objects.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LBank.Net.Interfaces.Clients.SpotApi
{
    /// <summary>
    /// LBank Spot trading endpoints, placing and managing orders.
    /// </summary>
    public interface ILBankRestClientSpotApiTrading
    {
        /// <summary>
        /// Place a new order
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.lbank.com/docs/#place-an-order" /><br />
        /// Endpoint:<br />
        /// POST /v2/supplement/create_order.do<br />
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>symbol</c>"] The symbol, for example `ETHUSDT`</param>
        /// <param name="orderType">["<c>type</c>"] Order type and side</param>
        /// <param name="quantity">["<c>amount</c>"] Order quantity. For market buy orders in quote asset</param>
        /// <param name="price">["<c>price</c>"] Limit price</param>
        /// <param name="clientOrderId">["<c>custom_id</c>"] Client order id</param>
        /// <param name="receiveWindow">["<c>window</c>"] Receive window</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<LBankOrderResult>> PlaceOrderAsync(
            string symbol,
            OrderType orderType,
            decimal quantity,
            decimal? price = null,
            string? clientOrderId = null,
            int? receiveWindow = null,
            CancellationToken ct = default);

        /// <summary>
        /// Cancel an active order
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.lbank.com/docs/#cancel-order" /><br />
        /// Endpoint:<br />
        /// POST /v2/supplement/cancel_order.do<br />
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>symbol</c>"] The symbol, for example `eth_usdt`</param>
        /// <param name="orderId">["<c>orderId</c>"] Cancel by order id, either this or clientOrderId should be provided</param>
        /// <param name="clientOrderId">["<c>origClientOrderId</c>"] Cancel by client order id, either this or orderId should be provided</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<LBankOrder>> CancelOrderAsync(
            string symbol,
            string? orderId = null,
            string? clientOrderId = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get orders
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.lbank.com/docs/#query-all-orders" /><br />
        /// Endpoint:<br />
        /// POST /v2/spot/trade/orders_info_history.do<br />
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>symbol</c>"] The symbol, for example `eth_usdt`</param>
        /// <param name="page">["<c>current_page</c>"] Page</param>
        /// <param name="pageSize">["<c>page_length</c>"] Page size, max 200</param>
        /// <param name="status">["<c>status</c>"] Filter by status</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<LBankOrderPage>> GetOrdersAsync(
            string symbol,
            int page,
            int pageSize,
            OrderStatus? status = null,
            CancellationToken ct = default);

        /// <summary>
        /// Cancel all orders on a symbol
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.lbank.com/docs/#cancel-all-pending-orders-for-a-single-trading-pair" /><br />
        /// Endpoint:<br />
        /// POST /v2/supplement/cancel_order_by_symbol.do<br />
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>symbol</c>"] The symbol, for example `eth_usdt`</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<LBankOrder[]>> CancelAllOrdersAsync(string symbol, CancellationToken ct = default);

        /// <summary>
        /// Get order by id
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.lbank.com/docs/#query-order" /><br />
        /// Endpoint:<br />
        /// POST /v2/spot/trade/orders_info.do<br />
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>symbol</c>"] The symbol, for example `eth_usdt`</param>
        /// <param name="orderId">["<c>orderId</c>"] Cancel by order id, either this or clientOrderId should be provided</param>
        /// <param name="clientOrderId">["<c>origClientOrderId</c>"] Cancel by client order id, either this or orderId should be provided</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<LBankOrder>> GetOrderAsync(
            string symbol,
            string? orderId = null,
            string? clientOrderId = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get open orders
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.lbank.com/docs/#current-pending-order" /><br />
        /// Endpoint:<br />
        /// GET /v2/supplement/orders_info_no_deal.do<br />
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>symbol</c>"] The symbol, for example `eth_usdt`</param>
        /// <param name="page">["<c>current_page</c>"] Page</param>
        /// <param name="pageSize">["<c>page_length</c>"] Page size, max 200</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<LBankOrderPage>> GetOpenOrdersAsync(
            string symbol,
            int page,
            int pageSize,
            CancellationToken ct = default);

        /// <summary>
        /// Get user trades
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.lbank.com/docs/#historical-transaction-details" /><br />
        /// Endpoint:<br />
        /// POST /v2/supplement/transaction_history.do<br />
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>symbol</c>"] The symbol, for example `eth_usdt`</param>
        /// <param name="fromId">["<c>fromId</c>"] Filter by id</param>
        /// <param name="startTime">["<c>startTime</c>"] Filter by start time</param>
        /// <param name="endTime">["<c>endTime</c>"] Filter by end time</param>
        /// <param name="limit">["<c>limit</c>"] Max number of results, max 100</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<LBankUserTrade[]>> GetUserTradesAsync(
            string symbol,
            string? fromId = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            int? limit = null,
            CancellationToken ct = default);

    }
}
