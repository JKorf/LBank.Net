using System.Text.Json.Serialization;

namespace LBank.Net.Objects.Models;

/// <summary>
/// Order book snapshot update
/// </summary>
public record LBankOrderBookUpdate
{
    /// <summary>
    /// ["<c>asks</c>"] Asks
    /// </summary>
    [JsonPropertyName("asks")]
    public LBankOrderBookEntry[] Asks { get; set; } = [];
    /// <summary>
    /// ["<c>bids</c>"] Bids
    /// </summary>
    [JsonPropertyName("bids")]
    public LBankOrderBookEntry[] Bids { get; set; } = [];
}
