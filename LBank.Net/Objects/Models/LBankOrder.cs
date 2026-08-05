using System;
using System.Text.Json.Serialization;
using LBank.Net.Enums;

namespace LBank.Net.Objects.Models;

/// <summary>
/// Orders page
/// </summary>
public record LBankOrderPage : LBankPage
{
    /// <summary>
    /// ["<c>orders</c>"] Orders
    /// </summary>
    [JsonPropertyName("orders")]
    public LBankOrder[] Orders { get; set; } = [];

}

/// <summary>
/// Order info
/// </summary>
public record LBankOrder
{
    /// <summary>
    /// ["<c>cummulativeQuoteQty</c>"] Cummulative quote quantity
    /// </summary>
    [JsonPropertyName("cummulativeQuoteQty")]
    public decimal CummulativeQuoteQuantity { get; set; }
    /// <summary>
    /// ["<c>symbol</c>"] Symbol
    /// </summary>
    [JsonPropertyName("symbol")]
    public string Symbol { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>executedQty</c>"] Executed quantity
    /// </summary>
    [JsonPropertyName("executedQty")]
    public decimal ExecutedQuantity { get; set; }
    /// <summary>
    /// ["<c>orderId</c>"] Order id
    /// </summary>
    [JsonPropertyName("orderId")]
    public string OrderId { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>origClientOrderId</c>"] Client order id
    /// </summary>
    [JsonPropertyName("origClientOrderId")]
    public string? ClientOrderId { get; set; }
    [JsonInclude]
    [JsonPropertyName("clientOrderId")]
    internal string? ClientOrderIdInt
    {
        set => ClientOrderId = value;
    }
    /// <summary>
    /// ["<c>origQty</c>"] Original quantity
    /// </summary>
    [JsonPropertyName("origQty")]
    public decimal OriginalQuantity { get; set; }
    /// <summary>
    /// ["<c>price</c>"] Price
    /// </summary>
    [JsonPropertyName("price")]
    public decimal Price { get; set; }
    /// <summary>
    /// ["<c>origQuoteOrderQty</c>"] Original quote order quantity
    /// </summary>
    [JsonPropertyName("origQuoteOrderQty")]
    public decimal OriginalQuoteOrderQuantity { get; set; }
    /// <summary>
    /// ["<c>updateTime</c>"] Update time
    /// </summary>
    [JsonPropertyName("updateTime")]
    public DateTime UpdateTime { get; set; }
    /// <summary>
    /// ["<c>time</c>"] Timestamp
    /// </summary>
    [JsonPropertyName("time")]
    public DateTime Timestamp { get; set; }
    /// <summary>
    /// ["<c>type</c>"] Type
    /// </summary>
    [JsonPropertyName("type")]
    public OrderType Type { get; set; }
    [JsonInclude]
    [JsonPropertyName("tradeType")]
    internal OrderType TradeType
    {
        set => Type = value;
    }
    /// <summary>
    /// ["<c>status</c>"] Status
    /// </summary>
    [JsonPropertyName("status")]
    public OrderStatus Status { get; set; }
}
