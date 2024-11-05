using System.Text.Json.Serialization;

namespace DiiaClient.SDK.Models.Remote
{
    [Serializable]
    public class Branch
    {
        [JsonPropertyName("_id")]
        public string Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("region")]
        public string Region { get; set; }
        [JsonPropertyName("district")]
        public string District { get; set; }
        [JsonPropertyName("location")]
        public string Location { get; set; }
        [JsonPropertyName("street")]
        public string Street { get; set; }
        [JsonPropertyName("house")]
        public string House { get; set; }
        [JsonPropertyName("customFullName")]
        public string CustomFullName { get; set; }
        [JsonPropertyName("customFullAddress")]
        public string CustomFullAddress { get; set; }
        [JsonPropertyName("deliveryTypes")]
        public List<string> DeliveryTypes { get; set; }
        [JsonPropertyName("offerRequestType")]
        public string OfferRequestType { get; set; }
        [JsonPropertyName("scopes")]
        public BranchScopes Scopes { get; set; }
    }
}
