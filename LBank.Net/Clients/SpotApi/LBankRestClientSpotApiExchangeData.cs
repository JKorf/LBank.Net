using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using LBank.Net.Interfaces.Clients.SpotApi;
using LBank.Net.Objects.Models;
using LBank.Net.Enums;
using CryptoExchange.Net.Objects.Errors;

namespace LBank.Net.Clients.SpotApi
{
    /// <inheritdoc />
    internal class LBankRestClientSpotApiExchangeData : ILBankRestClientSpotApiExchangeData
    {
        private readonly LBankRestClientSpotApi _baseClient;
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();

        internal LBankRestClientSpotApiExchangeData(ILogger logger, LBankRestClientSpotApi baseClient)
        {
            _baseClient = baseClient;
        }

        #region Get Server Time

        /// <inheritdoc />
        public async Task<HttpResult<DateTime>> GetServerTimeAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/v2/timestamp.do", LBankExchange.RateLimiter.RestApi, 1, false);
            var result = await _baseClient.SendAsync<DateTime>(request, null, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Available Symbols

        /// <inheritdoc />
        public async Task<HttpResult<string[]>> GetAvailableSymbolsAsync(CancellationToken ct = default)
        {
            var parameters = new Parameters(LBankExchange._parameterSerializationSettings);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/v2/currencyPairs.do", LBankExchange.RateLimiter.RestApi, 1, false);
            var result = await _baseClient.SendAsync<string[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Symbols

        /// <inheritdoc />
        public async Task<HttpResult<LBankSymbol[]>> GetSymbolsAsync(string? symbol = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(LBankExchange._parameterSerializationSettings);
            parameters.Add("symbol", symbol);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/v2/accuracy.do", LBankExchange.RateLimiter.RestApi, 1, false);
            var result = await _baseClient.SendAsync<LBankSymbol[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Assets

        /// <inheritdoc />
        public async Task<HttpResult<LBankAsset[]>> GetAssetsAsync(string asset, CancellationToken ct = default)
        {
            var parameters = new Parameters(LBankExchange._parameterSerializationSettings);
            parameters.Add("assetCode", asset);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/v2/assetConfigs.do", LBankExchange.RateLimiter.RestApi, 1, false);
            var result = await _baseClient.SendAsync<LBankAsset[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Order Book

        /// <inheritdoc />
        public async Task<HttpResult<LBankOrderBook>> GetOrderBookAsync(
            string symbol,
            int? limit = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(LBankExchange._parameterSerializationSettings);
            parameters.Add("symbol", symbol);
            parameters.Add("size", limit ?? 20);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/v2/depth.do", LBankExchange.RateLimiter.RestApi, 1, false);
            var result = await _baseClient.SendAsync<LBankOrderBook>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Price

        /// <inheritdoc />
        public async Task<HttpResult<LBankPrice[]>> GetPriceAsync(string? symbol = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(LBankExchange._parameterSerializationSettings);
            parameters.Add("symbol", symbol);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/v2/supplement/ticker/price.do", LBankExchange.RateLimiter.RestApi, 1, false);
            var result = await _baseClient.SendAsync<LBankPrice[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Book Ticker

        /// <inheritdoc />
        public async Task<HttpResult<LBankBookTicker>> GetBookTickerAsync(string symbol, CancellationToken ct = default)
        {
            var parameters = new Parameters(LBankExchange._parameterSerializationSettings);
            parameters.Add("symbol", symbol);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/v2/supplement/ticker/bookTicker.do", LBankExchange.RateLimiter.RestApi, 1, false);
            var result = await _baseClient.SendAsync<LBankBookTicker>(request, parameters, ct).ConfigureAwait(false);
            if (!result.Success)
                return result;

            if (result.Data == null)
                return HttpResult.Fail<LBankBookTicker>(result, new ServerError(ErrorType.UnknownSymbol, "Symbol not found"));

            return result;
        }

        #endregion

        #region Get Tickers

        /// <inheritdoc />
        public async Task<HttpResult<LBankSymbolTicker[]>> GetTickersAsync(string? symbol = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(LBankExchange._parameterSerializationSettings);
            parameters.Add("symbol", symbol ?? "all");
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/v2/ticker/24hr.do", LBankExchange.RateLimiter.RestApi, 1, false);
            var result = await _baseClient.SendAsync<LBankSymbolTicker[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Leveraged Token Tickers

        /// <inheritdoc />
        public async Task<HttpResult<LBankSymbolTicker[]>> GetLeveragedTokenTickersAsync(string? symbol = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(LBankExchange._parameterSerializationSettings);
            parameters.Add("symbol", symbol ?? "all");
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/v2/etfTicker/24hr.do", LBankExchange.RateLimiter.RestApi, 1, false);
            var result = await _baseClient.SendAsync<LBankSymbolTicker[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Trades

        /// <inheritdoc />
        public async Task<HttpResult<LBankTrade[]>> GetTradesAsync(
            string symbol,
            int? limit = null,
            DateTime? afterTime = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(LBankExchange._parameterSerializationSettings);
            parameters.Add("symbol", symbol);
            parameters.Add("size", limit ?? 100);
            parameters.Add("time", afterTime);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/v2/supplement/trades.do", LBankExchange.RateLimiter.RestApi, 1, false);
            var result = await _baseClient.SendAsync<LBankTrade[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Klines

        /// <inheritdoc />
        public async Task<HttpResult<LBankKline[]>> GetKlinesAsync(
            string symbol,
            KlineInterval interval,
            int limit,
            DateTime afterTime,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(LBankExchange._parameterSerializationSettings);
            parameters.Add("symbol", symbol);
            parameters.Add("type", interval);
            parameters.Add("size", limit);
            parameters.Add("time", afterTime, DateTimeSerialization.SecondsString);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/v2/kline.do", LBankExchange.RateLimiter.RestApi, 1, false);
            var result = await _baseClient.SendAsync<LBankKline[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

    }
}
