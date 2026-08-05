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
        /// Get the shared rest requests client. This interface is shared with other exchanges to allow for a common implementation for different exchanges.
        /// </summary>
        public ILBankRestClientSpotApiShared SharedClient { get; }
    }
}
