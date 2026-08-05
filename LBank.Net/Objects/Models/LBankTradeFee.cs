using System.Text.Json.Serialization;

namespace LBank.Net.Objects.Models;

/// <summary>
/// Trade fee
/// </summary>
public record LBankTradeFee
{
    /// <summary>
    /// ["<c>symbol</c>"] Symbol
    /// </summary>
    [JsonPropertyName("symbol")]
    public string Symbol { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>makerCommission</c>"] Maker commission
    /// </summary>
    [JsonPropertyName("makerCommission")]
    public decimal MakerCommission { get; set; }
    /// <summary>
    /// ["<c>takerCommission</c>"] Taker commission
    /// </summary>
    [JsonPropertyName("takerCommission")]
    public decimal TakerCommission { get; set; }
}

