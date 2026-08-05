using LBank.Net.Enums;
using System.Text.Json.Serialization;

namespace LBank.Net.Objects.Models;

/// <summary>
/// Asset info
/// </summary>
public record LBankAsset
{
    /// <summary>
    /// ["<c>assetCode</c>"] Asset code
    /// </summary>
    [JsonPropertyName("assetCode")]
    public string Asset { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>chainName</c>"] Network name
    /// </summary>
    [JsonPropertyName("chainName")]
    public string Network { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>canDraw</c>"] Can withdraw
    /// </summary>
    [JsonPropertyName("canDraw")]
    public bool CanWithdraw { get; set; }
    /// <summary>
    /// ["<c>canStationDraw</c>"] Can transfer
    /// </summary>
    [JsonPropertyName("canStationDraw")]
    public bool CanTransfer { get; set; }
    /// <summary>
    /// ["<c>canDeposit</c>"] Can deposit
    /// </summary>
    [JsonPropertyName("canDeposit")]
    public bool CanDeposit { get; set; }
    /// <summary>
    /// ["<c>hasMemo</c>"] Has memo
    /// </summary>
    [JsonPropertyName("hasMemo")]
    public bool HasMemo { get; set; }
    /// <summary>
    /// ["<c>contractInfo</c>"] Contract address
    /// </summary>
    [JsonPropertyName("contractInfo")]
    public string? ContractAddress { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>assetFee</c>"] Asset fee
    /// </summary>
    [JsonPropertyName("assetFee")]
    public LBankAssetFee AssetFee { get; set; } = null!;
}

/// <summary>
/// Fee info
/// </summary>
public record LBankAssetFee
{
    /// <summary>
    /// ["<c>type</c>"] Type
    /// </summary>
    [JsonPropertyName("type")]
    public FeeType Type { get; set; }
    /// <summary>
    /// ["<c>feeCode</c>"] Fee code
    /// </summary>
    [JsonPropertyName("feeCode")]
    public string FeeCode { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>scale</c>"] Withdrawal scale
    /// </summary>
    [JsonPropertyName("scale")]
    public int WithdrawScale { get; set; }
    /// <summary>
    /// ["<c>minAmt</c>"] Minimal withdraw quantity
    /// </summary>
    [JsonPropertyName("minAmt")]
    public decimal MinWithdrawQuantity { get; set; }
    /// <summary>
    /// ["<c>feeAmt</c>"] Fee quantity
    /// </summary>
    [JsonPropertyName("feeAmt")]
    public decimal? FeeQuantity { get; set; }
    /// <summary>
    /// ["<c>feeRate</c>"] Fee rate
    /// </summary>
    [JsonPropertyName("feeRate")]
    public decimal FeeRate { get; set; }
    /// <summary>
    /// ["<c>stationFeeAmt</c>"] Transfer fee
    /// </summary>
    [JsonPropertyName("stationFeeAmt")]
    public decimal TransferFeeQuantity { get; set; }
    /// <summary>
    /// ["<c>stationScale</c>"] Transfer scale
    /// </summary>
    [JsonPropertyName("stationScale")]
    public int TransferScale { get; set; }
    /// <summary>
    /// ["<c>stationMinAmt</c>"] Minimal transfer quantity
    /// </summary>
    [JsonPropertyName("stationMinAmt")]
    public decimal MinTransferQuantity { get; set; }
    /// <summary>
    /// ["<c>minDepositAmt</c>"] Min deposit quantity
    /// </summary>
    [JsonPropertyName("minDepositAmt")]
    public decimal MinDepositQuantity { get; set; }
    /// <summary>
    /// ["<c>depositFee</c>"] Deposit fee
    /// </summary>
    [JsonPropertyName("depositFee")]
    public decimal DepositFee { get; set; }
}

