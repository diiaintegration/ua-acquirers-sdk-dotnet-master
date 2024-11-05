using System.Text.Json.Serialization;

namespace DiiaClient.SDK.Models.Remote
{
    [Serializable]
    public class File
    {
        [JsonPropertyName("filename")]
        public string FileName { get; set; }
        [JsonPropertyName("data")]
        public byte[] Data { get; set; }
    }
}
