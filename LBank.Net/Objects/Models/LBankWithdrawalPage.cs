using System;
using System.Text.Json.Serialization;
using LBank.Net.Enums;

namespace LBank.Net.Objects.Models;

/// <summary>
/// Withdrawal page
/// </summary>
public record LBankWithdrawalPage: LBankPage
{
    /// <summary>
    /// ["<c>withdraws</c>"] Withdrawal orders
    /// </summary>
    [JsonPropertyName("withdraws")]
    public LBankWithdrawal[] Withdrawals { get; set; } = [];
}

/// <summary>
/// Withdrawal info
/// </summary>
public record LBankWithdrawal
{
    /// <summary>
    /// ["<c>amount</c>"] Quantity
    /// </summary>
    [JsonPropertyName("amount")]
    public decimal Quantity { get; set; }
    /// <summary>
    /// ["<c>coid</c>"] Client order id
    /// </summary>
    [JsonPropertyName("coid")]
    public string? ClientOrderId { get; set; }
    /// <summary>
    /// ["<c>address</c>"] Address
    /// </summary>
    [JsonPropertyName("address")]
    public string Address { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>withdrawOrderId</c>"] Withdraw order id
    /// </summary>
    [JsonPropertyName("withdrawOrderId")]
    public string WithdrawOrderId { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>fee</c>"] Fee
    /// </summary>
    [JsonPropertyName("fee")]
    public decimal Fee { get; set; }
    /// <summary>
    /// ["<c>networkName</c>"] Network name
    /// </summary>
    [JsonPropertyName("networkName")]
    public string Network { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>transferType</c>"] Transfer type
    /// </summary>
    [JsonPropertyName("transferType")]
    public string TransferType { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>txId</c>"] Transaction id
    /// </summary>
    [JsonPropertyName("txId")]
    public string TransactionId { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>feeAssetCode</c>"] Fee asset code
    /// </summary>
    [JsonPropertyName("feeAssetCode")]
    public string FeeAsset { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>id</c>"] Id
    /// </summary>
    [JsonPropertyName("id")]
    public long Id { get; set; }
    /// <summary>
    /// ["<c>applyTime</c>"] Apply time
    /// </summary>
    [JsonPropertyName("applyTime")]
    public DateTime ApplyTime { get; set; }
    /// <summary>
    /// ["<c>status</c>"] Status
    /// </summary>
    [JsonPropertyName("status")]
    public WithdrawStatus Status { get; set; }
}

