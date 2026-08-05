using LBank.Net.Enums;
using System.Text.Json.Serialization;

namespace LBank.Net.Objects.Internal
{
    internal class LBankSubscribeMessage
    {
        [JsonPropertyName("action")]
        public string Action { get; set; } = string.Empty;
        [JsonPropertyName("subscribe")]
        public string Topic { get; set; } = string.Empty;
        [JsonPropertyName("pair")]
        public string Symbol { get; set; } = string.Empty;

        [JsonPropertyName("kbar"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public StreamKlineInterval? Interval { get; set; }

        [JsonPropertyName("depth"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int? Depth { get; set; }
        [JsonPropertyName("subscribeKey"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? SubscribeKey { get; set; }
    }
}
