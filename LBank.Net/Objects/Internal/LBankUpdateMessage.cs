using LBank.Net.Objects.Models;
using System;
using System.Text.Json.Serialization;

namespace LBank.Net.Objects.Internal
{
    internal class LBankUpdateMessage
    {
        [JsonPropertyName("TS")]
        public DateTime Timestamp { get; set; }
        public DateTime TimestampUtc => Timestamp.AddHours(-8);
        [JsonPropertyName("type")]
        public string Topic { get; set; } = string.Empty;
        [JsonPropertyName("pair")]
        public string Symbol { get; set; } = string.Empty;
    }

    internal class LBankTradeUpdateMessage : LBankUpdateMessage
    {
        [JsonPropertyName("trade")]
        public LBankTradeUpdate Trade { get; set; } = default!;
    }

    internal class LBankKlineUpdateMessage : LBankUpdateMessage
    {
        [JsonPropertyName("kbar")]
        public LBankKlineUpdate Kline { get; set; } = default!;
    }

    internal class LBankOrderBookUpdateMessage : LBankUpdateMessage
    {
        [JsonPropertyName("depth")]
        public LBankOrderBookUpdate OrderBook { get; set; } = default!;
        [JsonPropertyName("count")]
        public int Count { get; set; }
    }

    internal class LBankTickerUpdateMessage : LBankUpdateMessage
    {
        [JsonPropertyName("tick")]
        public LBankTickerUpdate Ticker { get; set; } = default!;
    }

    internal class LBankOrderUpdateMessage : LBankUpdateMessage
    {
        [JsonPropertyName("orderUpdate")]
        public LBankOrderUpdate Order { get; set; } = default!;
    }

    internal class LBankBalanceUpdateMessage : LBankUpdateMessage
    {
        [JsonPropertyName("data")]
        public LBankBalanceUpdate Balance { get; set; } = default!;
    }
}
