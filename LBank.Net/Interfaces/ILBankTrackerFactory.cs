using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.SharedApis;
using CryptoExchange.Net.Trackers.UserData.Interfaces;
using CryptoExchange.Net.Trackers.UserData.Objects;

namespace LBank.Net.Interfaces
{
    /// <summary>
    /// Tracker factory
    /// </summary>
    public interface ILBankTrackerFactory : ITrackerFactory
    {
        /// <summary>
        /// Create a new Spot user data tracker
        /// </summary>
        /// <param name="userIdentifier">User identifier</param>
        /// <param name="config">Configuration</param>
        /// <param name="credentials">Credentials</param>
        /// <param name="environment">Environment</param>
        /// <param name="exchangeParameters">Exchange specific parameters</param>
        IUserSpotDataTracker CreateUserSpotDataTracker(string userIdentifier, LBankCredentials credentials, SpotUserDataTrackerConfig? config = null, LBankEnvironment? environment = null, ExchangeParameters? exchangeParameters = null);
        /// <summary>
        /// Create a new spot user data tracker
        /// </summary>
        /// <param name="config">Configuration</param>
        /// <param name="exchangeParameters">Exchange specific parameters</param>
        IUserSpotDataTracker CreateUserSpotDataTracker(SpotUserDataTrackerConfig? config = null, ExchangeParameters? exchangeParameters = null);
    }
}
