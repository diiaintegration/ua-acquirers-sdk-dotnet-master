using System.Text.Json.Serialization;

namespace DiiaClient.SDK.Models.Remote
{
    [Serializable]
    public class OfferScopes
    {
        [JsonPropertyName("sharing")]
        public List<string>? Sharing { get; set; }

        [JsonPropertyName("diiaId")]
        public List<string>? DiiaId { get; set; }
    }
}
