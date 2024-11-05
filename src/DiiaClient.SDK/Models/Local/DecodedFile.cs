using System.Text.Json.Serialization;

namespace DiiaClient.SDK.Models.Local
{
    [Serializable]
    public class DecodedFile
    {
        [JsonPropertyName("fileName")]
        public string FileName { get; set; }
        [JsonPropertyName("data")]
        public byte[] Data { get; set; }
    }
}
