using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace LBank.Net.Enums;

/// <summary>
/// Stream kline interval
/// </summary>
[JsonConverter(typeof(EnumConverter<StreamKlineInterval>))]
public enum StreamKlineInterval
{
    /// <summary>
    /// ["<c>1min</c>"] One minute
    /// </summary>
    [Map("1min")]
    OneMinute = 60,
    /// <summary>
    /// ["<c>5min</c>"] Five minutes
    /// </summary>
    [Map("5min")]
    FiveMinutes = 60 * 5,
    /// <summary>
    /// ["<c>15min</c>"] Fifteen minutes
    /// </summary>
    [Map("15min")]
    FifteenMinutes = 60 * 15,
    /// <summary>
    /// ["<c>30min</c>"] Thirty minutes
    /// </summary>
    [Map("30min")]
    ThirtyMinutes = 60 * 30,
    /// <summary>
    /// ["<c>1hr</c>"] One hour
    /// </summary>
    [Map("1hr")]
    OneHour = 60 * 60,
    /// <summary>
    /// ["<c>4hr</c>"] Four hours
    /// </summary>
    [Map("4hr")]
    FourHours = 60 * 60 * 4,
    /// <summary>
    /// ["<c>12hr</c>"] Twelve hours
    /// </summary>
    [Map("12hr")]
    TwelveHours = 60 * 60 * 12,
    /// <summary>
    /// ["<c>day</c>"] One day
    /// </summary>
    [Map("day")]
    OneDay = 60 * 60 * 24,
    /// <summary>
    /// ["<c>week</c>"] One week
    /// </summary>
    [Map("week")]
    OneWeek = 60 * 60 * 24 * 7,
    /// <summary>
    /// ["<c>month</c>"] One month
    /// </summary>
    [Map("month")]
    OneMonth = 60 * 60 * 24 * 30,
}
