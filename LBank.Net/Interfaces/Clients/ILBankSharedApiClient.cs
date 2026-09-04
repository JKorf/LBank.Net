using LBank.Net.Interfaces.Clients.SpotApi;

namespace LBank.Net.Interfaces.Clients
{
    /// <summary>
    /// Client for the shared REST and WebSocket API implementations of LBank
    /// </summary>
    public interface ILBankSharedApiClient
    {
        /// <summary>
        /// REST shared API implementations
        /// </summary>
        ILBankRestClientSpotSharedApi Rest { get; }

        /// <summary>
        /// WebSocket shared API implementations
        /// </summary>
        ILBankSocketClientSpotSharedApi Socket { get; }
    }
}
