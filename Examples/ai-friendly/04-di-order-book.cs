// 04-di-order-book.cs
//
// Demonstrates: dependency injection and LBank's synchronized local Spot order book.
//
// Setup:
//   dotnet add package LBank.Net
//   dotnet add package Microsoft.Extensions.DependencyInjection

using LBank.Net.Interfaces;
using LBank.Net.Interfaces.Clients;
using CryptoExchange.Net.SharedApis;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

services.AddLBank(options =>
{
    // Configure options.ApiCredentials here when resolving authenticated clients.
    options.Rest.RequestTimeout = TimeSpan.FromSeconds(15);
});

await using var provider = services.BuildServiceProvider();

var bookFactory = provider.GetRequiredService<ILBankOrderBookFactory>();
// Shared API V2: depend on a narrow capability for portable application code.
var shared = provider.GetRequiredService<ILBankSharedApiClient>();
IGetTickerRest tickerClient = shared.SpotRest;
var ticker = await tickerClient.GetTickerAsync(new GetTickerRequest(
    new SharedSymbol(TradingMode.Spot, "ETH", "USDT")));
if (!ticker.Success)
{
    Console.WriteLine(ticker.Error);
    return;
}
Console.WriteLine(ticker.Data.LastPrice);
IGetSpotSymbolsRest symbolsClient = shared.SpotRest;
var symbols = await symbolsClient.GetSpotSymbolsAsync(new GetSymbolsRequest());
if (!symbols.Success)
{
    Console.WriteLine(symbols.Error);
    return;
}
var catalog = symbolsClient.SpotSymbolCatalog;

var book = bookFactory.CreateSpot(
    "eth_usdt",
    options =>
    {
        // WebSocket depth accepts 10, 50, or 100.
        options.Limit = 50;
        options.InitialDataTimeout = TimeSpan.FromSeconds(15);
    });

var start = await book.StartAsync();
if (!start.Success)
{
    Console.WriteLine($"Could not start order book: {start.Error}");
    return;
}

try
{
    var snapshot = book.Book;
    Console.WriteLine(
        $"{book.Symbol}: bids={snapshot.bids.Count()}, asks={snapshot.asks.Count()}, state={book.Status}");

    var bestBid = snapshot.bids.FirstOrDefault();
    var bestAsk = snapshot.asks.FirstOrDefault();
    Console.WriteLine($"Best bid={bestBid?.Price}, best ask={bestAsk?.Price}");

}
finally
{
    await book.StopAsync();
}
