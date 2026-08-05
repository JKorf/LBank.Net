using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Objects;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net.Http;
using LBank.Net.Clients;

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
    }
}
