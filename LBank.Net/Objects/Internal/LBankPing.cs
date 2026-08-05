using System.Text.Json.Serialization;

namespace LBank.Net.Objects.Internal
{
    internal class LBankPing
    {
        [JsonPropertyName("action")]
        public string Action { get; set; } = string.Empty;
        [JsonPropertyName("ping")]
        public string Ping { get; set; } = string.Empty;
    }
}
