// 03-websocket.cs
//
// Demonstrates: LBank public and authenticated Spot WebSocket subscriptions.
//
// Setup: dotnet add package LBank.Net

using LBank.Net;
using LBank.Net.Clients;
using LBank.Net.Enums;

var socketClient = new LBankSocketClient(options =>
{
    // Only the private order subscription requires credentials.
    options.ApiCredentials = new LBankCredentials("API_KEY", "API_SECRET");
});

const string symbol = "eth_usdt";

var tickerSub = await socketClient.SpotApi.SubscribeToTickerUpdatesAsync(
    symbol,
    update =>
    {
        Console.WriteLine($"{update.Symbol} ticker: last={update.Data.LastPrice}");
    });

if (!tickerSub.Success)
{
    Console.WriteLine($"Ticker subscription failed: {tickerSub.Error}");
    return;
}

var bookSub = await socketClient.SpotApi.SubscribeToOrderBookUpdatesAsync(
    symbol,
    depth: 50, // LBank accepts 10, 50, or 100.
    update =>
    {
        var bestBid = update.Data.Bids.FirstOrDefault();
        var bestAsk = update.Data.Asks.FirstOrDefault();
        Console.WriteLine($"{update.Symbol} book: bid={bestBid?.Price}, ask={bestAsk?.Price}");
    });

if (!bookSub.Success)
{
    Console.WriteLine($"Order book subscription failed: {bookSub.Error}");
    await socketClient.UnsubscribeAsync(tickerSub.Data);
    return;
}

var klineSub = await socketClient.SpotApi.SubscribeToKlineUpdatesAsync(
    symbol,
    StreamKlineInterval.OneMinute,
    update =>
    {
        Console.WriteLine($"{update.Symbol} 1m close: {update.Data.ClosePrice}");
    });

if (!klineSub.Success)
{
    Console.WriteLine($"Kline subscription failed: {klineSub.Error}");
    await socketClient.UnsubscribeAsync(tickerSub.Data);
    await socketClient.UnsubscribeAsync(bookSub.Data);
    return;
}

// Passing null lets the authenticated socket client acquire and maintain a listen key.
var orderSub = await socketClient.SpotApi.SubscribeToOrderUpdatesAsync(
    listenKey: null,
    update =>
    {
        Console.WriteLine(
            $"{update.Data.Symbol} order {update.Data.OrderId}: {update.Data.Status}");
    });

if (!orderSub.Success)
{
    Console.WriteLine($"Private order subscription failed: {orderSub.Error}");
    await socketClient.UnsubscribeAsync(tickerSub.Data);
    await socketClient.UnsubscribeAsync(bookSub.Data);
    await socketClient.UnsubscribeAsync(klineSub.Data);
    return;
}

Console.WriteLine("Subscriptions active. Press Enter to unsubscribe.");
Console.ReadLine();

await socketClient.UnsubscribeAsync(tickerSub.Data);
await socketClient.UnsubscribeAsync(bookSub.Data);
await socketClient.UnsubscribeAsync(klineSub.Data);
await socketClient.UnsubscribeAsync(orderSub.Data);
