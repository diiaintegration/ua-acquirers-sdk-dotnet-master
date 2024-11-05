using System.Text.Json.Serialization;

namespace DiiaClient.SDK.Models.Remote
{
    [Serializable]
    internal class SessionToken
    {
        [JsonPropertyName("token")]
        public string Token { get; set; }
    }
}
