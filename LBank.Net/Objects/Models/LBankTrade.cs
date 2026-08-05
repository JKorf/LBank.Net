using System;
using System.Text.Json.Serialization;

namespace LBank.Net.Objects.Models;

/// <summary>
/// Trade info
/// </summary>
public record LBankTrade
{
    /// <summary>
    /// ["<c>quoteQty</c>"] Quote quantity
    /// </summary>
    [JsonPropertyName("quoteQty")]
    public decimal QuoteQuantity { get; set; }
    /// <summary>
    /// ["<c>price</c>"] Price
    /// </summary>
    [JsonPropertyName("price")]
    public decimal Price { get; set; }
    /// <summary>
    /// ["<c>qty</c>"] Quantity
    /// </summary>
    [JsonPropertyName("qty")]
    public decimal Quantity { get; set; }
    /// <summary>
    /// ["<c>id</c>"] Id
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>time</c>"] Timestamp
    /// </summary>
    [JsonPropertyName("time")]
    public DateTime Timestamp { get; set; }
    /// <summary>
    /// ["<c>isBuyerMaker</c>"] Is buyer maker
    /// </summary>
    [JsonPropertyName("isBuyerMaker")]
    public bool IsBuyerMaker { get; set; }
}

