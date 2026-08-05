# LBank.Net AI API Quick Map

Route user intent to the implemented LBank.Net member shown here. The source interfaces under `LBank.Net/Interfaces/Clients/**` are authoritative if a signature is not listed.

## Client roots

| User intent | Use |
|---|---|
| Public or private REST | `new LBankRestClient()` |
| WebSocket streams | `new LBankSocketClient()` |
| Spot REST | `restClient.SpotApi` |
| Spot WebSocket | `socketClient.SpotApi` |
| HMAC authentication | `new LBankCredentials("API_KEY", "API_SECRET")` |
| RSA XML authentication | `new LBankCredentials().WithRSAXml(key, privateKey)` |
| RSA PEM authentication | `new LBankCredentials().WithRSAPem(key, privateKey)` on compatible targets |
| Dependency injection | `services.AddLBank(options => { ... })` |
| Live environment | `LBankEnvironment.Live` |
| Custom API addresses | `LBankEnvironment.CreateCustom(...)` |

There is no Futures, derivatives, or margin root. Native symbol examples use lowercase `base_quote`, such as `eth_usdt`.

## Spot REST market data

| User intent | LBank.Net member |
|---|---|
| Get server time | `client.SpotApi.ExchangeData.GetServerTimeAsync()` |
| Get available symbol names | `client.SpotApi.ExchangeData.GetAvailableSymbolsAsync()` |
| Get symbol rules/precision | `client.SpotApi.ExchangeData.GetSymbolsAsync("eth_usdt")` |
| Get asset and network configuration | `client.SpotApi.ExchangeData.GetAssetsAsync("eth")` |
| Get order book snapshot | `client.SpotApi.ExchangeData.GetOrderBookAsync("eth_usdt", limit: 20)` |
| Get last price | `client.SpotApi.ExchangeData.GetPriceAsync("eth_usdt")` |
| Get all last prices | `client.SpotApi.ExchangeData.GetPriceAsync()` |
| Get best bid and ask | `client.SpotApi.ExchangeData.GetBookTickerAsync("eth_usdt")` |
| Get 24-hour ticker | `client.SpotApi.ExchangeData.GetTickersAsync("eth_usdt")` |
| Get all 24-hour tickers | `client.SpotApi.ExchangeData.GetTickersAsync()` |
| Get leveraged-token tickers | `client.SpotApi.ExchangeData.GetLeveragedTokenTickersAsync("eth3l_usdt")` |
| Get recent public trades | `client.SpotApi.ExchangeData.GetTradesAsync("eth_usdt", limit: 100)` |
| Get REST candles | `client.SpotApi.ExchangeData.GetKlinesAsync("eth_usdt", KlineInterval.OneMinute, 100, afterTime)` |

`GetKlinesAsync` requires both `limit` and `afterTime`. The REST order-book maximum is 200 entries.

## Spot REST account and wallet

All methods in this section require credentials.

| User intent | LBank.Net member |
|---|---|
| Get API-key permissions | `client.SpotApi.Account.GetApiKeyInfoAsync()` |
| Get balances and asset networks | `client.SpotApi.Account.GetUserAssetsAsync()` |
| Get account info and balances | `client.SpotApi.Account.GetAccountInfoAsync()` |
| Withdraw | `client.SpotApi.Account.WithdrawAsync(address, asset, quantity, fee, ...)` |
| Get deposit history | `client.SpotApi.Account.GetDepositHistoryAsync(...)` |
| Get withdrawal history | `client.SpotApi.Account.GetWithdrawHistoryAsync(...)` |
| Get deposit address | `client.SpotApi.Account.GetDepositAddressAsync("eth", network)` |
| Get asset details | `client.SpotApi.Account.GetAssetDetailsAsync("ETH")` |
| Get trade fee | `client.SpotApi.Account.GetTradeFeeAsync("ETHUSDT")` |
| Create user-stream key | `client.SpotApi.Account.StartUserStreamAsync()` |
| Refresh user-stream key | `client.SpotApi.Account.KeepAliveUserStreamAsync(listenKey)` |
| Destroy user-stream key | `client.SpotApi.Account.StopUserStreamAsync(listenKey)` |

Withdrawals are state-changing operations. Preserve all user-provided network, memo/tag, fee, address, and quantity values and surface API errors.

## Spot REST trading

| User intent | LBank.Net member |
|---|---|
| Place order | `client.SpotApi.Trading.PlaceOrderAsync(symbol, orderType, quantity, price, ...)` |
| Get order by exchange ID | `client.SpotApi.Trading.GetOrderAsync(symbol, orderId: id)` |
| Get order by client ID | `client.SpotApi.Trading.GetOrderAsync(symbol, clientOrderId: id)` |
| Cancel by exchange ID | `client.SpotApi.Trading.CancelOrderAsync(symbol, orderId: id)` |
| Cancel by client ID | `client.SpotApi.Trading.CancelOrderAsync(symbol, clientOrderId: id)` |
| Cancel all symbol orders | `client.SpotApi.Trading.CancelAllOrdersAsync(symbol)` |
| Get order history | `client.SpotApi.Trading.GetOrdersAsync(symbol, page, pageSize, status)` |
| Get open orders | `client.SpotApi.Trading.GetOpenOrdersAsync(symbol, page, pageSize)` |
| Get user trades | `client.SpotApi.Trading.GetUserTradesAsync(symbol, ...)` |

