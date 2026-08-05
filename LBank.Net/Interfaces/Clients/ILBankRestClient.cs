using CryptoExchange.Net.Interfaces.Clients;
using LBank.Net.Interfaces.Clients.SpotApi;

namespace LBank.Net.Interfaces.Clients
{
    /// <summary>
    /// Client for accessing the LBank Rest API. 
    /// </summary>
    public interface ILBankRestClient : IRestClient<LBankCredentials>
    {
        /// <summary>
        /// Spot API endpoints
        /// </summary>
        /// <see cref="ILBankRestClientSpotApi"/>
        public ILBankRestClientSpotApi SpotApi { get; }
    }
}
