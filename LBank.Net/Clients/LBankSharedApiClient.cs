using CryptoExchange.Net.SharedApis;
using LBank.Net.Interfaces.Clients;
using LBank.Net.Interfaces.Clients.SpotApi;
using LBank.Net.Objects.Options;
using Microsoft.Extensions.Options;

namespace LBank.Net.Clients
{
    /// <inheritdoc />
    public class LBankSharedApiClient : SharedApiClientBase, ILBankSharedApiClient
    {
        /// <inheritdoc />
        public ILBankRestClientSpotSharedApi SpotRest { get; }
        /// <inheritdoc />
        public ILBankSocketClientSpotSharedApi SpotSocket { get; }

        /// <summary>
        /// ctor
        /// </summary>
        public LBankSharedApiClient(
            ILBankRestClient restClient,
            ILBankSocketClient socketClient,
            IOptions<LBankOptions> options)
            : base(options.Value.SharedApi.PreferredTransport,
                  restClient.SpotApi.SharedApi,
                  socketClient.SpotApi.SharedApi)
        {
            SpotRest = restClient.SpotApi.SharedApi;
            SpotSocket = socketClient.SpotApi.SharedApi;
        }
    }
}
