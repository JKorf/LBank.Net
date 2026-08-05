using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace LBank.Net.Enums;

/// <summary>
/// Deposit status
/// </summary>
[JsonConverter(typeof(EnumConverter<DepositStatus>))]
public enum DepositStatus
{
    /// <summary>
    /// ["<c>1</c>"] Applying
    /// </summary>
    [Map("1")]
    Applying,
    /// <summary>
    /// ["<c>2</c>"] Deposit successful
    /// </summary>
    [Map("2")]
    Success,
    /// <summary>
    /// ["<c>3</c>"] Recharge failed
    /// </summary>
    [Map("3")]
    Failed,
    /// <summary>
    /// ["<c>4</c>"] Canceled
    /// </summary>
    [Map("4")]
    Canceled,
    /// <summary>
    /// ["<c>5</c>"] Transfer
    /// </summary>
    [Map("5")]
    Transfer,
}
