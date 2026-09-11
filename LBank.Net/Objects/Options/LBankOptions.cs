using CryptoExchange.Net.Objects.Options;
using CryptoExchange.Net.SharedApis;

namespace LBank.Net.Objects.Options
{
    /// <summary>
    /// LBank options
    /// </summary>
    public class LBankOptions : LibraryOptions<LBankRestOptions, LBankSocketOptions, LBankCredentials, LBankEnvironment>
    {
        /// <summary>
        /// Options for Shared API usage
        /// </summary>
        public SharedApiOptions SharedApi { get; set; } = new();
    }
}
