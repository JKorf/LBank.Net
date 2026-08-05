using System.Text.Json.Serialization;

namespace LBank.Net.Objects.Models;

/// <summary>
/// Deposit address
/// </summary>
public record LBankDepositAddress
{
    /// <summary>
    /// ["<c>address</c>"] Address
    /// </summary>
    [JsonPropertyName("address")]
    public string Address { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>memo</c>"] Memo
    /// </summary>
    [JsonPropertyName("memo")]
    public string? Memo { get; set; }
    /// <summary>
    /// ["<c>coin</c>"] Asset
    /// </summary>
    [JsonPropertyName("coin")]
    public string Asset { get; set; } = string.Empty;
}

