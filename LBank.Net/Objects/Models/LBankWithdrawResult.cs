using System.Text.Json.Serialization;

namespace LBank.Net.Objects.Models;

/// <summary>
/// Withdraw result
/// </summary>
public record LBankWithdrawResult
{
    /// <summary>
    /// ["<c>fee</c>"] Fee
    /// </summary>
    [JsonPropertyName("fee")]
    public decimal Fee { get; set; }
    /// <summary>
    /// ["<c>withdrawId</c>"] Withdraw id
    /// </summary>
    [JsonPropertyName("withdrawId")]
    public long WithdrawId { get; set; }
}

