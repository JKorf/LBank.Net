using CryptoExchange.Net.Objects.Options;

namespace LBank.Net.Objects.Options
{
    /// <summary>
    /// Options for the LBankSocketClient
    /// </summary>
    public class LBankSocketOptions : SocketExchangeOptions<LBankEnvironment, LBankCredentials>
    {
        /// <summary>
        /// Default options for new clients
        /// </summary>
        internal static LBankSocketOptions Default { get; set; } = new LBankSocketOptions()
        {
            Environment = LBankEnvironment.Live,
            SocketSubscriptionsCombineTarget = 10
        };

        /// <summary>
        /// ctor
        /// </summary>
        public LBankSocketOptions()
        {
            Default?.Set(this);
        }

        /// <summary>
        /// Whether to use the v3 websocket API. If false, the v2 websocket API will be used. V3 is more stable but not officially documented. Currently only affects public streams. Defaults to true.
        /// </summary>
        public bool UseV3 { get; set; } = true;

        /// <summary>
        /// Futures API options
        /// </summary>
        public SocketApiOptions FuturesOptions { get; private set; } = new SocketApiOptions();

         /// <summary>
        /// Spot API options
        /// </summary>
        public SocketApiOptions SpotOptions { get; private set; } = new SocketApiOptions();

        internal LBankSocketOptions Set(LBankSocketOptions targetOptions)
        {
            targetOptions = base.Set<LBankSocketOptions>(targetOptions);
            targetOptions.UseV3 = UseV3;
            targetOptions.FuturesOptions = FuturesOptions.Set(targetOptions.FuturesOptions);
            targetOptions.SpotOptions = SpotOptions.Set(targetOptions.SpotOptions);
            return targetOptions;
        }
    }
}
