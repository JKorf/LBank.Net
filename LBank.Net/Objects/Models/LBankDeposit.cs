using System;
using System.Text.Json.Serialization;
using LBank.Net.Enums;

namespace LBank.Net.Objects.Models;

/// <summary>
/// Deposit page
/// </summary>
public record LBankDepositPage : LBankPage
{
    /// <summary>
    /// ["<c>depositOrders</c>"] Deposits
    /// </summary>
    [JsonPropertyName("depositOrders")]
    public LBankDeposit[] Deposits { get; set; } = [];
}

/// <summary>
/// Deposit info
/// </summary>
public record LBankDeposit
{
    /// <summary>
    /// ["<c>insertTime</c>"] Insert time
    /// </summary>
    [JsonPropertyName("insertTime")]
    public DateTime InsertTime { get; set; }
    /// <summary>
    /// ["<c>amount</c>"] Quantity
    /// </summary>
    [JsonPropertyName("amount")]
    public decimal Quantity { get; set; }
    /// <summary>
    /// ["<c>address</c>"] Address
    /// </summary>
    [JsonPropertyName("address")]
    public string Address { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>networkName</c>"] Network name
    /// </summary>
    [JsonPropertyName("networkName")]
    public string Network { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>txId</c>"] Transaction id
    /// </summary>
    [JsonPropertyName("txId")]
    public string TransactionId { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>coin</c>"] Asset
    /// </summary>
    [JsonPropertyName("coin")]
    public string Asset { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>status</c>"] Status
    /// </summary>
    [JsonPropertyName("status")]
    public DepositStatus Status { get; set; }
}

