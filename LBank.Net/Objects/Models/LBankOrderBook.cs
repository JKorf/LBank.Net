using System;
using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Interfaces;

namespace LBank.Net.Objects.Models;

/// <summary>
/// Order book snapshot
/// </summary>
public record LBankOrderBook
{
    /// <summary>
    /// ["<c>timestamp</c>"] Timestamp
    /// </summary>
    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; }
    /// <summary>
    /// ["<c>asks</c>"] Asks
    /// </summary>
    [JsonPropertyName("asks")]
    public LBankOrderBookEntry[] Asks { get; set; } = [];
    /// <summary>
    /// ["<c>bids</c>"] Bids
    /// </summary>
    [JsonPropertyName("bids")]
    public LBankOrderBookEntry[] Bids { get; set; } = [];
}

/// <summary>
/// Order book entry
/// </summary>
[JsonConverter(typeof(ArrayConverter<LBankOrderBookEntry>))]
public record LBankOrderBookEntry : ISymbolOrderBookEntry
{
    /// <summary>
    /// Price
    /// </summary>
    [ArrayProperty(0)]
    public decimal Price { get; set; }
    /// <summary>
    /// Quantity
    /// </summary>
    [ArrayProperty(1)]
    public decimal Quantity { get; set; }
}
