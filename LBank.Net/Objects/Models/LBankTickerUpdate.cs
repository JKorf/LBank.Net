using System.Text.Json.Serialization;
using LBank.Net.Enums;

namespace LBank.Net.Objects.Models;

/// <summary>
/// Price ticker update
/// </summary>
public record LBankTickerUpdate
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
    /// <summary>
    /// ["<c>dir</c>"] Last trade side
    /// </summary>
    [JsonPropertyName("dir")]
    public OrderSide Side { get; set; }
    /// <summary>
    /// ["<c>usd</c>"] Price in USD
    /// </summary>
    [JsonPropertyName("usd")]
    public decimal LastPriceBaseAssetUsd { get; set; }
    /// <summary>
    /// ["<c>cny</c>"] Price in CNY
    /// </summary>
    [JsonPropertyName("cny")]
    public decimal LastPriceBaseAssetCny { get; set; }
    /// <summary>
    /// ["<c>to_usd</c>"] Quote asset price in USD
    /// </summary>
    [JsonPropertyName("to_usd")]
    public decimal LastPriceQuoteAssetUsd { get; set; }
    /// <summary>
    /// ["<c>to_cny</c>"] Quote asset price in CNY
    /// </summary>
    [JsonPropertyName("to_cny")]
    public decimal LastPriceQuoteAssetCny { get; set; }
}

