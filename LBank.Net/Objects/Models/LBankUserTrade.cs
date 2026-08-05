using System;
using System.Text.Json.Serialization;

namespace LBank.Net.Objects.Models;

/// <summary>
/// User trade info
/// </summary>
public record LBankUserTrade
{
    /// <summary>
    /// ["<c>symbol</c>"] Symbol
    /// </summary>
    [JsonPropertyName("symbol")]
    public string Symbol { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>quoteQty</c>"] Quote quantity
    /// </summary>
    [JsonPropertyName("quoteQty")]
    public decimal QuoteQuantity { get; set; }
    /// <summary>
    /// ["<c>orderId</c>"] Order id
    /// </summary>
    [JsonPropertyName("orderId")]
    public string OrderId { get; set; } = string.Empty;
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
    /// ["<c>commission</c>"] Commission
    /// </summary>
    [JsonPropertyName("commission")]
    public decimal Commission { get; set; }
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
    /// ["<c>isMaker</c>"] Is maker
    /// </summary>
    [JsonPropertyName("isMaker")]
    public bool IsMaker { get; set; }
    /// <summary>
    /// ["<c>isBuyer</c>"] Is buyer
    /// </summary>
    [JsonPropertyName("isBuyer")]
    public bool IsBuyer { get; set; }
}

