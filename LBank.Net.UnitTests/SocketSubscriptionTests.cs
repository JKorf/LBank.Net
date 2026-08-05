using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Testing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System.Threading.Tasks;
using LBank.Net.Clients;
using LBank.Net.Objects.Models;
using LBank.Net.Objects.Options;

namespace LBank.Net.UnitTests
{
    [TestFixture]
    public class SocketSubscriptionTests
    {
        [Test]
        public async Task ValidateSubscriptions()
        {
            var logger = new LoggerFactory();
            logger.AddProvider(new TraceLoggerProvider());

            var client = new LBankSocketClient(Options.Create(new LBankSocketOptions
            {
                ApiCredentials = new LBankCredentials("123", "456"),
                OutputOriginalData = true
            }), logger);

            var tester = new SocketSubscriptionValidator<LBankSocketClient>(client, "Subscriptions/Spot", "wss://www.lbank.com/old-wss/ccws/ws/V3/");
            await tester.ValidateAsync<LBankKlineUpdate>((client, handler) => client.SpotApi.SubscribeToKlineUpdatesAsync("eth_usdt", Enums.StreamKlineInterval.OneDay, handler), "Klines", nestedJsonProperty: "kbar");
            await tester.ValidateAsync<LBankOrderBookUpdate>((client, handler) => client.SpotApi.SubscribeToOrderBookUpdatesAsync("eth_usdt", 100, handler), "OrderBook", nestedJsonProperty: "depth");
            await tester.ValidateAsync<LBankTradeUpdate>((client, handler) => client.SpotApi.SubscribeToTradeUpdatesAsync("eth_usdt", handler), "Trades", nestedJsonProperty: "trade");
            await tester.ValidateAsync<LBankTickerUpdate>((client, handler) => client.SpotApi.SubscribeToTickerUpdatesAsync("eth_usdt", handler), "Ticker", nestedJsonProperty: "tick");
        }

        [TestCase]
        public async Task ValidateConcurrentSpotSubscriptions()
        {
            var logger = new LoggerFactory();
            logger.AddProvider(new TraceLoggerProvider());

            var client = new LBankSocketClient(Options.Create(new LBankSocketOptions
            {
                ApiCredentials = new LBankCredentials("123", "456"),
                OutputOriginalData = true
            }), logger);

            var tester = new SocketSubscriptionValidator<LBankSocketClient>(client, "Subscriptions/Spot", "wss://www.lbank.com/old-wss/ccws/ws/V3/", "kbar");
            await tester.ValidateConcurrentAsync<LBankKlineUpdate>(
                (client, handler) => client.SpotApi.SubscribeToKlineUpdatesAsync("eth_usdt", Enums.StreamKlineInterval.OneDay, handler),
                (client, handler) => client.SpotApi.SubscribeToKlineUpdatesAsync("eth_usdt", Enums.StreamKlineInterval.OneHour, handler),
                "Concurrent");
        }
    }
}
