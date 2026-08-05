using System.Text.Json.Serialization;

namespace LBank.Net.Objects.Models
{
    /// <summary>
    /// Listen key
    /// </summary>
    internal record LBankListenKey
    {
        /// <summary>
        /// Key
        /// </summary>
        [JsonPropertyName("key")]
        public string Key { get; set; } = string.Empty;
    }
}
