using CryptoExchange.Net.Objects;
using LBank.Net.Objects;

namespace LBank.Net
{
    /// <summary>
    /// LBank environments
    /// </summary>
    public class LBankEnvironment : TradeEnvironment
    {
        /// <summary>
        /// Rest Spot API address
        /// </summary>
        public string RestClientSpotAddress { get; }

        /// <summary>
        /// Socket API address
        /// </summary>
        public string SocketClientAddress { get; }

        /// <summary>
        /// Socket API address
        /// </summary>
        public string SocketClientV3Address { get; }

        internal LBankEnvironment(
            string name,
            string restSpotAddress,
            string streamAddress,
            string streamV3Address) :
            base(name)
        {
            RestClientSpotAddress = restSpotAddress;
            SocketClientAddress = streamAddress;
            SocketClientV3Address = streamV3Address;
        }

        /// <summary>
        /// ctor for DI, use <see cref="CreateCustom"/> for creating a custom environment
        /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public LBankEnvironment() : base(TradeEnvironmentNames.Live)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        { }

        /// <summary>
        /// Get the LBank environment by name
        /// </summary>
        public static LBankEnvironment? GetEnvironmentByName(string? name)
         => name switch
         {
             TradeEnvironmentNames.Live => Live,
             "" => Live,
             null => Live,
             _ => default
         };

        /// <summary>
        /// Available environment names
        /// </summary>
        /// <returns></returns>
        public static string[] All => [Live.Name];

        /// <summary>
        /// Live environment
        /// </summary>
        public static LBankEnvironment Live { get; }
            = new LBankEnvironment(TradeEnvironmentNames.Live,
                                     LBankApiAddresses.Default.RestClientSpotAddress,
                                     LBankApiAddresses.Default.SocketClientSpotAddress,
                                     LBankApiAddresses.Default.SocketClientV3SpotAddress);

        /// <summary>
        /// Create a custom environment
        /// </summary>
        public static LBankEnvironment CreateCustom(
                        string name,
                        string spotRestAddress,
                        string spotSocketStreamsAddress,
                        string spotSocketStreamsV3Address)
            => new LBankEnvironment(name, spotRestAddress, spotSocketStreamsAddress, spotSocketStreamsV3Address);
    }
}