Order values combine side and behavior:

| Desired order | `OrderType` |
|---|---|
| Limit buy/sell | `BuyLimit` / `SellLimit` |
| Market buy/sell | `BuyMarket` / `SellMarket` |
| Post-only buy/sell | `BuyMaker` / `SellMaker` |
| Immediate-or-cancel buy/sell | `BuyIoc` / `SellIoc` |
| Fill-or-kill buy/sell | `BuyFok` / `SellFok` |

For a market buy, `quantity` is in the quote asset. The upstream LBank order API may not be enabled for every account.

## Spot WebSocket

| User intent | LBank.Net member |
|---|---|
| Subscribe trades | `socketClient.SpotApi.SubscribeToTradeUpdatesAsync(symbol, handler)` |
| Subscribe candles | `socketClient.SpotApi.SubscribeToKlineUpdatesAsync(symbol, StreamKlineInterval.OneMinute, handler)` |
| Subscribe order book | `socketClient.SpotApi.SubscribeToOrderBookUpdatesAsync(symbol, depth, handler)` |
| Subscribe ticker | `socketClient.SpotApi.SubscribeToTickerUpdatesAsync(symbol, handler)` |
| Subscribe private orders | `socketClient.SpotApi.SubscribeToOrderUpdatesAsync(listenKey, handler)` |
| Subscribe private balances | `socketClient.SpotApi.SubscribeToBalanceUpdatesAsync(listenKey, handler)` |
| Unsubscribe | `await socketClient.UnsubscribeAsync(subscription.Data)` |

Order book `depth` must be `10`, `50`, or `100`. For a private stream, pass an existing listen key or pass `null` on an authenticated socket client so the library acquires and maintains one.

Public streams use the v3 WebSocket implementation by default through `LBankSocketOptions.UseV3 = true`. Set it to `false` to select v2 public streams.

## Local order book and trackers

| User intent | LBank.Net member |
|---|---|
| Resolve order-book factory | `provider.GetRequiredService<ILBankOrderBookFactory>()` |
| Create native Spot order book | `factory.CreateSpot("eth_usdt", options => options.Limit = 50)` |
| Create from shared symbol | `factory.Create(sharedSymbol, options => ...)` |
| Start local book | `await book.StartAsync()` |
| Read local snapshot | `book.Book` |
| Stop local book | `await book.StopAsync()` |
| Resolve tracker factory | `provider.GetRequiredService<ILBankTrackerFactory>()` |
| Create kline tracker | `trackerFactory.CreateKlineTracker(symbol, interval, ...)` |
| Create trade tracker | `trackerFactory.CreateTradeTracker(symbol, ...)` |
| Create configured user tracker | `trackerFactory.CreateUserSpotDataTracker(...)` |

## SharedApis

| User intent | Use |
|---|---|
| Shared Spot REST | `new LBankRestClient().SpotApi.SharedClient` |
| Shared Spot socket | `new LBankSocketClient().SpotApi.SharedClient` |
| Discover supported operations | `sharedClient.Discover()` |
| Shared symbol | `new SharedSymbol(TradingMode.Spot, "ETH", "USDT")` |
| Shared ticker request | `shared.GetSpotTickerAsync(new GetTickerRequest(symbol))` |
| Shared ticker subscription | `sharedSocket.SubscribeToTickerUpdatesAsync(new SubscribeTickerRequest(symbol), handler)` |

The REST shared client implements assets, balances, book ticker, deposits, fees, klines, order books, recent trades, withdrawals, Spot symbols/tickers/orders, and client-order-ID support. LBank does not provide a full all-assets query; retrieve a specified asset instead. The socket shared client implements balances, klines, trades, order books, tickers, and Spot orders.

## Result handling and routing pitfalls

| Avoid | Use |
|---|---|
| Reading `result.Data` immediately | Check `result.Success` first |
| `ETHUSDT` for native calls | `eth_usdt` |
| `FuturesApi` or `DerivativesApi` | `SpotApi` |
| Separate side and time-in-force parameters | Combined `OrderType` |
| `KlineInterval` on sockets | `StreamKlineInterval` |
| Omitting order-history pagination | Supply `page` and `pageSize` |
| Arbitrary socket depth | `10`, `50`, or `100` |
| Raw HTTP/socket code | `LBankRestClient` / `LBankSocketClient` |
| Base `ApiCredentials` | `LBankCredentials` |
| Assuming testnet | `LBankEnvironment.Live` or an explicit custom environment |
