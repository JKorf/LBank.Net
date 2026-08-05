using System;
using System.Text.Json.Serialization;

namespace LBank.Net.Objects.Models;

/// <summary>
/// User balance update
/// </summary>
public record LBankBalanceUpdate
{
    /// <summary>
    /// ["<c>assetCode</c>"] Asset
    /// </summary>
    [JsonPropertyName("assetCode")]
    public string Asset { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>asset</c>"] Total quantity
    /// </summary>
    [JsonPropertyName("asset")]
    public decimal Total { get; set; }
    /// <summary>
    /// ["<c>free</c>"] Free quantity
    /// </summary>
    [JsonPropertyName("free")]
    public decimal Free { get; set; }
    /// <summary>
    /// ["<c>freeze</c>"] Frozen quantity
    /// </summary>
    [JsonPropertyName("freeze")]
    public decimal Frozen { get; set; }
    /// <summary>
    /// ["<c>type</c>"] Update type
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>time</c>"] Timestamp
    /// </summary>
    [JsonPropertyName("time")]
    public DateTime Timestamp { get; set; }
}


