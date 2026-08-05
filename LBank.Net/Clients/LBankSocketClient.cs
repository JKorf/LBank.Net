using CryptoExchange.Net.Clients;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using LBank.Net.Interfaces.Clients;
using LBank.Net.Objects.Options;
using LBank.Net.Interfaces.Clients.SpotApi;
using LBank.Net.Clients.SpotApi;

namespace LBank.Net.Clients
{
    /// <inheritdoc cref="ILBankSocketClient" />
    public class LBankSocketClient : BaseSocketClient<LBankEnvironment, LBankCredentials>, ILBankSocketClient
    {
        #region fields
        #endregion

        #region Api clients

         /// <inheritdoc />
        public ILBankSocketClientSpotApi SpotApi { get; }


        #endregion

        #region constructor/destructor

        /// <summary>
        /// Create a new instance of LBankSocketClient
        /// </summary>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public LBankSocketClient(Action<LBankSocketOptions>? optionsDelegate = null)
            : this(Options.Create(ApplyOptionsDelegate(optionsDelegate)), null)
        {
        }

        /// <summary>
        /// Create a new instance of LBankSocketClient
        /// </summary>
        /// <param name="loggerFactory">The logger factory</param>
        /// <param name="options">Option configuration</param>
        public LBankSocketClient(IOptions<LBankSocketOptions> options, ILoggerFactory? loggerFactory = null) : base(loggerFactory, "LBank")
        {
            Initialize(options.Value);

            SpotApi = AddApiClient(new LBankSocketClientSpotApi(loggerFactory, options.Value));
        }
        #endregion

        /// <summary>
        /// Set the default options to be used when creating new clients
        /// </summary>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public static void SetDefaultOptions(Action<LBankSocketOptions> optionsDelegate)
        {
            LBankSocketOptions.Default = ApplyOptionsDelegate(optionsDelegate);
        }
    }
}
