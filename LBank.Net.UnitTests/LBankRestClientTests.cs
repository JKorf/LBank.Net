using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Interfaces.Clients;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Testing;
using LBank.Net.Clients;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net.Http;

namespace LBank.Net.UnitTests
{
    [TestFixture()]
    public class LBankRestClientTests
    {
        [Test]
        public void CheckInterfaces()
        {
            CryptoExchange.Net.Testing.TestHelpers.CheckForMissingRestInterfaces<LBankRestClient>();
            CryptoExchange.Net.Testing.TestHelpers.CheckForMissingSocketInterfaces<LBankSocketClient>();
        }

        [Test]
        public void TestSpotRestSharedApiDiscoveryMatchesAggregate()
        {
            var (missingOptions, missingInterfaces) = CryptoExchange.Net.Testing.TestHelpers.ValidateSharedApi(new LBankRestClient().SpotApi.SharedApi);

            Assert.That(missingOptions, Is.Empty);
            Assert.That(missingInterfaces, Is.Empty);
        }

        [Test]
        public void TestSpotSocketSharedApiDiscoveryMatchesAggregate()
        {
            var (missingOptions, missingInterfaces) = CryptoExchange.Net.Testing.TestHelpers.ValidateSharedApi(new LBankSocketClient().SpotApi.SharedApi);

            Assert.That(missingOptions, Is.Empty);
            Assert.That(missingInterfaces, Is.Empty);
        }

        [Test]
        public void TestSpotRestSharedApiDoesntHaveUnsupportedCapabilities()
        {
            var unsupported = TestHelpers.ValidateUnsupportedCapabilities(new LBankRestClient().SpotApi.SharedApi);

            Assert.That(unsupported, Is.Empty);
        }

        [Test]
        public void TestSpotSocketSharedApiDoesntHaveUnsupportedCapabilities()
        {
            var unsupported = TestHelpers.ValidateUnsupportedCapabilities(new LBankSocketClient().SpotApi.SharedApi);

            Assert.That(unsupported, Is.Empty);
        }

    }
}
