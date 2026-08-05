using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace LBank.Net.Enums;

/// <summary>
/// Order status
/// </summary>
[JsonConverter(typeof(EnumConverter<OrderStatus>))]
public enum OrderStatus
{
    /// <summary>
    /// ["<c>-1</c>"] Canceled
    /// </summary>
    [Map("-1")]
    Canceled,
    /// <summary>
    /// ["<c>0</c>"] Open
    /// </summary>
    [Map("0")]
    Open,
    /// <summary>
    /// ["<c>1</c>"] Partially filled
    /// </summary>
    [Map("1")]
    PartiallyFilled,
    /// <summary>
    /// ["<c>2</c>"] Filled
    /// </summary>
    [Map("2")]
    Filled,
    /// <summary>
    /// ["<c>3</c>"] Partially canceled
    /// </summary>
    [Map("3")]
    PartiallyCanceled,
    /// <summary>
    /// ["<c>4</c>"] Cancelling
    /// </summary>
    [Map("4")]
    Cancelling,
}
