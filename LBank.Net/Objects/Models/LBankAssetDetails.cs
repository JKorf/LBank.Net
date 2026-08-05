using System.Text.Json.Serialization;

namespace LBank.Net.Objects.Models;

/// <summary>
/// Asset details
/// </summary>
public record LBankAssetDetails
{
    /// <summary>
    /// ["<c>minWithdrawAmount</c>"] Min withdraw quantity
    /// </summary>
    [JsonPropertyName("minWithdrawAmount")]
    public decimal MinWithdrawQuantity { get; set; }
    /// <summary>
    /// ["<c>stationDrawStatus</c>"] Transfer enabled
    /// </summary>
    [JsonPropertyName("stationDrawStatus")]
    public bool TransferEnabled { get; set; }
    /// <summary>
    /// ["<c>depositStatus</c>"] Deposit enabled
    /// </summary>
    [JsonPropertyName("depositStatus")]
    public bool DepositEnabled { get; set; }
    /// <summary>
    /// ["<c>withdrawFee</c>"] Withdraw fee
    /// </summary>
    [JsonPropertyName("withdrawFee")]
    public decimal WithdrawFee { get; set; }
    /// <summary>
    /// ["<c>withdrawStatus</c>"] Withdraw enabled
    /// </summary>
    [JsonPropertyName("withdrawStatus")]
    public bool WithdrawEnabled { get; set; }
}
