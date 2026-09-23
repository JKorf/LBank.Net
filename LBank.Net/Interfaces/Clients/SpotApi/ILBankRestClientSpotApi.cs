using CryptoExchange.Net.Interfaces.Clients;
using System;

namespace LBank.Net.Interfaces.Clients.SpotApi
{
    /// <summary>
    /// LBank Spot API endpoints
    /// </summary>
    public interface ILBankRestClientSpotApi : IRestApiClient<LBankCredentials>, IDisposable
    {
        /// <summary>
        /// Endpoints related to account settings, info or actions
        /// </summary>
        /// <see cref="ILBankRestClientSpotApiAccount" />
        public ILBankRestClientSpotApiAccount Account { get; }

        /// <summary>
        /// Endpoints related to retrieving market and system data
        /// </summary>
        /// <see cref="ILBankRestClientSpotApiExchangeData" />
        public ILBankRestClientSpotApiExchangeData ExchangeData { get; }

        /// <summary>
        /// Endpoints related to orders and trades
        /// </summary>
        /// <see cref="ILBankRestClientSpotApiTrading" />
        public ILBankRestClientSpotApiTrading Trading { get; }

        /// <summary>
        /// [V1] Get the shared rest requests client. For new implementations prefer <see cref="SharedApi"/>
        /// </summary>
        public ILBankRestClientSpotApiShared SharedClient { get; }
        /// <summary>
        /// [V2] Gets the aggregate Shared API interface. Shared APIs provide a common,
        /// exchange-independent contract for accessing functionality across different
        /// exchange client libraries.
        /// </summary>
        ILBankRestClientSpotSharedApi SharedApi { get; }
    }
}
