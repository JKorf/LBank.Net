# LBank.Net usage

## Imports

```csharp
using System;
using System.Linq;
using System.Threading.Tasks;
using CryptoExchange.Net.SharedApis;
using LBank.Net;
using LBank.Net.Clients;
using LBank.Net.Enums;
using LBank.Net.Interfaces;
using LBank.Net.Interfaces.Clients;
using Microsoft.Extensions.DependencyInjection;
```

## Public client and results

```csharp
using var client = new LBankRestClient();
var result = await client.SpotApi.ExchangeData.GetTickersAsync("eth_usdt");
if (!result.Success)
{
    Console.WriteLine(result.Error);
    return;
}
foreach (var ticker in result.Data)
    Console.WriteLine(ticker.Ticker.LastPrice);
```

Ticker calls return arrays even when filtered to one symbol.

## Authentication and balances

```csharp
var key = Environment.GetEnvironmentVariable("LBANK_KEY")
    ?? throw new InvalidOperationException("API key is required");
var secret = Environment.GetEnvironmentVariable("LBANK_SECRET")
    ?? throw new InvalidOperationException("API secret is required");
using var client = new LBankRestClient(options =>
{
    options.ApiCredentials = new LBankCredentials(key, secret);
});
var result = await client.SpotApi.Account.GetUserAssetsAsync();
if (!result.Success)
{
    Console.WriteLine(result.Error);
    return;
}
foreach (var asset in result.Data.Where(x => x.Quantity != 0 || x.FrozenQuantity != 0))
    Console.WriteLine($"{asset.Asset}: available={asset.UsableQuantity}, frozen={asset.FrozenQuantity}");
```

