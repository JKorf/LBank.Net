using CryptoExchange.Net.Testing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using LBank.Net.Clients;
using LBank.Net.Objects.Options;
using LBank.Net.Objects.Models;

namespace LBank.Net.UnitTests
{
    [NonParallelizable]
    internal class LBankSocketIntegrationTests : SocketIntegrationTest<LBankSocketClient>
    {
        public override bool Run { get; set; } = true;

        public LBankSocketIntegrationTests()
        {
        }

        public override LBankSocketClient GetClient(ILoggerFactory loggerFactory)
        {
            var key = Environment.GetEnvironmentVariable("APIKEY");
            var sec = Environment.GetEnvironmentVariable("APISECRET");

            Authenticated = key != null && sec != null;
            return new LBankSocketClient(Options.Create(new LBankSocketOptions
            {
                OutputOriginalData = true,
                ApiCredentials = Authenticated ? new LBankCredentials(key, sec) : null
            }), loggerFactory);
        }

        [TestCase]
        public async Task TestSubscriptions()
        {
            await RunAndCheckUpdate<LBankTickerUpdate>((client, updateHandler) => client.SpotApi.SubscribeToTickerUpdatesAsync("eth_usdt", updateHandler, default), true, false);
        } 
    }
}
