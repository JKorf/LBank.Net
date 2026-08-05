using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace LBank.Net.Enums;

/// <summary>
/// Kline interval
/// </summary>
[JsonConverter(typeof(EnumConverter<KlineInterval>))]
public enum KlineInterval
{
    /// <summary>
    /// ["<c>minute1</c>"] One minute
    /// </summary>
    [Map("minute1")]
    OneMinute = 60,
    /// <summary>
    /// ["<c>minute5</c>"] Five minutes
    /// </summary>
    [Map("minute5")]
    FiveMinutes = 60 * 5,
    /// <summary>
    /// ["<c>minute15</c>"] Fifteen minutes
    /// </summary>
    [Map("minute15")]
    FifteenMinutes = 60 * 15,
    /// <summary>
    /// ["<c>minute30</c>"] Thirty minutes
    /// </summary>
    [Map("minute30")]
    ThirtyMinutes = 60 * 30,
    /// <summary>
    /// ["<c>hour1</c>"] One hour
    /// </summary>
    [Map("hour1")]
    OneHour = 60 * 60,
    /// <summary>
    /// ["<c>hour4</c>"] Four hours
    /// </summary>
    [Map("hour4")]
    FourHours = 60 * 60 * 4,
    /// <summary>
    /// ["<c>hour8</c>"] Eight hours
    /// </summary>
    [Map("hour8")]
    EightHours = 60 * 60 * 8,
    /// <summary>
    /// ["<c>hour12</c>"] Twelve hours
    /// </summary>
    [Map("hour12")]
    TwelveHours = 60 * 60 * 12,
    /// <summary>
    /// ["<c>day1</c>"] One day
    /// </summary>
    [Map("day1")]
    OneDay = 60 * 60 * 24,
    /// <summary>
    /// ["<c>week1</c>"] One week
    /// </summary>
    [Map("week1")]
    OneWeek = 60 * 60 * 24 * 7,
    /// <summary>
    /// ["<c>month1</c>"] One month
    /// </summary>
    [Map("month1")]
    OneMonth = 60 * 60 * 24 * 30,
}
