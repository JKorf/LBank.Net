using System.Text.Json.Serialization;

namespace LBank.Net.Objects.Models
{
    /// <summary>
    /// Page
    /// </summary>
    public record LBankPage
    {
        /// <summary>
        /// ["<c>total</c>"] Total results
        /// </summary>
        [JsonPropertyName("total")]
        public int Total { get; set; }
        /// <summary>
        /// ["<c>page_size</c>"] Page size
        /// </summary>
        [JsonPropertyName("page_length")]
        public int PageSize { get; set; }
        /// <summary>
        /// ["<c>current_page</c>"] Current page
        /// </summary>
        [JsonPropertyName("current_page")]
        public int CurrentPage { get; set; }
    }
}
