using System.Text.Json.Serialization;

namespace LBank.Net.Objects.Models;

/// <summary>
/// Book ticker
/// </summary>
public record LBankBookTicker
{
    /// <summary>
    /// ["<c>symbol</c>"] Symbol
    /// </summary>
    [JsonPropertyName("symbol")]
    public string Symbol { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>askPrice</c>"] Ask price
    /// </summary>
    [JsonPropertyName("askPrice")]
    public decimal BestAskPrice { get; set; }
    /// <summary>
    /// ["<c>askQty</c>"] Ask quantity
    /// </summary>
    [JsonPropertyName("askQty")]
    public decimal BestAskQuantity { get; set; }
    /// <summary>
    /// ["<c>bidQty</c>"] Bid quantity
    /// </summary>
    [JsonPropertyName("bidQty")]
    public decimal BestBidQuantity { get; set; }
    /// <summary>
    /// ["<c>bidPrice</c>"] Bid price
    /// </summary>
    [JsonPropertyName("bidPrice")]
    public decimal BestBidPrice { get; set; }
}

