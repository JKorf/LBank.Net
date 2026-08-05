// 01-spot-quickstart.cs
//
// Demonstrates: LBank public Spot REST data and authenticated balances.
//
// Setup: dotnet add package LBank.Net

using LBank.Net;
using LBank.Net.Clients;
using LBank.Net.Enums;

var client = new LBankRestClient(options =>
{
    // Credentials are only required for Account and Trading endpoints.
    options.ApiCredentials = new LBankCredentials("API_KEY", "API_SECRET");
});

// Native LBank symbols use lowercase base_quote formatting.
const string symbol = "eth_usdt";

var symbols = await client.SpotApi.ExchangeData.GetSymbolsAsync(symbol);
if (!symbols.Success)
{
    Console.WriteLine($"Symbols failed: {symbols.Error}");
    return;
}

var ethUsdt = symbols.Data.SingleOrDefault();
if (ethUsdt != null)
{
    Console.WriteLine(
        $"{ethUsdt.Symbol}: min quantity={ethUsdt.MinOrderQuantity}, " +
        $"quantity decimals={ethUsdt.QuantityAccuracy}, price decimals={ethUsdt.PriceAccuracy}");
}

var tickers = await client.SpotApi.ExchangeData.GetTickersAsync(symbol);
if (!tickers.Success)
{
    Console.WriteLine($"Ticker failed: {tickers.Error}");
    return;
}

var ticker = tickers.Data.SingleOrDefault();
if (ticker != null)
    Console.WriteLine($"{ticker.Symbol}: last={ticker.Ticker.LastPrice}, volume={ticker.Ticker.Volume}");

var orderBook = await client.SpotApi.ExchangeData.GetOrderBookAsync(symbol, limit: 20);
if (!orderBook.Success)
{
    Console.WriteLine($"Order book failed: {orderBook.Error}");
    return;
}

Console.WriteLine(
    $"{symbol} book levels: bids={orderBook.Data.Bids.Length}, asks={orderBook.Data.Asks.Length}");

var candles = await client.SpotApi.ExchangeData.GetKlinesAsync(
    symbol,
    KlineInterval.OneMinute,
    limit: 5,
    afterTime: DateTime.UtcNow.AddMinutes(-10));

if (candles.Success)
{
    foreach (var candle in candles.Data)
        Console.WriteLine($"{candle.OpenTime:u} open={candle.OpenPrice} close={candle.ClosePrice}");
}

var balances = await client.SpotApi.Account.GetUserAssetsAsync();
if (!balances.Success)
{
    Console.WriteLine($"Balances failed: {balances.Error}");
    return;
}

foreach (var balance in balances.Data.Where(x => x.Asset is "eth" or "usdt"))
{
    Console.WriteLine(
        $"{balance.Asset}: total={balance.Quantity}, available={balance.UsableQuantity}, " +
        $"frozen={balance.FrozenQuantity}");
}