RSA XML alternative: `new LBankCredentials().WithRSAXml(key, privateKey)`. On compatible targets PEM uses `WithRSAPem(key, privateKey)`. Use the type configured for the key; [credential considerations](safety.md#credentials-and-permissions) describes target restrictions.

## Metadata and candles

```csharp
var symbols = await client.SpotApi.ExchangeData.GetSymbolsAsync("eth_usdt");
if (!symbols.Success)
{
    Console.WriteLine(symbols.Error);
    return;
}
foreach (var symbol in symbols.Data)
    Console.WriteLine($"Min={symbol.MinOrderQuantity}, quantity decimals={symbol.QuantityAccuracy}, price decimals={symbol.PriceAccuracy}");
var klines = await client.SpotApi.ExchangeData.GetKlinesAsync(
    "eth_usdt", KlineInterval.OneMinute, limit: 100, afterTime: DateTime.UtcNow.AddHours(-2));
if (!klines.Success)
{
    Console.WriteLine(klines.Error);
    return;
}
Console.WriteLine(klines.Data.Length);
```

## Live order lifecycle

This assumes an authenticated client configured as above. It submits an executable order; consult [execution considerations](safety.md#orders-and-cancellation) before running. Use current symbol rules for actual sizing and pricing.

```csharp
var order = await client.SpotApi.Trading.PlaceOrderAsync(
    symbol: "eth_usdt", orderType: OrderType.BuyLimit, quantity: 0.01m, price: 2000m,
    clientOrderId: Guid.NewGuid().ToString("N"));
if (!order.Success)
{
    Console.WriteLine(order.Error);
    return;
}
var details = await client.SpotApi.Trading.GetOrderAsync("eth_usdt", orderId: order.Data.OrderId);
if (!details.Success)
    Console.WriteLine(details.Error);
else
    Console.WriteLine($"{details.Data.Status}: executed={details.Data.ExecutedQuantity}");
var cancel = await client.SpotApi.Trading.CancelOrderAsync("eth_usdt", orderId: order.Data.OrderId);
if (!cancel.Success)
    Console.WriteLine(cancel.Error);
```

## Public subscription lifecycle

```csharp
using var socketClient = new LBankSocketClient();
var subscription = await socketClient.SpotApi.SubscribeToTickerUpdatesAsync(
    "eth_usdt", update => Console.WriteLine(update.Data.LastPrice));
if (!subscription.Success)
{
    Console.WriteLine(subscription.Error);
    return;
}
try
{
    await Task.Delay(TimeSpan.FromSeconds(30));
}
finally
{
    await socketClient.UnsubscribeAsync(subscription.Data);
}
```

REST uses `KlineInterval`; socket candles require `StreamKlineInterval`. Order-book stream depth must be 10, 50, or 100.

## Private subscription with managed listen key

```csharp
var key = Environment.GetEnvironmentVariable("LBANK_KEY")
    ?? throw new InvalidOperationException("API key is required");
var secret = Environment.GetEnvironmentVariable("LBANK_SECRET")
    ?? throw new InvalidOperationException("API secret is required");
using var socketClient = new LBankSocketClient(options =>
{
    options.ApiCredentials = new LBankCredentials(key, secret);
});
var subscription = await socketClient.SpotApi.SubscribeToOrderUpdatesAsync(
    listenKey: null, update => Console.WriteLine(update.Data.Status));
if (!subscription.Success)
{
    Console.WriteLine(subscription.Error);
    return;
}
try
{
    await Task.Delay(TimeSpan.FromSeconds(30));
}
finally
{
    await socketClient.UnsubscribeAsync(subscription.Data);
}
```

## Shared API V2

```csharp
IGetTickerRest ticker = client.SpotApi.SharedApi;
var result = await ticker.GetTickerAsync(new GetTickerRequest(
    new SharedSymbol(TradingMode.Spot, "ETH", "USDT")));
if (!result.Success)
{
    Console.WriteLine(result.Error);
    return;
}
Console.WriteLine(result.Data.LastPrice);
```

Load symbol metadata before reading the catalog:

```csharp
IGetSpotSymbolsRest symbolsClient = client.SpotApi.SharedApi;
var result = await symbolsClient.GetSpotSymbolsAsync(new GetSymbolsRequest());
if (!result.Success)
{
    Console.WriteLine(result.Error);
    return;
}
foreach (var symbol in result.Data.Take(5))
    Console.WriteLine($"{symbol.Name}: {symbol.BaseAsset}/{symbol.QuoteAsset}");
var catalog = symbolsClient.SpotSymbolCatalog;
```

## Dependency injection and dynamic lookup

```csharp
var services = new ServiceCollection();
services.AddLBank();
using var provider = services.BuildServiceProvider();
var aggregate = provider.GetRequiredService<ILBankSharedApiClient>();
var match = aggregate.GetCapability(
    SharedCapabilities.Tickers.GetTicker.Rest, tradingMode: TradingMode.Spot);
if (match is null)
    return;
var result = await match.Capability.GetTickerAsync(new GetTickerRequest(
    new SharedSymbol(TradingMode.Spot, "ETH", "USDT")));
if (!result.Success)
{
    Console.WriteLine(result.Error);
    return;
}
Console.WriteLine(result.Data.LastPrice);
```

Set `options.ApiCredentials` via `services.AddLBank(options => { ... })` for authenticated consumers; REST/socket credentials can also be set independently. Inject `ILBankRestClient`, `ILBankSocketClient`, the aggregate, or a registered narrow capability. [API routing](api-surfaces.md#shared-api-v2) explains selection when multiple exchanges are registered.

## Managed local order book

```csharp
var services = new ServiceCollection();
services.AddLBank();
using var provider = services.BuildServiceProvider();
var factory = provider.GetRequiredService<ILBankOrderBookFactory>();
var book = factory.CreateSpot("eth_usdt", options => options.Limit = 50);
var start = await book.StartAsync();
if (!start.Success)
{
    Console.WriteLine(start.Error);
    return;
}
try
{
    var snapshot = book.Book;
    Console.WriteLine(snapshot.bids.FirstOrDefault()?.Price);
}
finally
{
    await book.StopAsync();
}
```

## Complete programs (checkout-only)

[Examples](../../../../Examples/ai-friendly/README.md) includes Spot reads, live order lifecycle, subscriptions, DI/local books, Shared API V2, and public-read retry patterns. These links require a checkout; the snippets above work with the installed package.
