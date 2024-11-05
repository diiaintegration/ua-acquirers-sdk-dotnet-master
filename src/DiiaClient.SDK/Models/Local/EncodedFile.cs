using System.Text.Json.Serialization;

namespace DiiaClient.SDK.Models.Local
{
    [Serializable]
    public class EncodedFile
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("data")]
        public string Data { get; set; }
    }
}
