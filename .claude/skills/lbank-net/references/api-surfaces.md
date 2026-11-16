# LBank.Net API surfaces

## Client roots

`LBankRestClient` / `ILBankRestClient` expose `SpotApi` with `ExchangeData`, `Account`, and `Trading`. `LBankSocketClient` / `ILBankSocketClient` expose subscriptions directly on `SpotApi`. The library has no Futures, derivatives, margin, or Unified root.

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

## Request limits and workflow details

REST order-book depth is at most 200, recent public trades at most 500, and candles at most 2000. REST candles require both `limit` and `afterTime`; user trades have a maximum limit of 100. Order history/open-order pagination requires `page` and `pageSize` (maximum 200). Use exactly one of exchange order ID and client order ID for lookup or cancellation.

Private stream listen keys can be supplied explicitly or managed by a credentialed socket client. Native `GetPriceAsync`, `GetTickersAsync`, and leveraged-token ticker calls return arrays. Native ticker prices are under `LBankSymbolTicker.Ticker.LastPrice`. For per-user cached clients use `ILBankUserClientProvider`; clear cached clients when credentials change. Trackers and local books should be owned for the application service lifetime.

## Shared API V2

`ILBankSharedApiClient` exposes these aggregate properties. The corresponding native surfaces expose `.SharedApi`.

- `SpotRest` (`ILBankRestClientSpotSharedApi`): `IGetAssetRest`, `IGetBalancesRest`, `IGetBookTickerRest`, `IGetDepositAddressesRest`, `IGetDepositHistoryRest`, `IGetFeesRest`, `IGetKlinesRest`, `IGetOrderBookRest`, `IGetRecentTradesRest`, `IGetWithdrawalHistoryRest`, `IWithdrawRest`, `IGetSpotSymbolsRest`, `IGetTickerRest`, `IGetAllTickersRest`, `IPlaceSpotOrderRest`, `IGetSpotOrderRest`, `IGetOpenSpotOrdersRest`, `IGetClosedSpotOrdersRest`, `IGetSpotOrderTradesRest`, `IGetSpotUserTradeHistoryRest`, `ICancelSpotOrderRest`, `IGetSpotOrderByClientOrderIdRest`, `ICancelSpotOrderByClientOrderIdRest`.
- `SpotSocket` (`ILBankSocketClientSpotSharedApi`): `ISubscribeBalancesSocket`, `ISubscribeKlinesSocket`, `ISubscribeTradesSocket`, `ISubscribeOrderBookSocket`, `ISubscribeTickerSocket`, `ISubscribeSpotOrdersSocket`.

For known workflows use an aggregate property or inject the narrow capability. For dynamic selection, call `aggregate.GetCapability(SharedCapabilities.Tickers.GetTicker.Rest, tradingMode: TradingMode.Spot)` and check for null before using the match's `Capability` and `Options`. A descriptor identifies an operation; it does not guarantee support. `GetCapabilities` enumerates matches and `Discover` summarizes metadata. With several exchanges registered, inject `IEnumerable<TCapability>` and select by exchange, supported mode, and options.

There is no `IGetAllAssetsRest` capability; use `IGetAssetRest` for a specified asset. Socket capabilities are subscriptions; they do not provide place/cancel order requests.

## Symbols, catalogs, and quantities

Native symbols normally use lowercase `base_quote`, e.g. `eth_usdt`. Some method documentation uses compact uppercase inputs, such as asset details/trade fees; check that method's signature and XML documentation. `SharedSymbol(TradingMode.Spot, "ETH", "USDT")` and `LBankExchange.FormatSymbol` support reusable formatting.

Call `IGetSpotSymbolsRest.GetSpotSymbolsAsync(new GetSymbolsRequest())`, check success, then read `SpotSymbolCatalog`. Discovery merges symbol rules and available symbols. Requests honor base/quote asset type/subtype filters. Returned metadata includes display names, precision, quantity steps, minimum quantity, and availability; classification covers crypto, stablecoins, fiat, and TradFi equity/commodity where populated.

Shared order books use `SharedQuantityType.BaseAsset`. Shared results use `SharedOrderQuantity`; select base/quote fields according to the returned operation and quantity support. Native market buys use quote-asset quantity, while other orders normally use base quantity. Inspect `PlaceSpotOrderOptions` and supported quantity metadata before selecting Shared sizing; do not translate a quote-sized market buy as base quantity.

## Environments and checkout-only sources

Only `LBankEnvironment.Live` is built in. `LBankEnvironment.CreateCustom(...)` configures explicit addresses; it does not imply an official testnet. Public sockets default to v3 through `LBankSocketOptions.UseV3 = true`; false selects v2.

- [Public interfaces](../../../../LBank.Net/Interfaces/Clients/): exact signatures and method-specific symbol requirements (checkout-only).
- [Client implementations](../../../../LBank.Net/Clients/): Shared options, conversion and classification (checkout-only).
- [Complete examples](../../../../Examples/ai-friendly/): independently compilable programs (checkout-only).
