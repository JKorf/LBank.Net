using System;
using System.Text.Json.Serialization;
using LBank.Net.Enums;

namespace LBank.Net.Objects.Models;

/// <summary>
/// Order info
/// </summary>
public record LBankOrderUpdate
{
    /// <summary>
    /// ["<c>accAmt</c>"] Accumulative filled quantity
    /// </summary>
    [JsonPropertyName("accAmt")]
    public decimal AccumulativeQuantity { get; set; }
    /// <summary>
    /// ["<c>amount</c>"] Trade quantity
    /// </summary>
    [JsonPropertyName("amount")]
    public decimal? Quantity { get; set; }
    /// <summary>
    /// ["<c>avgPrice</c>"] Average price
    /// </summary>
    [JsonPropertyName("avgPrice")]
    public decimal? AveragePrice { get; set; }
    /// <summary>
    /// ["<c>symbol</c>"] Symbol
    /// </summary>
    [JsonPropertyName("symbol")]
    public string Symbol { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>type</c>"] Type
    /// </summary>
    [JsonPropertyName("type")]
    public OrderType Type { get; set; }
    /// <summary>
    /// ["<c>orderAmt</c>"] Order quantity
    /// </summary>
    [JsonPropertyName("orderAmt")]
    public decimal OrderQuantity { get; set; }
    /// <summary>
    /// ["<c>status</c>"] Status
    /// </summary>
    [JsonPropertyName("orderStatus")]
    public OrderStatus Status { get; set; }
    /// <summary>
    /// ["<c>orderPrice</c>"] Order price
    /// </summary>
    [JsonPropertyName("orderPrice")]
    public decimal OrderPrice { get; set; }
    /// <summary>
    /// ["<c>price</c>"] Trade price
    /// </summary>
    [JsonPropertyName("price")]
    public decimal? Price { get; set; }
    /// <summary>
    /// ["<c>role</c>"] Role
    /// </summary>
    [JsonPropertyName("role")]
    public TradeRole? Role { get; set; }
    /// <summary>
    /// ["<c>remainAmt</c>"] Quantity remaining
    /// </summary>
    [JsonPropertyName("remainAmt")]
    public decimal QuantityRemaining { get; set; }
    /// <summary>
    /// ["<c>updateTime</c>"] Update time
    /// </summary>
    [JsonPropertyName("updateTime")]
    public DateTime UpdateTime { get; set; }
    /// <summary>
    /// ["<c>uuid</c>"] Order id
    /// </summary>
    [JsonPropertyName("uuid")]
    public string OrderId { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>txUuid</c>"] Trade id
    /// </summary>
    [JsonPropertyName("txUuid")]
    public string? TradeId { get; set; }
    /// <summary>
    /// ["<c>volumePrice</c>"] Value of filled quantity in quote asset
    /// </summary>
    [JsonPropertyName("volumePrice")]
    public decimal QuoteQuantityFilled { get; set; }
}
