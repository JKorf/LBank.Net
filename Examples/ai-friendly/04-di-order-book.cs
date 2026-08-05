// 04-di-order-book.cs
//
// Demonstrates: dependency injection and LBank's synchronized local Spot order book.
//
// Setup:
//   dotnet add package JKorf.LBank.Net
//   dotnet add package Microsoft.Extensions.DependencyInjection

using LBank.Net.Interfaces;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

services.AddLBank(options =>
{
    // Configure options.ApiCredentials here when resolving authenticated clients.
    options.Rest.RequestTimeout = TimeSpan.FromSeconds(15);
});

await using var provider = services.BuildServiceProvider();

var bookFactory = provider.GetRequiredService<ILBankOrderBookFactory>();
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

var snapshot = book.Book;
Console.WriteLine(
    $"{book.Symbol}: bids={snapshot.bids.Count()}, asks={snapshot.asks.Count()}, state={book.Status}");

var bestBid = snapshot.bids.FirstOrDefault();
var bestAsk = snapshot.asks.FirstOrDefault();
Console.WriteLine($"Best bid={bestBid?.Price}, best ask={bestAsk?.Price}");

await book.StopAsync();
