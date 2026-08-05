using System;
using System.Text.Json.Serialization;

namespace LBank.Net.Objects.Models;

/// <summary>
/// Symbol price ticker
/// </summary>
public record LBankSymbolTicker
{
    /// <summary>
    /// ["<c>symbol</c>"] Symbol
    /// </summary>
    [JsonPropertyName("symbol")]
    public string Symbol { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>ticker</c>"] Ticker
    /// </summary>
    [JsonPropertyName("ticker")]
    public LBankTicker Ticker { get; set; } = null!;
    /// <summary>
    /// ["<c>timestamp</c>"] Timestamp
    /// </summary>
    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; }
}

/// <summary>
/// Price ticker
/// </summary>
public record LBankTicker
{
    /// <summary>
    /// ["<c>high</c>"] Highest price
    /// </summary>
    [JsonPropertyName("high")]
    public decimal HighPrice { get; set; }
    /// <summary>
    /// ["<c>vol</c>"] Volume
    /// </summary>
    [JsonPropertyName("vol")]
    public decimal Volume { get; set; }
    /// <summary>
    /// ["<c>low</c>"] Lowest price
    /// </summary>
    [JsonPropertyName("low")]
    public decimal LowPrice { get; set; }
    /// <summary>
    /// ["<c>change</c>"] Price change percentage
    /// </summary>
    [JsonPropertyName("change")]
    public decimal PriceChangePercentage { get; set; }
    /// <summary>
    /// ["<c>turnover</c>"] Turnover
    /// </summary>
    [JsonPropertyName("turnover")]
    public decimal Turnover { get; set; }
    /// <summary>
    /// ["<c>latest</c>"] Latest price
    /// </summary>
    [JsonPropertyName("latest")]
    public decimal LastPrice { get; set; }
}

