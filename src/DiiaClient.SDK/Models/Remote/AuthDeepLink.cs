using System.Text.Json.Serialization;

namespace DiiaClient.SDK.Models.Remote
{
    [Serializable]
    public class AuthDeepLink
    {
        [JsonPropertyName("deep_link")]
        public string DeepLink { get; set; }
        [JsonPropertyName("request_id")]
        public string RequestId { get; set; }
        [JsonPropertyName("request_id_hash")]
        public string RequestIdHash { get; set; }

    }
}
