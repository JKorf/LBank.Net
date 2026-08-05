---
name: lbank-net
description: Use LBank.Net when generating C#/.NET code for the LBank Spot REST or WebSocket APIs, including market data, account and wallet operations, order management, subscriptions, local order books, trackers, dependency injection, or CryptoExchange.Net SharedApis.
---

# LBank.Net Skill

## Quick decision

Use `LBankRestClient` for REST and `LBankSocketClient` for WebSocket access. Do not generate raw `HttpClient` or `ClientWebSocket` integrations when LBank.Net already exposes the requested operation.

LBank.Net currently supports **Spot only**:

```csharp
restClient.SpotApi.ExchangeData
restClient.SpotApi.Account
restClient.SpotApi.Trading
socketClient.SpotApi
```

Do not invent `FuturesApi`, `DerivativesApi`, margin, or futures methods.

## Installation

```bash
dotnet add package LBank.Net
```

Targets: netstandard2.0, netstandard2.1, net8.0, net9.0, net10.0. Native AOT is supported on compatible targets.

## Symbols and enums

Native LBank symbols use lowercase `base_quote` formatting:

```csharp
const string symbol = "eth_usdt";
```

REST candles use `KlineInterval`; socket candles use `StreamKlineInterval`. They are different enum types.

LBank combines order side, execution type, and some time-in-force behavior in `OrderType`:

```csharp
OrderType.BuyLimit
OrderType.SellMarket
OrderType.BuyMaker
OrderType.SellIoc
OrderType.BuyFok
```

Do not generate separate `OrderSide` or `TimeInForce` arguments for `PlaceOrderAsync`.

## Client and result pattern

Public market data needs no credentials:

```csharp
using LBank.Net.Clients;

var client = new LBankRestClient();
var result = await client.SpotApi.ExchangeData.GetTickersAsync("eth_usdt");

if (!result.Success)
{
    Console.WriteLine(result.Error);
    return;
}

var ticker = result.Data.SingleOrDefault();
Console.WriteLine(ticker?.Ticker.LastPrice);
```

REST calls return `HttpResult<T>` or `HttpResult`. Socket subscriptions return `WebSocketResult<UpdateSubscription>`. Always check `.Success` before reading `.Data`.

## Authentication

Use `LBankCredentials`, not the base `ApiCredentials` type.

HMAC:

```csharp
var client = new LBankRestClient(options =>
{
    options.ApiCredentials = new LBankCredentials("API_KEY", "API_SECRET");
});
```

RSA is also supported through `WithRSAXml(...)`, and through `WithRSAPem(...)` on compatible targets:

```csharp
var credentials = new LBankCredentials()
    .WithRSAPem("API_KEY", "PRIVATE_KEY_PEM_OR_BASE64");
```

Never log or embed real credentials.

## Spot order pattern

```csharp
using LBank.Net;
using LBank.Net.Clients;
using LBank.Net.Enums;

var client = new LBankRestClient(options =>
{
    options.ApiCredentials = new LBankCredentials("API_KEY", "API_SECRET");
});

var order = await client.SpotApi.Trading.PlaceOrderAsync(
    symbol: "eth_usdt",
    orderType: OrderType.BuyLimit,
    quantity: 0.01m,
    price: 2000m);

if (!order.Success)
{
    Console.WriteLine(order.Error);
    return;
}

Console.WriteLine(order.Data.OrderId);
```

For `OrderType.BuyMarket`, `quantity` is denominated in the quote asset. A limit-style order needs `price`; a market order does not.

`GetOrderAsync` and `CancelOrderAsync` accept either `orderId` or `clientOrderId`. `GetOrdersAsync` and `GetOpenOrdersAsync` require `page` and `pageSize`.

The upstream LBank order API may not be publicly accessible to every account. Preserve result/error handling even when the code is structurally correct.

## WebSocket pattern

