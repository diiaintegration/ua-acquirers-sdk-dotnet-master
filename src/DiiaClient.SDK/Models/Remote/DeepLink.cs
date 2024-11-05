using System.Text.Json.Serialization;

namespace DiiaClient.SDK.Models.Remote
{
    [Serializable]
    internal class DeepLink
    {
        [JsonPropertyName("deeplink")]
        public string Deeplink { get; set; }
    }
}
