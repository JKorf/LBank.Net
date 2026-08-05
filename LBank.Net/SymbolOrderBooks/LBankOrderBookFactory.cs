using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.SharedApis;
using CryptoExchange.Net.OrderBook;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using LBank.Net.Interfaces;
using LBank.Net.Interfaces.Clients;
using LBank.Net.Objects.Options;

namespace LBank.Net.SymbolOrderBooks
{
    /// <summary>
    /// LBank order book factory
    /// </summary>
    public class LBankOrderBookFactory : ILBankOrderBookFactory
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="serviceProvider">Service provider for resolving logging and clients</param>
        public LBankOrderBookFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            
            Spot = new OrderBookFactory<LBankOrderBookOptions>(CreateSpot, Create);
        }

         /// <inheritdoc />
        public IOrderBookFactory<LBankOrderBookOptions> Spot { get; }

        /// <inheritdoc />
        public ISymbolOrderBook Create(SharedSymbol symbol, Action<LBankOrderBookOptions>? options = null)
        {
            var symbolName = symbol.GetSymbol(LBankExchange.FormatSymbol);
            return CreateSpot(symbolName, options);
        }

         /// <inheritdoc />
        public ISymbolOrderBook CreateSpot(string symbol, Action<LBankOrderBookOptions>? options = null)
            => new LBankSpotSymbolOrderBook(symbol, options, 
                                                          _serviceProvider.GetRequiredService<ILoggerFactory>(),
                                                          _serviceProvider.GetRequiredService<ILBankSocketClient>());


    }
}
