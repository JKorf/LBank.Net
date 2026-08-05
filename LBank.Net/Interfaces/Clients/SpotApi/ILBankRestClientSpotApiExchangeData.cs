using System;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Objects;
using LBank.Net.Enums;
using LBank.Net.Objects.Models;

namespace LBank.Net.Interfaces.Clients.SpotApi
{
    /// <summary>
    /// LBank Spot exchange data endpoints. Exchange data includes market data (tickers, order books, etc) and system status.
    /// </summary>
    public interface ILBankRestClientSpotApiExchangeData
    {
        /// <summary>
        /// Get server timestamp
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.lbank.com/docs/#get-timestamp" /><br />
        /// Endpoint:<br />
        /// GET /v2/timestamp.do<br />
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<DateTime>> GetServerTimeAsync(CancellationToken ct = default);

        /// <summary>
        /// Get list of available symbols
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.lbank.com/docs/#available-trading-pairs" /><br />
        /// Endpoint:<br />
        /// GET /v2/currencyPairs.do<br />
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<string[]>> GetAvailableSymbolsAsync(CancellationToken ct = default);

        /// <summary>
        /// Get supported symbols
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.lbank.com/docs/#trading-pairs" /><br />
        /// Endpoint:<br />
        /// GET /v2/accuracy.do<br />
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>symbol</c>"] Filter by symbol, for example `eth_usdt`</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<LBankSymbol[]>> GetSymbolsAsync(string? symbol = null, CancellationToken ct = default);

        /// <summary>
        /// Get asset info and supported networks
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.lbank.com/docs/#coin-information" /><br />
        /// Endpoint:<br />
        /// GET /v2/assetConfigs.do<br />
        /// </para>
        /// </summary>
        /// <param name="asset">["<c>assetCode</c>"] Filter by asset, for example `eth`</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<LBankAsset[]>> GetAssetsAsync(string asset, CancellationToken ct = default);

        /// <summary>
        /// Get order book snapshot
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.lbank.com/docs/#depth-information" /><br />
        /// Endpoint:<br />
        /// GET /v2/depth.do<br />
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>symbol</c>"] The symbol, for example `eth_usdt`</param>
        /// <param name="limit">["<c>size</c>"] Number of entries, max 200</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<LBankOrderBook>> GetOrderBookAsync(
            string symbol,
            int? limit = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get last trade price
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.lbank.com/docs/#get-the-latest-price-of-the-trading-pair" /><br />
        /// Endpoint:<br />
        /// GET /v2/supplement/ticker/price.do<br />
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>symbol</c>"] Filter by symbol, for example `ETHUSDT`</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<LBankPrice[]>> GetPriceAsync(string? symbol = null, CancellationToken ct = default);

        /// <summary>
        /// Get best bid and ask price
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.lbank.com/docs/#symbol-order-book-ticker" /><br />
        /// Endpoint:<br />
        /// GET /v2/supplement/ticker/bookTicker.do<br />
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>symbol</c>"] The symbol, for example `eth_usdt`</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<LBankBookTicker>> GetBookTickerAsync(string symbol, CancellationToken ct = default);

        /// <summary>
        /// Get 24hr ticker price change statistics
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.lbank.com/docs/#24hr-ticker" /><br />
        /// Endpoint:<br />
        /// GET /v2/ticker/24hr.do<br />
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>symbol</c>"] Filter by symbol, for example `eth_usdt`</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<LBankSymbolTicker[]>> GetTickersAsync(string? symbol = null, CancellationToken ct = default);

        /// <summary>
        /// Get 24hr ticker price change statistics for leveraged tokens
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.lbank.com/docs/#leveraged-tokens-24hr-ticker" /><br />
        /// Endpoint:<br />
        /// GET /v2/etfTicker/24hr.do<br />
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>symbol</c>"] Filter by symbol, for example `eth3l_usdt`</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<LBankSymbolTicker[]>> GetLeveragedTokenTickersAsync(string? symbol = null, CancellationToken ct = default);

        /// <summary>
        /// Get public trade history
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.lbank.com/docs/#recent-transactions-list" /><br />
        /// Endpoint:<br />
        /// GET /v2/supplement/trades.do<br />
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>symbol</c>"] The symbol, for example `eth_usdt`</param>
        /// <param name="limit">["<c>size</c>"] Max number of results, max 500</param>
        /// <param name="afterTime">["<c>time</c>"] Filter by end time</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<LBankTrade[]>> GetTradesAsync(
            string symbol,
            int? limit = null,
            DateTime? afterTime = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get kline/candlestick data
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.lbank.com/docs/#query-k-bar-data" /><br />
        /// Endpoint:<br />
        /// GET /v2/kline.do<br />
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>symbol</c>"] The symbol, for example `eth_usdt`</param>
        /// <param name="interval">["<c>type</c>"] Kline interval</param>
        /// <param name="limit">["<c>size</c>"] Max number of results, max 2000</param>
        /// <param name="afterTime">["<c>time</c>"] Filter by timestamp</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<LBankKline[]>> GetKlinesAsync(
            string symbol,
            KlineInterval interval,
            int limit,
            DateTime afterTime,
            CancellationToken ct = default);

    }
}
