using System;
using System.Text.Json.Serialization;

namespace LBank.Net.Objects.Internal
{
    internal class LBankError
    {
        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;
        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;
        [JsonPropertyName("TS")]
        public DateTime Timestamp { get; set; }
    }
}
