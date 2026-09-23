using CryptoExchange.Net.Objects;
using LBank.Net.Enums;
using LBank.Net.Interfaces.Clients.SpotApi;
using LBank.Net.Objects.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace LBank.Net.Clients.SpotApi
{
    /// <inheritdoc />
    internal class LBankRestClientSpotApiTrading : ILBankRestClientSpotApiTrading
    {
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();
        private readonly LBankRestClientSpotApi _baseClient;
        private readonly ILogger _logger;

        internal LBankRestClientSpotApiTrading(ILogger logger, LBankRestClientSpotApi baseClient)
        {
            _baseClient = baseClient;
            _logger = logger;
        }

        #region Place Order

        /// <inheritdoc />
        public async Task<HttpResult<LBankOrderResult>> PlaceOrderAsync(
            string symbol,
            OrderType orderType,
            decimal quantity,
            decimal? price = null,
            string? clientOrderId = null,
            int? receiveWindow = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(LBankExchange._parameterSerializationSettings);
            parameters.Add("symbol", symbol);
            parameters.Add("type", orderType);
            if (orderType == OrderType.BuyMarket)
            {
                // For market buy orders the quote quantity should be passed in price
                parameters.Add("price", quantity);
            }
            else
            {
                parameters.Add("amount", quantity);
                parameters.Add("price", price);
            }
            parameters.Add("custom_id", clientOrderId);
            parameters.Add("window", receiveWindow);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/v2/supplement/create_order.do", LBankExchange.RateLimiter.OrderApi, 1, true);
            var result = await _baseClient.SendAsync<LBankOrderResult>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Cancel Order

        /// <inheritdoc />
        public async Task<HttpResult<LBankOrder>> CancelOrderAsync(
            string symbol,
            string? orderId = null,
            string? clientOrderId = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(LBankExchange._parameterSerializationSettings);
            parameters.Add("symbol", symbol);
            parameters.Add("orderId", orderId);
            parameters.Add("origClientOrderId", clientOrderId);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/v2/supplement/cancel_order.do", LBankExchange.RateLimiter.OrderApi, 1, true);
            var result = await _baseClient.SendAsync<LBankOrder>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Orders

        /// <inheritdoc />
        public async Task<HttpResult<LBankOrderPage>> GetOrdersAsync(
            string symbol,
            int page,
            int pageSize,
            OrderStatus? status = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(LBankExchange._parameterSerializationSettings);
            parameters.Add("symbol", symbol);
            parameters.Add("current_page", page);
            parameters.Add("page_length", pageSize);
            parameters.Add("status", status);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/v2/spot/trade/orders_info_history.do", LBankExchange.RateLimiter.RestApi, 1, true);
            var result = await _baseClient.SendAsync<LBankOrderPage>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Open Orders

        /// <inheritdoc />
        public async Task<HttpResult<LBankOrderPage>> GetOpenOrdersAsync(
            string symbol,
            int page,
            int pageSize,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(LBankExchange._parameterSerializationSettings);
            parameters.Add("symbol", symbol);
            parameters.Add("current_page", page);
            parameters.Add("page_length", pageSize);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/v2/supplement/orders_info_no_deal.do", LBankExchange.RateLimiter.RestApi, 1, true);
            var result = await _baseClient.SendAsync<LBankOrderPage>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Order

        /// <inheritdoc />
        public async Task<HttpResult<LBankOrder>> GetOrderAsync(
            string symbol,
            string? orderId = null,
            string? clientOrderId = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(LBankExchange._parameterSerializationSettings);
            parameters.Add("symbol", symbol);
            parameters.Add("orderId", orderId);
            parameters.Add("origClientOrderId", clientOrderId);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/v2/spot/trade/orders_info.do", LBankExchange.RateLimiter.RestApi, 1, true);
            var result = await _baseClient.SendAsync<LBankOrder>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion


        #region Cancel All Orders

        /// <inheritdoc />
        public async Task<HttpResult<LBankOrder[]>> CancelAllOrdersAsync(string symbol, CancellationToken ct = default)
        {
            var parameters = new Parameters(LBankExchange._parameterSerializationSettings);
            parameters.Add("symbol", symbol);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/v2/supplement/cancel_order_by_symbol.do", LBankExchange.RateLimiter.OrderApi, 1, true);
            var result = await _baseClient.SendAsync<LBankOrder[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get User Trades

        /// <inheritdoc />
        public async Task<HttpResult<LBankUserTrade[]>> GetUserTradesAsync(
            string symbol,
            string? fromId = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            int? limit = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(LBankExchange._parameterSerializationSettings);
            parameters.Add("symbol", symbol);
            parameters.Add("fromId", fromId);
            if (startTime != null) // time is in UTC + 8 on the server
                parameters.Add("startTime", startTime.Value.AddHours(-8).ToString("yyyy-MM-dd HH:mm:ss"));
            if (endTime != null) // time is in UTC + 8 on the server
                parameters.Add("endTime", endTime.Value.AddHours(-8).ToString("yyyy-MM-dd HH:mm:ss"));
            parameters.Add("limit", limit);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/v2/supplement/transaction_history.do", LBankExchange.RateLimiter.RestApi, 1, true);
            var result = await _baseClient.SendAsync<LBankUserTrade[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion
    }
}
