using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Testing;
using LBank.Net.Clients;
using LBank.Net.Enums;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LBank.Net.UnitTests
{
    [TestFixture]
    public class RestRequestTests
    {
        [Test]
        public async Task ValidateSpotAccountCalls()
        {
            var client = new LBankRestClient(opts =>
            {
                opts.AutoTimestamp = false;
                opts.ApiCredentials = new LBankCredentials("123", "456");
            });
            var tester = new RestRequestValidator<LBankRestClient>(client, "Endpoints/Spot/Account", "https://api.lbank.info", IsAuthenticated);
            await tester.ValidateAsync(client => client.SpotApi.Account.GetApiKeyInfoAsync(), "GetApiKeyInfo", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.SpotApi.Account.GetUserAssetsAsync(), "GetUserAssets", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.SpotApi.Account.WithdrawAsync("123", "123", 0.1m, 0.1m), "Withdraw", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.SpotApi.Account.GetDepositHistoryAsync(), "GetDepositHistory", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.SpotApi.Account.GetDepositAddressAsync("123"), "GetDepositAddress", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.SpotApi.Account.GetAssetDetailsAsync(), "GetAssetDetails", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.SpotApi.Account.GetAccountInfoAsync(), "GetAccountInfo", nestedJsonProperty: "data");
        }

        [Test]
        public async Task ValidateSpotExchangeDataCalls()
        {
            var client = new LBankRestClient(opts =>
            {
                opts.AutoTimestamp = false;
                opts.ApiCredentials = new LBankCredentials("123", "456");
            });
            var tester = new RestRequestValidator<LBankRestClient>(client, "Endpoints/Spot/ExchangeData", "https://api.lbank.info", IsAuthenticated);
            //await tester.ValidateAsync(client => client.SpotApi.ExchangeData.GetServerTimeAsync(), "GetServerTime");
            await tester.ValidateAsync(client => client.SpotApi.ExchangeData.GetSymbolsAsync(), "GetSymbols", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.SpotApi.ExchangeData.GetAssetsAsync("eth"), "GetAssets", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.SpotApi.ExchangeData.GetOrderBookAsync("eth_usdt"), "GetOrderBook", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.SpotApi.ExchangeData.GetPriceAsync(), "GetPrice", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.SpotApi.ExchangeData.GetBookTickerAsync("eth_usdt"), "GetBookTicker", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.SpotApi.ExchangeData.GetTickersAsync(), "GetTickers", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.SpotApi.ExchangeData.GetTradesAsync("eth_usdt"), "GetTrades", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.SpotApi.ExchangeData.GetKlinesAsync("eth_usdt", KlineInterval.OneMinute, 123, new DateTime(2026,1, 1)), "GetKlines", nestedJsonProperty: "data");

        }

        [Test]
        public async Task ValidateSpotTradingCalls()
        {
            var client = new LBankRestClient(opts =>
            {
                opts.AutoTimestamp = false;
                opts.ApiCredentials = new LBankCredentials("123", "456");
            });
            var tester = new RestRequestValidator<LBankRestClient>(client, "Endpoints/Spot/Trading", "https://api.lbank.info", IsAuthenticated);
            await tester.ValidateAsync(client => client.SpotApi.Trading.PlaceOrderAsync("eth_usdt", OrderType.BuyLimit, 0.1m), "PlaceOrder", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.SpotApi.Trading.CancelOrderAsync("eth_usdt"), "CancelOrder", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.SpotApi.Trading.GetOrdersAsync("eth_usdt", 1, 20), "GetOrders", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.SpotApi.Trading.GetUserTradesAsync("eth_usdt"), "GetUserTrades", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.SpotApi.Trading.CancelAllOrdersAsync("eth_usdt"), "CancelAllOrders", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.SpotApi.Trading.GetOrderAsync("eth_usdt"), "GetOrder", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.SpotApi.Trading.GetOpenOrdersAsync("eth_usdt", 1, 20), "GetOpenOrders", nestedJsonProperty: "data");
        }

        private bool IsAuthenticated(IHttpResult result)
        {
            return result.RequestBody?.Contains("sign=") == true;
        }
    }
}
