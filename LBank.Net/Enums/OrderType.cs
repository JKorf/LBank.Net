using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace LBank.Net.Enums;

/// <summary>
/// Order type
/// </summary>
[JsonConverter(typeof(EnumConverter<OrderType>))]
public enum OrderType
{
    /// <summary>
    /// ["<c>buy</c>"] Buy limit order
    /// </summary>
    [Map("buy")]
    BuyLimit,
    /// <summary>
    /// ["<c>sell</c>"] Sell limit order
    /// </summary>
    [Map("sell")]
    SellLimit,
    /// <summary>
    /// ["<c>buy_market</c>"] Buy market order
    /// </summary>
    [Map("buy_market")]
    BuyMarket,
    /// <summary>
    /// ["<c>sell_market</c>"] Sell market order
    /// </summary>
    [Map("sell_market")]
    SellMarket,
    /// <summary>
    /// ["<c>buy_maker</c>"] Buy post-only order
    /// </summary>
    [Map("buy_maker")]
    BuyMaker,
    /// <summary>
    /// ["<c>sell_maker</c>"] Sell post-only order
    /// </summary>
    [Map("sell_maker")]
    SellMaker,
    /// <summary>
    /// ["<c>buy_ioc</c>"] Buy immediate or cancel order
    /// </summary>
    [Map("buy_ioc")]
    BuyIoc,
    /// <summary>
    /// ["<c>sell_ioc</c>"] Sell immediate or cancel order
    /// </summary>
    [Map("sell_ioc")]
    SellIoc,
    /// <summary>
    /// ["<c>buy_fok</c>"] Buy fill or kill order
    /// </summary>
    [Map("buy_fok")]
    BuyFok,
    /// <summary>
    /// ["<c>sell_fok</c>"] Sell fill or kill order
    /// </summary>
    [Map("sell_fok")]
    SellFok,
}
