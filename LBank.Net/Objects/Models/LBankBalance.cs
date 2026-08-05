using System.Text.Json.Serialization;
using LBank.Net.Enums;

namespace LBank.Net.Objects.Models;

/// <summary>
/// User balance
/// </summary>
public record LBankBalance
{
    /// <summary>
    /// ["<c>usableAmt</c>"] Usable quantity
    /// </summary>
    [JsonPropertyName("usableAmt")]
    public decimal UsableQuantity { get; set; }
    /// <summary>
    /// ["<c>assetAmt</c>"] Quantity
    /// </summary>
    [JsonPropertyName("assetAmt")]
    public decimal Quantity { get; set; }
    /// <summary>
    /// ["<c>networkList</c>"] Networks
    /// </summary>
    [JsonPropertyName("networkList")]
    public LBankBalanceNetwork[] Networks { get; set; } = [];
    /// <summary>
    /// ["<c>freezeAmt</c>"] Frozen quantity
    /// </summary>
    [JsonPropertyName("freezeAmt")]
    public decimal FrozenQuantity { get; set; }
    /// <summary>
    /// ["<c>coin</c>"] Asset
    /// </summary>
    [JsonPropertyName("coin")]
    public string Asset { get; set; } = string.Empty;
}

/// <summary>
/// Network info
/// </summary>
public record LBankBalanceNetwork
{
    /// <summary>
    /// ["<c>isDefault</c>"] Is default
    /// </summary>
    [JsonPropertyName("isDefault")]
    public bool IsDefault { get; set; }
    /// <summary>
    /// ["<c>withdrawFeeRate</c>"] Withdraw fee rate
    /// </summary>
    [JsonPropertyName("withdrawFeeRate")]
    public decimal WithdrawFeeRate { get; set; }
    /// <summary>
    /// ["<c>name</c>"] Name
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>withdrawMin</c>"] Min withdraw quantity
    /// </summary>
    [JsonPropertyName("withdrawMin")]
    public decimal MinWithdraw { get; set; }
    /// <summary>
    /// ["<c>minLimit</c>"] Min transfer quantity
    /// </summary>
    [JsonPropertyName("minLimit")]
    public decimal MinTransfer { get; set; }
    /// <summary>
    /// ["<c>minDeposit</c>"] Min deposit quantity
    /// </summary>
    [JsonPropertyName("minDeposit")]
    public decimal MinDeposit { get; set; }
    /// <summary>
    /// ["<c>feeAssetCode</c>"] Fee asset
    /// </summary>
    [JsonPropertyName("feeAssetCode")]
    public string FeeAsset { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>withdrawFee</c>"] Withdraw fee
    /// </summary>
    [JsonPropertyName("withdrawFee")]
    public decimal? WithdrawFee { get; set; }
    /// <summary>
    /// ["<c>type</c>"] Type
    /// </summary>
    [JsonPropertyName("type")]
    public FeeType Type { get; set; }
    /// <summary>
    /// ["<c>coin</c>"] Asset
    /// </summary>
    [JsonPropertyName("coin")]
    public string Asset { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>network</c>"] Network code
    /// </summary>
    [JsonPropertyName("network")]
    public string Network { get; set; } = string.Empty;
}

