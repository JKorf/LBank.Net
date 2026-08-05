using CryptoExchange.Net.SharedApis;
using CryptoExchange.Net.Trackers.Klines;
using CryptoExchange.Net.Trackers.Trades;
using CryptoExchange.Net.Trackers.UserData.Interfaces;
using CryptoExchange.Net.Trackers.UserData.Objects;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using LBank.Net.Clients;
using LBank.Net.Interfaces;
using LBank.Net.Interfaces.Clients;

namespace LBank.Net
{
    /// <inheritdoc />
    public class LBankTrackerFactory : ILBankTrackerFactory
    {
        private readonly IServiceProvider? _serviceProvider;

        /// <summary>
        /// ctor
        /// </summary>
        public LBankTrackerFactory()
        {
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="serviceProvider">Service provider for resolving logging and clients</param>
        public LBankTrackerFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <inheritdoc />
        public bool CanCreateKlineTracker(SharedSymbol symbol, SharedKlineInterval interval)
        {
            var client = _serviceProvider?.GetRequiredService<ILBankSocketClient>() ?? new LBankSocketClient();            
            return client.SpotApi.SharedClient.SubscribeKlineOptions.IsSupported(interval); 
        }

        /// <inheritdoc />
        public bool CanCreateTradeTracker(SharedSymbol symbol) => true;

        /// <inheritdoc />
        public IKlineTracker CreateKlineTracker(SharedSymbol symbol, SharedKlineInterval interval, int? limit = null, TimeSpan? period = null, ExchangeParameters? exchangeParameters = null)
        {
            var restClient = _serviceProvider?.GetRequiredService<ILBankRestClient>() ?? new LBankRestClient();
            var socketClient = _serviceProvider?.GetRequiredService<ILBankSocketClient>() ?? new LBankSocketClient();

            var sharedRestClient = restClient.SpotApi.SharedClient;
            var sharedSocketClient = socketClient.SpotApi.SharedClient;

            return new KlineTracker(
                _serviceProvider?.GetRequiredService<ILoggerFactory>().CreateLogger(restClient.Exchange),
                sharedRestClient,
                sharedSocketClient,
                symbol,
                interval,
                limit,
                period,
                exchangeParameters
                );
        }
        /// <inheritdoc />
        public ITradeTracker CreateTradeTracker(SharedSymbol symbol, int? limit = null, TimeSpan? period = null, ExchangeParameters? exchangeParameters = null)
        {
            var restClient = _serviceProvider?.GetRequiredService<ILBankRestClient>() ?? new LBankRestClient();
            var socketClient = _serviceProvider?.GetRequiredService<ILBankSocketClient>() ?? new LBankSocketClient();

            var sharedRestClient = restClient.SpotApi.SharedClient;
            var sharedSocketClient = socketClient.SpotApi.SharedClient;

            return new TradeTracker(
                _serviceProvider?.GetRequiredService<ILoggerFactory>().CreateLogger(restClient.Exchange),
                sharedRestClient,
                null,
                sharedSocketClient,
                symbol,
                limit,
                period,
                TradeQuantityType.BaseAsset,
                exchangeParameters
                );
        }

        /// <inheritdoc />
        public IUserSpotDataTracker CreateUserSpotDataTracker(SpotUserDataTrackerConfig? config = null, ExchangeParameters? exchangeParameters = null)
        {
            var restClient = _serviceProvider?.GetRequiredService<ILBankRestClient>() ?? new LBankRestClient();
            var socketClient = _serviceProvider?.GetRequiredService<ILBankSocketClient>() ?? new LBankSocketClient();
            return new LBankUserSpotDataTracker(
                _serviceProvider?.GetRequiredService<ILogger<LBankUserSpotDataTracker>>() ?? new NullLogger<LBankUserSpotDataTracker>(),
                restClient,
                socketClient,
                null,
                config,
                exchangeParameters
                );
        }

        /// <inheritdoc />
        public IUserSpotDataTracker CreateUserSpotDataTracker(string userIdentifier, LBankCredentials credentials, SpotUserDataTrackerConfig? config = null, LBankEnvironment? environment = null, ExchangeParameters? exchangeParameters = null)
        {
            var clientProvider = _serviceProvider?.GetRequiredService<ILBankUserClientProvider>() ?? new LBankUserClientProvider();
            var restClient = clientProvider.GetRestClient(userIdentifier, credentials, environment);
            var socketClient = clientProvider.GetSocketClient(userIdentifier, credentials, environment);
            return new LBankUserSpotDataTracker(
                _serviceProvider?.GetRequiredService<ILogger<LBankUserSpotDataTracker>>() ?? new NullLogger<LBankUserSpotDataTracker>(),
                restClient,
                socketClient,
                userIdentifier,
                config,
                exchangeParameters
                );
        }
    }
}
