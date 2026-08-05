using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace LBank.Net.Enums;

/// <summary>
/// Withdrawal status
/// </summary>
[JsonConverter(typeof(EnumConverter<WithdrawStatus>))]
public enum WithdrawStatus
{
    /// <summary>
    /// ["<c>1</c>"] In progress
    /// </summary>
    [Map("1")]
    Applying,
    /// <summary>
    /// ["<c>2</c>"] Canceled
    /// </summary>
    [Map("2")]
    Canceled,
    /// <summary>
    /// ["<c>3</c>"] Failed
    /// </summary>
    [Map("3")]
    Failed,
    /// <summary>
    /// ["<c>4</c>"] Completed
    /// </summary>
    [Map("4")]
    Completed,
}
