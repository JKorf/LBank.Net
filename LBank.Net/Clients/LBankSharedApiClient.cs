using LBank.Net.Interfaces.Clients;
using LBank.Net.Interfaces.Clients.SpotApi;

namespace LBank.Net.Clients
{
    /// <inheritdoc />
    public class LBankSharedApiClient : ILBankSharedApiClient
    {
        /// <inheritdoc />
        public ILBankRestClientSpotSharedApi Rest { get; }
        /// <inheritdoc />
        public ILBankSocketClientSpotSharedApi Socket { get; }

        /// <summary>
        /// ctor
        /// </summary>
        public LBankSharedApiClient(
            ILBankRestClient restClient,
            ILBankSocketClient socketClient)
        {
            Rest = restClient.SpotApi.SharedApi;
            Socket = socketClient.SpotApi.SharedApi;
        }
    }
}