```csharp
using LBank.Net.Clients;

var socketClient = new LBankSocketClient();
var sub = await socketClient.SpotApi.SubscribeToTickerUpdatesAsync(
    "eth_usdt",
    update => Console.WriteLine(update.Data.LastPrice));

if (!sub.Success)
{
    Console.WriteLine(sub.Error);
    return;
}

await socketClient.UnsubscribeAsync(sub.Data);
```

Public subscriptions include trades, klines, order books, and tickers. Order book depth must be `10`, `50`, or `100`.

Private order and balance subscriptions accept a listen key. Passing `null` lets an authenticated socket client acquire and maintain one:

```csharp
var sub = await socketClient.SpotApi.SubscribeToOrderUpdatesAsync(
    listenKey: null,
    update => Console.WriteLine(update.Data.Status));
```

`LBankSocketOptions.UseV3` defaults to `true` for public streams. Set it to `false` only when the documented v2 socket behavior is specifically required. Private streams use the appropriate authenticated flow.

## Dependency injection and local order book

```csharp
using LBank.Net.Interfaces;
using Microsoft.Extensions.DependencyInjection;

services.AddLBank(options =>
{
    options.ApiCredentials = new LBankCredentials("API_KEY", "API_SECRET");
});
```

Inject `ILBankRestClient`, `ILBankSocketClient`, `ILBankOrderBookFactory`, or `ILBankTrackerFactory`.

```csharp
var factory = provider.GetRequiredService<ILBankOrderBookFactory>();
var book = factory.CreateSpot("eth_usdt", options => options.Limit = 50);

var start = await book.StartAsync();
if (start.Success)
{
    var snapshot = book.Book;
    await book.StopAsync();
}
```

The tracker factory can create Spot kline, trade, and authenticated user-data trackers.

## Multi-exchange SharedApis

Use the shared client for exchange-agnostic code:

```csharp
using CryptoExchange.Net.SharedApis;
using LBank.Net.Clients;

var shared = new LBankRestClient().SpotApi.SharedClient;
var symbol = new SharedSymbol(TradingMode.Spot, "ETH", "USDT");
var ticker = await shared.GetSpotTickerAsync(new GetTickerRequest(symbol));
```

The REST shared client supports Spot symbols, tickers, orders, balances, assets, book tickers, deposits, withdrawals, fees, klines, order books, and recent trades. The socket shared client supports balances, klines, trades, order books, tickers, and Spot orders. Inspect the corresponding `ILBank*Shared` interface or `Discover()` before relying on a specific shared operation.

## Common pitfalls

- Do not use Binance-style symbols such as `ETHUSDT` for native calls; use `eth_usdt`.
- Do not invent Futures or margin roots.
- Do not split order side and type; use LBank's combined `OrderType`.
- Do not pass `KlineInterval` to a socket method or `StreamKlineInterval` to a REST method.
- Do not omit required `limit` and `afterTime` arguments from `GetKlinesAsync`.
- Do not omit required pagination from `GetOrdersAsync` or `GetOpenOrdersAsync`.
- Do not read `.Data` until `.Success` is true.
- Do not create a client for every request.
- Do not block async calls with `.Result` or `.Wait()`.
- Do not forget to unsubscribe and stop local order books during shutdown.
- Do not assume sandbox/testnet exists; the built-in environment is `LBankEnvironment.Live`, with custom endpoints available through `CreateCustom`.
- Do not call the shared all-assets operation expecting it to work; LBank can retrieve asset configuration only for a specified asset.

## Reference

- Full context: `llms-full.txt`
- Intent map: `docs/ai-api-map.md`
- Compilable examples: `Examples/ai-friendly/`
- Source interfaces: `LBank.Net/Interfaces/Clients/**`
- Documentation: https://cryptoexchange.jkorf.dev/LBank.Net/
- Repository: https://github.com/JKorf/LBank.Net
- NuGet: https://www.nuget.org/packages/LBank.Net
