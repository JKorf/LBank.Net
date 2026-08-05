using Microsoft.Extensions.Logging;
using System.Net.Http;
using System;
using LBank.Net.Interfaces.Clients;
using LBank.Net.Objects.Options;
using CryptoExchange.Net.Clients;
using Microsoft.Extensions.Options;
using LBank.Net.Clients.SpotApi;
using LBank.Net.Interfaces.Clients.SpotApi;

namespace LBank.Net.Clients
{
    /// <inheritdoc cref="ILBankRestClient" />
    public class LBankRestClient : BaseRestClient<LBankEnvironment, LBankCredentials>, ILBankRestClient
    {
        #region Api clients

         /// <inheritdoc />
        public ILBankRestClientSpotApi SpotApi { get; }

        #endregion

        #region constructor/destructor

        /// <summary>
        /// Create a new instance of the LBankRestClient using provided options
        /// </summary>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public LBankRestClient(Action<LBankRestOptions>? optionsDelegate = null)
            : this(null, null, Options.Create(ApplyOptionsDelegate(optionsDelegate)))
        {
        }

        /// <summary>
        /// Create a new instance of the LBankRestClient using provided options
        /// </summary>
        /// <param name="options">Option configuration</param>
        /// <param name="loggerFactory">The logger factory</param>
        /// <param name="httpClient">Http client for this client</param>
        public LBankRestClient(HttpClient? httpClient, ILoggerFactory? loggerFactory, IOptions<LBankRestOptions> options) : base(loggerFactory, "LBank")
        {
            Initialize(options.Value);

            SpotApi = AddApiClient(new LBankRestClientSpotApi(loggerFactory, httpClient, options.Value));
        }

        #endregion

        /// <summary>
        /// Set the default options to be used when creating new clients
        /// </summary>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public static void SetDefaultOptions(Action<LBankRestOptions> optionsDelegate)
        {
            LBankRestOptions.Default = ApplyOptionsDelegate(optionsDelegate);
        }
    }
}
