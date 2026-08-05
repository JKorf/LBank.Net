using LBank.Net.Enums;
using System;
using System.Text.Json.Serialization;

namespace LBank.Net.Objects.Models
{
    /// <summary>
    /// Kline/candlestick update
    /// </summary>
    public record LBankKlineUpdate
    {
        /// <summary>
        /// ["<c>a</c>"] Turnover
        /// </summary>
        [JsonPropertyName("a")]
        public decimal Turnover { get; set; }
        /// <summary>
        /// ["<c>c</c>"] Close price
        /// </summary>
        [JsonPropertyName("c")]
        public decimal ClosePrice { get; set; }
        /// <summary>
        /// ["<c>t</c>"] Open timestamp in UTC+8
        /// </summary>
        [JsonPropertyName("t")]
        public DateTime OpenTime { get; set; }
        /// <summary>
        /// Open timestamp UTC
        /// </summary>
        public DateTime OpenTimeUtc => Interval >= StreamKlineInterval.OneDay ? OpenTime : OpenTime.AddHours(-8);
        /// <summary>
        /// ["<c>v</c>"] Volume
        /// </summary>
        [JsonPropertyName("v")]
        public decimal Volume { get; set; }
        /// <summary>
        /// ["<c>h</c>"] High price
        /// </summary>
        [JsonPropertyName("h")]
        public decimal HighPrice { get; set; }
        /// <summary>
        /// ["<c>l</c>"] Low price
        /// </summary>
        [JsonPropertyName("l")]
        public decimal LowPrice { get; set; }
        /// <summary>
        /// ["<c>o</c>"] Open price
        /// </summary>
        [JsonPropertyName("o")]
        public decimal OpenPrice { get; set; }
        /// <summary>
        /// ["<c>n</c>"] Number of trades
        /// </summary>
        [JsonPropertyName("n")]
        public int TradeCount { get; set; }
        /// <summary>
        /// ["<c>slot</c>"] Interval
        /// </summary>
        [JsonPropertyName("slot")]
        public StreamKlineInterval Interval { get; set; }
    }
}
