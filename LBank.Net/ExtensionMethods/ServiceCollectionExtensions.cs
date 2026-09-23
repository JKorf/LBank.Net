using CryptoExchange.Net;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.SharedApis;
using LBank.Net;
using LBank.Net.Clients;
using LBank.Net.Interfaces;
using LBank.Net.Interfaces.Clients;
using LBank.Net.Objects.Options;
using LBank.Net.SymbolOrderBooks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extensions for DI
    /// </summary>
    public static class ServiceCollectionExtensions
    {

        /// <summary>
        /// Add services such as the ILBankRestClient and ILBankSocketClient. Configures the services based on the provided configuration.
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="configuration">The configuration(section) containing the options</param>
        /// <returns></returns>
        public static IServiceCollection AddLBank(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var options = LBankOptions.CreateFromConfiguration(configuration);
            services.AddSingleton(Options.Options.Create(options.Rest));
            services.AddSingleton(Options.Options.Create(options.Socket));
            services.AddSingleton(Options.Options.Create(options));

            return AddLBankCore(services, options.SocketClientLifeTime);
        }

        /// <summary>
        /// Add services such as the ILBankRestClient and ILBankSocketClient. Services will be configured based on the provided options.
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="optionsDelegate">Set options for the LBank services</param>
        /// <returns></returns>
        public static IServiceCollection AddLBank(
            this IServiceCollection services,
            Action<LBankOptions>? optionsDelegate = null)
        {
            var options = LBankOptions.Create(optionsDelegate);
            services.AddSingleton(Options.Options.Create(options.Rest));
            services.AddSingleton(Options.Options.Create(options.Socket));
            services.AddSingleton(Options.Options.Create(options));

            return AddLBankCore(services, options.SocketClientLifeTime);
        }

        private static IServiceCollection AddLBankCore(
            this IServiceCollection services,
            ServiceLifetime? socketClientLifeTime = null)
        {
            services.AddHttpClient<ILBankRestClient, LBankRestClient>((client, serviceProvider) =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<LBankRestOptions>>().Value;
                client.Timeout = options.RequestTimeout;
                return new LBankRestClient(client, serviceProvider.GetRequiredService<ILoggerFactory>(), serviceProvider.GetRequiredService<IOptions<LBankRestOptions>>());
            }).ConfigurePrimaryHttpMessageHandler((serviceProvider) => {
                var options = serviceProvider.GetRequiredService<IOptions<LBankRestOptions>>().Value;
                return LibraryHelpers.CreateHttpClientMessageHandler(options);
            }).SetHandlerLifetime(Timeout.InfiniteTimeSpan);
            services.Add(new ServiceDescriptor(typeof(ILBankSocketClient), x => { return new LBankSocketClient(x.GetRequiredService<IOptions<LBankSocketOptions>>(), x.GetRequiredService<ILoggerFactory>()); }, socketClientLifeTime ?? ServiceLifetime.Singleton));

            services.AddTransient<ILBankOrderBookFactory, LBankOrderBookFactory>();
            services.AddTransient<ITrackerFactory, LBankTrackerFactory>();
            services.AddTransient<ILBankTrackerFactory, LBankTrackerFactory>();
            services.AddSingleton<ILBankUserClientProvider, LBankUserClientProvider>(x =>
                new LBankUserClientProvider(
                    x.GetRequiredService<IHttpClientFactory>().CreateClient(typeof(ILBankRestClient).Name),
                    x.GetRequiredService<ILoggerFactory>(),
                    x.GetRequiredService<IOptions<LBankRestOptions>>(),
                    x.GetRequiredService<IOptions<LBankSocketOptions>>()));

            services.RegisterSharedRestInterfaces(x => x.GetRequiredService<ILBankRestClient>().SpotApi.SharedClient);
            services.RegisterSharedSocketInterfaces(x => x.GetRequiredService<ILBankSocketClient>().SpotApi.SharedClient);

            services.RegisterSharedApiClient<
                ILBankSharedApiClient,
                LBankSharedApiClient>(sharedApis => sharedApis
                    .Add(client => client.SpotRest)
                    .Add(client => client.SpotSocket)
                    );
            return services;
        }
    }
}
