using CryptoExchange.Net.SharedApis;
using LBank.Net.Interfaces.Clients.SpotApi;

namespace LBank.Net.Interfaces.Clients
{
    /// <summary>
    /// Client for the shared REST and WebSocket API implementations of LBank
    /// </summary>
    public interface ILBankSharedApiClient : ISharedApiClientBase
    {
        /// <summary>
        /// REST shared API implementations
        /// </summary>
        ILBankRestClientSpotSharedApi SpotRest { get; }

        /// <summary>
        /// WebSocket shared API implementations
        /// </summary>
        ILBankSocketClientSpotSharedApi SpotSocket { get; }
    }
}
