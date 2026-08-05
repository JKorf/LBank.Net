using CryptoExchange.Net.Objects.Options;

namespace LBank.Net.Objects.Options
{
    /// <summary>
    /// Options for the LBankRestClient
    /// </summary>
    public class LBankRestOptions : RestExchangeOptions<LBankEnvironment, LBankCredentials>
    {
        /// <summary>
        /// Default options for new clients
        /// </summary>
        internal static LBankRestOptions Default { get; set; } = new LBankRestOptions()
        {
            Environment = LBankEnvironment.Live,
            AutoTimestamp = true
        };

        /// <summary>
        /// ctor
        /// </summary>
        public LBankRestOptions()
        {
            Default?.Set(this);
        }

        /// <summary>
        /// Futures API options
        /// </summary>
        public RestApiOptions FuturesOptions { get; private set; } = new RestApiOptions();

        /// <summary>
        /// Spot API options
        /// </summary>
        public RestApiOptions SpotOptions { get; private set; } = new RestApiOptions();

        internal LBankRestOptions Set(LBankRestOptions targetOptions)
        {
            targetOptions = base.Set<LBankRestOptions>(targetOptions);
            targetOptions.FuturesOptions = FuturesOptions.Set(targetOptions.FuturesOptions);
            targetOptions.SpotOptions = SpotOptions.Set(targetOptions.SpotOptions);
            return targetOptions;
        }
    }
}
