using System;
using System.Text.Json.Serialization;

namespace LBank.Net.Objects.Internal
{
    internal record LBankResponse
    {
        [JsonPropertyName("msg")]
        public string? Message { get; set; }
        [JsonPropertyName("result")]
        public bool Result { get; set; }
        [JsonPropertyName("error_code")]
        public int ErrorCode { get; set; }
        [JsonPropertyName("ts")]
        public DateTime Timestamp { get; set; }
    }

    internal record LBankResponse<T> : LBankResponse
    {
        [JsonPropertyName("data")]
        public T? Data { get; set; }
    }
}
