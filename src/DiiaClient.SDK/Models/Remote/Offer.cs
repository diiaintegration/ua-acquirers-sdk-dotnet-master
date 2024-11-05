using System.Text.Json.Serialization;

namespace DiiaClient.SDK.Models.Remote
{
    [Serializable]
    public class Offer
    {
        [JsonPropertyName("_id")]
        public string? Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("returnLink")]
        public string ReturnLink { get; set; }
        [JsonPropertyName("scopes")]
        public OfferScopes Scopes { get; set; }
    }
}
