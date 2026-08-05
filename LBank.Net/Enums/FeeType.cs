using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace LBank.Net.Enums;

/// <summary>
/// Fee type
/// </summary>
[JsonConverter(typeof(EnumConverter<FeeType>))]
public enum FeeType
{
    /// <summary>
    /// ["<c>1</c>"] Fixed fee
    /// </summary>
    [Map("1")]
    FixedFee,
    /// <summary>
    /// ["<c>2</c>"] Rate fee
    /// </summary>
    [Map("2")]
    RateFee,
    /// <summary>
    /// ["<c>3</c>"] Both fixed and rate fee
    /// </summary>
    [Map("3")]
    FixedAndRateFee,
}
