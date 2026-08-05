using CryptoExchange.Net.SharedApis;
using CryptoExchange.Net.Trackers.UserData;
using CryptoExchange.Net.Trackers.UserData.Objects;
using Microsoft.Extensions.Logging;
using LBank.Net.Interfaces.Clients;

namespace LBank.Net
{
    /// <inheritdoc />
    public class LBankUserSpotDataTracker : UserSpotDataTracker
    {
        /// <summary>
        /// ctor
        /// </summary>
        public LBankUserSpotDataTracker(
            ILogger<LBankUserSpotDataTracker> logger,
            ILBankRestClient restClient,
            ILBankSocketClient socketClient,
            string? userIdentifier,
            SpotUserDataTrackerConfig? config = null,
            ExchangeParameters? exchangeParameters = null) : base(
                logger,
                restClient.SpotApi.SharedClient,
                restClient.SpotApi.SharedClient,
                socketClient.SpotApi.SharedClient,
                restClient.SpotApi.SharedClient,
                socketClient.SpotApi.SharedClient,
                null,
                userIdentifier,
                config ?? new SpotUserDataTrackerConfig(),
                exchangeParameters)
        {

        }
    }
}
