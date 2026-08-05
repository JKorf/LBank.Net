using System.Text.Json.Serialization;

namespace LBank.Net.Objects.Internal
{
    internal class LBankPong
    {
        [JsonPropertyName("action")]
        public string Action { get; set; } = string.Empty;
        [JsonPropertyName("pong")]
        public string Ping { get; set; } = string.Empty;
    }
}
