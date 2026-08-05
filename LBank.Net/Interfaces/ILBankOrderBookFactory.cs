using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.SharedApis;
using System;
using LBank.Net.Objects.Options;

namespace LBank.Net.Interfaces
{
    /// <summary>
    /// LBank local order book factory
    /// </summary>
    public interface ILBankOrderBookFactory
    {
        /// <summary>
        /// Spot order book factory methods
        /// </summary>
        IOrderBookFactory<LBankOrderBookOptions> Spot { get; }

        /// <summary>
        /// Create a SymbolOrderBook for the symbol
        /// </summary>
        /// <param name="symbol">The symbol</param>
        /// <param name="options">Book options</param>
        /// <returns></returns>
        ISymbolOrderBook Create(SharedSymbol symbol, Action<LBankOrderBookOptions>? options = null);

        /// <summary>
        /// Create a new Spot local order book instance
        /// </summary>
        ISymbolOrderBook CreateSpot(string symbol, Action<LBankOrderBookOptions>? options = null);

    }
}