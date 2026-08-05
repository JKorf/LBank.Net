// 05-error-handling.cs
//
// Demonstrates: LBank REST and WebSocket result handling with transient retries.
//
// Setup: dotnet add package LBank.Net

using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using LBank.Net.Clients;

var restClient = new LBankRestClient();
var socketClient = new LBankSocketClient();

// API and network failures are returned as HttpResult<T>; inspect Success before Data.
var book = await WithRetry(
    () => restClient.SpotApi.ExchangeData.GetOrderBookAsync("eth_usdt", limit: 20));

if (!EnsureSuccess(book, "load order book"))
    return;

Console.WriteLine($"Best bid: {book.Data.Bids.FirstOrDefault()?.Price}");

// Subscription setup failures are returned as WebSocketResult<T>.
var subscription = await socketClient.SpotApi.SubscribeToTradeUpdatesAsync(
    "eth_usdt",
    update =>
    {
        Console.WriteLine($"Trade: {update.Data.Quantity} @ {update.Data.Price}");
    });

if (!EnsureSocketSuccess(subscription, "subscribe to trades"))
    return;

await socketClient.UnsubscribeAsync(subscription.Data);

static async Task<HttpResult<T>> WithRetry<T>(
    Func<Task<HttpResult<T>>> call,
    int maxAttempts = 3)
{
    HttpResult<T> last = default!;

    for (var attempt = 1; attempt <= maxAttempts; attempt++)
    {
        last = await call();
        if (last.Success || last.Error?.IsTransient != true)
            return last;

        await Task.Delay(TimeSpan.FromMilliseconds(250 * Math.Pow(2, attempt - 1)));
    }

    return last;
}

static bool EnsureSuccess<T>(HttpResult<T> result, string action)
{
    if (result.Success)
        return true;

    Console.WriteLine($"Could not {action}: {result.Error}");
    return false;
}

static bool EnsureSocketSuccess<T>(WebSocketResult<T> result, string action)
{
    if (result.Success)
        return true;

    Console.WriteLine($"Could not {action}: {result.Error}");
    return false;
}
