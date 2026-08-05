using System.Text.Json.Serialization;

namespace LBank.Net.Objects.Models;

/// <summary>
/// Symbol info
/// </summary>
public record LBankSymbol
{
    /// <summary>
    /// ["<c>minTranQua</c>"] Min transaction quantity
    /// </summary>
    [JsonPropertyName("minTranQua")]
    public decimal MinQuantityStep { get; set; }
    /// <summary>
    /// ["<c>minOrderAmount</c>"] Min order quantity
    /// </summary>
    [JsonPropertyName("minOrderAmount")]
    public decimal MinOrderQuantity { get; set; }
    /// <summary>
    /// ["<c>priceAccuracy</c>"] Price accuracy
    /// </summary>
    [JsonPropertyName("priceAccuracy")]
    public int PriceAccuracy { get; set; }
    /// <summary>
    /// ["<c>quantityAccuracy</c>"] Quantity accuracy
    /// </summary>
    [JsonPropertyName("quantityAccuracy")]
    public int QuantityAccuracy { get; set; }
    /// <summary>
    /// ["<c>symbol</c>"] Symbol
    /// </summary>
    [JsonPropertyName("symbol")]
    public string Symbol { get; set; } = string.Empty;
}

