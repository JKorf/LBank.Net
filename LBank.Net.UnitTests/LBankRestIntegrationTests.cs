using CryptoExchange.Net.Testing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LBank.Net.Clients;
using LBank.Net.Objects.Options;
using CryptoExchange.Net.Objects.Errors;

namespace LBank.Net.UnitTests
{
    [NonParallelizable]
    public class LBankRestIntegrationTests : RestIntegrationTest<LBankRestClient>
    {
        public override bool Run { get; set; } = true;

        public override LBankRestClient GetClient(ILoggerFactory loggerFactory)
        {
            var key = Environment.GetEnvironmentVariable("APIKEY");
            var sec = Environment.GetEnvironmentVariable("APISECRET");

            Authenticated = key != null && sec != null;
            return new LBankRestClient(null, loggerFactory, Options.Create(new LBankRestOptions
            {
                AutoTimestamp = false,
                OutputOriginalData = true,
                ApiCredentials = Authenticated ? new LBankCredentials(key, sec) : null
            }));
        }

        [Test]
        public async Task TestErrorResponseParsing()
        {
            if (!ShouldRun())
                return;

            var result = await CreateClient().SpotApi.ExchangeData.GetTickersAsync("TSTTST", default);

            Assert.That(result.Success, Is.False);
            Assert.That(result.Error.Code, Is.EqualTo(10008));
            Assert.That(result.Error.ErrorType, Is.EqualTo(ErrorType.UnavailableSymbol));
        }

        [Test]
        public async Task TestSpotAccount()
        {
            var warnings = new List<Exception>();
            await RunAndCheckResult(warnings, client => client.SpotApi.Account.GetApiKeyInfoAsync(default), true, "data");
            await RunAndCheckResult(warnings, client => client.SpotApi.Account.GetUserAssetsAsync(default), true, "data");
            await RunAndCheckResult(warnings, client => client.SpotApi.Account.GetDepositHistoryAsync(default, default, default, default, default), true, "data");
            await RunAndCheckResult(warnings, client => client.SpotApi.Account.GetWithdrawHistoryAsync(default, default, default, default, default, default), true, "data");
            await RunAndCheckResult(warnings, client => client.SpotApi.Account.GetAssetDetailsAsync(default, default), true, "data");
            await RunAndCheckResult(warnings, client => client.SpotApi.Account.GetTradeFeeAsync(default, default), true, "data");
            await RunAndCheckResult(warnings, client => client.SpotApi.Account.GetAccountInfoAsync(default), true, "data");
            foreach (var warning in warnings)
                Assert.Warn(warning.Message);
        }

        [Test]
        public async Task TestSpotExchangeData()
        {
            var warnings = new List<Exception>();
            await RunAndCheckResult(warnings, client => client.SpotApi.ExchangeData.GetServerTimeAsync(default), false, "data");
            await RunAndCheckResult(warnings, client => client.SpotApi.ExchangeData.GetAvailableSymbolsAsync(default), false, "data");
            await RunAndCheckResult(warnings, client => client.SpotApi.ExchangeData.GetSymbolsAsync(default, default), false, "data");
            await RunAndCheckResult(warnings, client => client.SpotApi.ExchangeData.GetAssetsAsync("usdt", default), false, "data");
            await RunAndCheckResult(warnings, client => client.SpotApi.ExchangeData.GetOrderBookAsync("eth_usdt", default, default), false, "data");
            await RunAndCheckResult(warnings, client => client.SpotApi.ExchangeData.GetPriceAsync(default, default), false, "data");
            await RunAndCheckResult(warnings, client => client.SpotApi.ExchangeData.GetBookTickerAsync("eth_usdt", default), false, "data");
            await RunAndCheckResult(warnings, client => client.SpotApi.ExchangeData.GetTickersAsync(default, default), false, "data");
            await RunAndCheckResult(warnings, client => client.SpotApi.ExchangeData.GetLeveragedTokenTickersAsync(default, default), false, "data");
            await RunAndCheckResult(warnings, client => client.SpotApi.ExchangeData.GetTradesAsync("eth_usdt", default, default, default), false, "data");
            await RunAndCheckResult(warnings, client => client.SpotApi.ExchangeData.GetKlinesAsync("eth_usdt", Enums.KlineInterval.OneHour, 100, DateTime.UtcNow.AddDays(-1), default), false, "data");
            foreach (var warning in warnings)
                Assert.Warn(warning.Message);
        }

        [Test]
        public async Task TestSpotTrading()
        {
            var warnings = new List<Exception>();
            // trading not currently publicly accessible
            foreach (var warning in warnings)
                Assert.Warn(warning.Message);
        }
    }
}
