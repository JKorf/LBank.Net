using CryptoExchange.Net.Interfaces.Clients;
using LBank.Net.Interfaces.Clients.SpotApi;

namespace LBank.Net.Interfaces.Clients
{
    /// <summary>
    /// Client for accessing the LBank websocket API
    /// </summary>
    public interface ILBankSocketClient : ISocketClient<LBankCredentials>
    {
        /// <summary>
        /// Spot API endpoints
        /// </summary>
        /// <see cref="ILBankSocketClientSpotApi"/>
        public ILBankSocketClientSpotApi SpotApi { get; }
    }
}
