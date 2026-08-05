using CryptoExchange.Net.Objects;
using CryptoExchange.Net.RateLimiting.Interfaces;
using CryptoExchange.Net.RateLimiting;
using System;
using CryptoExchange.Net.SharedApis;
using LBank.Net.Converters;
using System.Text.Json;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Converters;
using CryptoExchange.Net.RateLimiting.Guards;
using CryptoExchange.Net.RateLimiting.Filters;

namespace LBank.Net
{
    /// <summary>
    /// LBank exchange information and configuration
    /// </summary>
    public static class LBankExchange
    {
        internal static JsonSerializerOptions _serializerContext = SerializerOptions.WithConverters(JsonSerializerContextCache.GetOrCreate<LBankSourceGenerationContext>());
        internal static ParameterSerializationSettings _parameterSerializationSettings = new ParameterSerializationSettings
        {
            Sort = true,
            Decimal = DecimalSerialization.String
        };

        /// <summary>
        /// Platform metadata
        /// </summary>
        public static PlatformInfo Metadata { get; } = new PlatformInfo(
                "LBank",
                "LBank",
                "https://raw.githubusercontent.com/JKorf/LBank.Net/main/LBank.Net/Icon/icon.png",
                "https://www.lbank.com/",
                ["https://www.lbank.com/docs/#interaction-introduction"],
                PlatformType.CryptoCurrencyExchange,
                CentralizationType.Centralized,
                LBankEnvironment.All
                );

        /// <summary>
        /// Aliases for LBank assets
        /// </summary>
        public static AssetAliasConfiguration AssetAliases { get; } = new AssetAliasConfiguration
        {
            Aliases = [
                new AssetAlias("usdt", SharedSymbol.UsdOrStable.ToUpperInvariant(), AliasType.OnlyToExchange)
            ]
        };

        /// <summary>
        /// Format a base and quote asset to an LBank recognized symbol 
        /// </summary>
        /// <param name="baseAsset">Base asset</param>
        /// <param name="quoteAsset">Quote asset</param>
        /// <param name="tradingMode">Trading mode</param>
        /// <param name="deliverTime">Delivery time for delivery futures</param>
        /// <returns></returns>
        public static string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverTime = null)
        {
            baseAsset = AssetAliases.CommonToExchangeName(baseAsset.ToUpperInvariant());
            quoteAsset = AssetAliases.CommonToExchangeName(quoteAsset.ToUpperInvariant());

            return baseAsset.ToLowerInvariant() + "_" + quoteAsset.ToLowerInvariant();
        }

        /// <summary>
        /// Rate limiter configuration for the LBank API
        /// </summary>
        public static LBankRateLimiters RateLimiter { get; } = new LBankRateLimiters();
    }

    /// <summary>
    /// Rate limiter configuration for the LBank API
    /// </summary>
    public class LBankRateLimiters
    {
        /// <summary>
        /// Event for when a rate limit is triggered
        /// </summary>
        public event Action<RateLimitEvent> RateLimitTriggered;
        /// <summary>
        /// Event when the rate limit is updated. Note that it's only updated when a request is send, so there are no specific updates when the current usage is decaying.
        /// </summary>
        public event Action<RateLimitUpdateEvent> RateLimitUpdated;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        internal LBankRateLimiters()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            Initialize();
        }

        private void Initialize()
        {
            RestApi = new RateLimitGate("RestApi")
                .AddGuard(new RateLimitGuard(RateLimitGuard.PerHost, new LimitItemTypeFilter(RateLimitItemType.Request), 200, TimeSpan.FromSeconds(10), RateLimitWindowType.Sliding));
            OrderApi = new RateLimitGate("OrderApi")
                .AddGuard(new RateLimitGuard(RateLimitGuard.PerHost, new LimitItemTypeFilter(RateLimitItemType.Request), 500, TimeSpan.FromSeconds(10), RateLimitWindowType.Sliding));

            RestApi.RateLimitTriggered += (x) => RateLimitTriggered?.Invoke(x);
            RestApi.RateLimitUpdated += (x) => RateLimitUpdated?.Invoke(x);
            OrderApi.RateLimitTriggered += (x) => RateLimitTriggered?.Invoke(x);
            OrderApi.RateLimitUpdated += (x) => RateLimitUpdated?.Invoke(x);
        }


        internal IRateLimitGate RestApi { get; private set; }
        internal IRateLimitGate OrderApi { get; private set; }

    }
}
