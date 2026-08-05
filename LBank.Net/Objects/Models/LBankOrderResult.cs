using System.Text.Json.Serialization;

namespace LBank.Net.Objects.Models;

/// <summary>
/// Order result
/// </summary>
public record LBankOrderResult
{
    /// <summary>
    /// ["<c>symbol</c>"] Symbol
    /// </summary>
    [JsonPropertyName("symbol")]
    public string Symbol { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>order_id</c>"] Order id
    /// </summary>
    [JsonPropertyName("order_id")]
    public string OrderId { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>custom_id</c>"] Client order id
    /// </summary>
    [JsonPropertyName("custom_id")]
    public string? ClientOrderId { get; set; } = string.Empty;
}

