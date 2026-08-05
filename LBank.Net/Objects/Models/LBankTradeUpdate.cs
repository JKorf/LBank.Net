using System;
using System.Text.Json.Serialization;
using LBank.Net.Enums;

namespace LBank.Net.Objects.Models;

/// <summary>
/// Trade info
/// </summary>
public record LBankTradeUpdate
{
    /// <summary>
    /// ["<c>price</c>"] Price
    /// </summary>
    [JsonPropertyName("price")]
    public decimal Price { get; set; }
    /// <summary>
    /// ["<c>volume</c>"] Quantity
    /// </summary>
    [JsonPropertyName("volume")]
    public decimal Quantity { get; set; }
    /// <summary>
    /// ["<c>amount</c>"] Quantity in quote asset
    /// </summary>
    [JsonPropertyName("amount")]
    public decimal QuoteQuantity { get; set; }
    /// <summary>
    /// ["<c>TS</c>"] Timestamp
    /// </summary>
    [JsonPropertyName("TS")]
    public DateTime Timestamp { get; set; }
    /// <summary>
    /// ["<c>direction</c>"] Side
    /// </summary>
    [JsonPropertyName("direction")]
    public OrderSide Side { get; set; }
}

