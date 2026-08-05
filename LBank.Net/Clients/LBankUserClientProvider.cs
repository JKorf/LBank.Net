using LBank.Net.Interfaces.Clients;
using LBank.Net.Objects.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using CryptoExchange.Net.Clients;

namespace LBank.Net.Clients
{
    /// <inheritdoc />
    public class LBankUserClientProvider : UserClientProvider<
        ILBankRestClient,
        ILBankSocketClient,
        LBankRestOptions,
        LBankSocketOptions,
        LBankCredentials,
        LBankEnvironment
        >, ILBankUserClientProvider
    {
        /// <inheritdoc />
        public override string ExchangeName => LBankExchange.Metadata.Id;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="optionsDelegate">Options to use for created clients</param>
        public LBankUserClientProvider(Action<LBankOptions>? optionsDelegate = null)
            : this(null, null, Options.Create(ApplyOptionsDelegate(optionsDelegate).Rest), Options.Create(ApplyOptionsDelegate(optionsDelegate).Socket))
        {
        }

        /// <summary>
        /// ctor
        /// </summary>
        public LBankUserClientProvider(
            HttpClient? httpClient,
            ILoggerFactory? loggerFactory,
            IOptions<LBankRestOptions> restOptions,
            IOptions<LBankSocketOptions> socketOptions)
            : base(httpClient, loggerFactory, restOptions, socketOptions)
        {
        }

        /// <inheritdoc />
        protected override ILBankRestClient ConstructRestClient(HttpClient client, ILoggerFactory? loggerFactory, IOptions<LBankRestOptions> options)
            => new LBankRestClient(client, loggerFactory, options);
        /// <inheritdoc />
        protected override ILBankSocketClient ConstructSocketClient(ILoggerFactory? loggerFactory, IOptions<LBankSocketOptions> options)
            => new LBankSocketClient(options, loggerFactory);
    }
}
