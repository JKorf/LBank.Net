using System.Text.Json.Serialization;

namespace LBank.Net.Objects.Models;

/// <summary>
/// Account info
/// </summary>
public record LBankAccountInfo
{
    /// <summary>
    /// ["<c>uid</c>"] Uid
    /// </summary>
    [JsonPropertyName("uid")]
    public string UserId { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>balances</c>"] Balances
    /// </summary>
    [JsonPropertyName("balances")]
    public LBankAccountInfoBalance[] Balances { get; set; } = [];
    /// <summary>
    /// ["<c>canWithdraw</c>"] Can withdraw
    /// </summary>
    [JsonPropertyName("canWithdraw")]
    public bool CanWithdraw { get; set; }
    /// <summary>
    /// ["<c>canDeposit</c>"] Can deposit
    /// </summary>
    [JsonPropertyName("canDeposit")]
    public bool CanDeposit { get; set; }
    /// <summary>
    /// ["<c>canTrade</c>"] Can trade
    /// </summary>
    [JsonPropertyName("canTrade")]
    public bool CanTrade { get; set; }
}

/// <summary>
/// Balance info
/// </summary>
public record LBankAccountInfoBalance
{
    /// <summary>
    /// ["<c>asset</c>"] Asset
    /// </summary>
    [JsonPropertyName("asset")]
    public string Asset { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>free</c>"] Free
    /// </summary>
    [JsonPropertyName("free")]
    public decimal Free { get; set; }
    /// <summary>
    /// ["<c>locked</c>"] Locked
    /// </summary>
    [JsonPropertyName("locked")]
    public decimal Locked { get; set; }
}

