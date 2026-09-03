using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace LBank.Net.Enums;

/// <summary>
/// Order side
/// </summary>
[JsonConverter(typeof(EnumConverter<OrderSide>))]
public enum OrderSide
{
    /// <summary>
    /// ["<c>buy</c>", "<c>buy_market</c>", "<c>buy_maker</c>", "<c>buy_ioc</c>", "<c>buy_fok</c>"] Buy
    /// </summary>
    [Map("buy", "buy_market", "buy_maker", "buy_ioc", "buy_fok")]
    Buy,
    /// <summary>
    /// ["<c>sell</c>", "<c>sell_market</c>", "<c>sell_maker</c>", "<c>sell_ioc</c>", "<c>sell_fok</c>"] Sell
    /// </summary>
    [Map("sell", "sell_market", "sell_maker", "sell_ioc", "sell_fok")]
    Sell,
}
