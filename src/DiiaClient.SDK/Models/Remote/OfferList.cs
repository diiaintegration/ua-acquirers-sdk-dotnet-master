using System.Text.Json.Serialization;

namespace DiiaClient.SDK.Models.Remote
{
    public class OfferList
    {
        [JsonPropertyName("total")]
        public long Total { get; set; }
        [JsonPropertyName("offers")]
        public List<Offer> Offers { get; set; }
    }
}
