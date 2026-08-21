# Copilot Instructions for LBank.Net

This repository is **LBank.Net**, a strongly typed C#/.NET client for the LBank Spot REST and WebSocket APIs, built on CryptoExchange.Net.

## Use LBank.Net clients

Use `LBankRestClient` and `LBankSocketClient`; do not generate raw `HttpClient` or `ClientWebSocket` integrations for implemented operations. LBank.Net supports Spot only.

```csharp
using LBank.Net.Clients;

var client = new LBankRestClient();
var result = await client.SpotApi.ExchangeData.GetTickersAsync("eth_usdt");
if (!result.Success) { /* result.Error */ return; }
var lastPrice = result.Data.SingleOrDefault()?.Ticker.LastPrice;
```

## Authentication and results

Configure private clients with `LBankCredentials("API_KEY", "API_SECRET")`, not generic `ApiCredentials`. RSA is supported through `WithRSAXml(...)` and, on compatible targets, `WithRSAPem(...)`. Never embed real keys.

REST methods return `HttpResult<T>` or `HttpResult`; socket subscriptions return `WebSocketResult<UpdateSubscription>`. Always check `.Success` before reading `.Data`.

## API structure

- `restClient.SpotApi.ExchangeData` — public symbols, assets, prices, tickers, books, trades, and klines
- `restClient.SpotApi.Account` — balances, deposits, withdrawals, fees, and user-stream keys
- `restClient.SpotApi.Trading` — Spot order placement, queries, cancellation, and user trades
- `socketClient.SpotApi` — public market and private order/balance streams

Do not invent Futures, derivatives, or margin roots. Native symbols use lowercase `base_quote`, for example `eth_usdt`.

## Orders and streams

LBank's `OrderType` combines side and behavior. Use `OrderType.BuyLimit`, `SellMarket`, `BuyMaker`, `SellIoc`, and similar values; do not add separate side or time-in-force parameters to `PlaceOrderAsync`. Market-buy `quantity` is quote-asset quantity. Order history and open-order calls require pagination.

REST candles use `KlineInterval`; WebSocket candles use `StreamKlineInterval`. Order-book stream depth must be `10`, `50`, or `100`.

Store successful subscriptions and unsubscribe on shutdown:

```csharp
var sub = await socketClient.SpotApi.SubscribeToTickerUpdatesAsync(
    "eth_usdt", update => Console.WriteLine(update.Data.LastPrice));
if (!sub.Success) { /* sub.Error */ return; }
await socketClient.UnsubscribeAsync(sub.Data);
```

Private order and balance subscriptions accept a listen key. Passing `null` lets an authenticated socket client acquire and maintain one. Public streams use WebSocket v3 by default.

## Shared APIs

Use `new LBankRestClient().SpotApi.SharedClient` or the socket equivalent for exchange-agnostic `CryptoExchange.Net.SharedApis` code. Call `Discover()` or inspect operation options before assuming support.

Shared book-ticker and user-trade quantities use `SharedOrderQuantity`; read `QuantityInBaseAsset` or `QuantityInQuoteAsset`. Shared REST and socket order books identify entry quantities as `SharedQuantityType.BaseAsset`. The shared all-assets operation is not supported; request configuration for a specified asset.

## Avoid

- Raw HTTP or socket implementations
- `FuturesApi`, `DerivativesApi`, or margin methods
- Binance-style native symbols such as `ETHUSDT`
- Generic `ApiCredentials`
- Reading `.Data` before checking `.Success`
- Synchronous `.Result` or `.Wait()`
- Creating a client per request
- Leaving subscriptions or local order books running at shutdown
- Assuming an official sandbox/testnet exists

For signatures and fuller examples, see `AGENTS.md`, `llms-full.txt`, `docs/ai-api-map.md`, `Examples/ai-friendly/`, and `LBank.Net/Interfaces/Clients/**`.
