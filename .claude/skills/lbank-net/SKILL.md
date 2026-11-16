---
name: lbank-net
description: Generate and troubleshoot C#/.NET code consuming LBank.Net for Spot market data, accounts, wallet operations, orders, WebSocket streams, local order books, trackers, and Shared API V2 integrations.
---

# LBank.Net

Install the consumer package:

```sh
dotnet add package LBank.Net
```

Use `LBankRestClient.SpotApi.{ExchangeData|Account|Trading}` and `LBankSocketClient.SpotApi`. The library exposes Spot; do not invent futures or margin roots. Reuse and dispose clients, await calls, and retain successful subscriptions for unsubscribe cleanup.

HMAC credentials are `LBankCredentials(key, secret)`; no passphrase is required. RSA XML and compatible-target PEM credentials are also supported. Public market data needs no credentials. Private streams accept an existing listen key, or null with credentials so the library acquires and maintains one.

Native symbols normally use lowercase `base_quote`, e.g. `eth_usdt`; follow method-specific exceptions. `OrderType` combines side and execution behavior (`BuyLimit`, `SellMarket`, maker/IOC/FOK variants), so native placement has no separate side or time-in-force arguments. Market-buy quantity is in quote assets. REST candles use `KlineInterval`; sockets use `StreamKlineInterval`.

Check `Success` before `Data`: REST returns `HttpResult`, subscriptions return `WebSocketResult<UpdateSubscription>`, and some Shared helpers return `ExchangeCallResult`, with generic variants where applicable. Native filtered ticker/price calls still return arrays; ticker prices are under `Ticker.LastPrice`.

Use `.SharedApi` with narrow V2 capabilities for portable operations. `ILBankSharedApiClient` groups `SpotRest` and `SpotSocket`. Use native APIs for listen keys and exchange-specific wallet parameters. Request a specified asset; there is no Shared all-assets capability.

## Read when needed

- [API surfaces](references/api-surfaces.md): endpoints, factories, stream prerequisites, Shared capabilities, catalogs, and quantities.
- [Usage](references/usage.md): setup, credentials, account reads, live order syntax, subscriptions, Shared API V2, and DI.
- [Operational considerations](references/safety.md): authentication variants, execution, withdrawals, listen keys, and uncertain submissions.

For work on the library itself, use [repository instructions](../../../AGENTS.md) (checkout-only).
