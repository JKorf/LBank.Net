using CryptoExchange.Net;
using CryptoExchange.Net.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading;
using LBank.Net;
using LBank.Net.Clients;
using LBank.Net.Interfaces;
using LBank.Net.Interfaces.Clients;
using LBank.Net.Objects.Options;
using LBank.Net.SymbolOrderBooks;

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
            var options = new LBankOptions();
            // Reset environment so we know if they're overridden
            options.Rest.Environment = null!;
            options.Socket.Environment = null!;
            try
            {
                configuration.Bind(options);
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException("Invalid configuration provided", ex);
            }

            if (options.Rest == null || options.Socket == null)
                throw new ArgumentException("Options null");

            var restEnvName = options.Rest.Environment?.Name ?? options.Environment?.Name ?? LBankEnvironment.Live.Name;
            var socketEnvName = options.Socket.Environment?.Name ?? options.Environment?.Name ?? LBankEnvironment.Live.Name;
            options.Rest.Environment = LBankEnvironment.GetEnvironmentByName(restEnvName) ?? options.Rest.Environment!;
            options.Rest.ApiCredentials = options.Rest.ApiCredentials ?? options.ApiCredentials;
            options.Socket.Environment = LBankEnvironment.GetEnvironmentByName(socketEnvName) ?? options.Socket.Environment!;
            options.Socket.ApiCredentials = options.Socket.ApiCredentials ?? options.ApiCredentials;


            services.AddSingleton(x => Options.Options.Create(options.Rest));
            services.AddSingleton(x => Options.Options.Create(options.Socket));

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
            var options = new LBankOptions();
            // Reset environment so we know if they're overridden
            options.Rest.Environment = null!;
            options.Socket.Environment = null!;
            optionsDelegate?.Invoke(options);
            if (options.Rest == null || options.Socket == null)
                throw new ArgumentException("Options null");

            options.Rest.Environment = options.Rest.Environment ?? options.Environment ?? LBankEnvironment.Live;
            options.Rest.ApiCredentials = options.Rest.ApiCredentials ?? options.ApiCredentials;
            options.Socket.Environment = options.Socket.Environment ?? options.Environment ?? LBankEnvironment.Live;
            options.Socket.ApiCredentials = options.Socket.ApiCredentials ?? options.ApiCredentials;

            services.AddSingleton(x => Options.Options.Create(options.Rest));
            services.AddSingleton(x => Options.Options.Create(options.Socket));

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


            return services;
        }
    }
}
