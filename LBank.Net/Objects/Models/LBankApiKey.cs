using System;
using System.Text.Json.Serialization;

namespace LBank.Net.Objects.Models;

/// <summary>
/// API key info
/// </summary>
public record LBankApiKey
{
    /// <summary>
    /// ["<c>enableSpotTrading</c>"] Enable spot trading
    /// </summary>
    [JsonPropertyName("enableSpotTrading")]
    public bool EnableSpotTrading { get; set; }
    /// <summary>
    /// ["<c>createTime</c>"] Create time
    /// </summary>
    [JsonPropertyName("createTime")]
    public DateTime CreateTime { get; set; }
    /// <summary>
    /// ["<c>enableReading</c>"] Enable reading
    /// </summary>
    [JsonPropertyName("enableReading")]
    public bool EnableReading { get; set; }
    /// <summary>
    /// ["<c>ipRestrict</c>"] Ip restricted
    /// </summary>
    [JsonPropertyName("ipRestrict")]
    public bool IpRestricted { get; set; }
    /// <summary>
    /// ["<c>enableWithdrawals</c>"] Enable withdrawals
    /// </summary>
    [JsonPropertyName("enableWithdrawals")]
    public bool EnableWithdrawals { get; set; }
    /// <summary>
    /// ["<c>enableTransfer</c>"] Enable transfers
    /// </summary>
    [JsonPropertyName("enableTransfer")]
    public bool EnableTransfer { get; set; }
    /// <summary>
    /// ["<c>enableFuturesTrading</c>"] Enable futures trading
    /// </summary>
    [JsonPropertyName("enableFuturesTrading")]
    public bool EnableFuturesTrading { get; set; }
}

