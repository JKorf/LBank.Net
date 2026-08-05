using System.Text.Json.Serialization;

namespace LBank.Net.Objects.Models;

/// <summary>
/// Last price
/// </summary>
public record LBankPrice
{
    /// <summary>
    /// ["<c>symbol</c>"] Symbol
    /// </summary>
    [JsonPropertyName("symbol")]
    public string Symbol { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>price</c>"] Price
    /// </summary>
    [JsonPropertyName("price")]
    public decimal Price { get; set; }
}

