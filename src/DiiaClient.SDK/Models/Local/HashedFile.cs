using System.Text.Json.Serialization;

namespace DiiaClient.SDK.Models.Local
{
    [Serializable]
    public class HashedFile
    {
        [JsonPropertyName("fileName")]
        public string FileName { get; set; }
        [JsonPropertyName("fileHash")]
        public string FileHash { get; set; }
    }
}
