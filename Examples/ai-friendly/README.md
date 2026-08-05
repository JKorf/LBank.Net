# LBank.Net AI-Friendly Examples

These examples are small, self-contained console programs for AI assistants and quick onboarding. They are compiled by `LBank.Net.UnitTests/AiExampleCompileTests.cs`.

## Files

| File | Demonstrates |
| --- | --- |
| `01-spot-quickstart.cs` | Public Spot market data, authenticated balances, symbols, klines, and order books |
| `02-spot-trading.cs` | Spot order placement, lookup, pagination, and cancellation |
| `03-websocket.cs` | Public and authenticated Spot subscriptions with teardown |
| `04-di-order-book.cs` | Dependency injection and the synchronized local Spot order book |
| `05-error-handling.cs` | Reusable `HttpResult<T>` and `WebSocketResult<T>` handling with transient retry |

## LBank Shape To Remember

LBank.Net currently exposes a Spot surface:

```csharp
restClient.SpotApi.ExchangeData
restClient.SpotApi.Account
restClient.SpotApi.Trading
socketClient.SpotApi
```

There is no `FuturesApi`, `DerivativesApi`, or separate unified trading root in LBank.Net.

Native LBank symbols use lowercase `base_quote` formatting:

```csharp
const string symbol = "eth_usdt";
```

REST candles use `KlineInterval`; WebSocket candles use `StreamKlineInterval`. Order side and execution behavior are combined in `OrderType`, for example `OrderType.BuyLimit` and `OrderType.SellMarket`.

Credentials support HMAC or RSA. The common HMAC form is:

```csharp
new LBankCredentials("API_KEY", "API_SECRET")
```

## Running

```bash
dotnet new console -n MyLBankApp
cd MyLBankApp
dotnet add package LBank.Net
# Copy one example into Program.cs
dotnet run
```
