using DiiaClient.SDK.Models.Local;
using System.Text.Json.Serialization;

namespace DiiaClient.SDK.Models.Remote
{
    [Serializable]
    public class DocumentPackage
    {
        [JsonPropertyName("requestId")]
        public string RequestId { get; set; }
        [JsonPropertyName("decodedFiles")]
        public List<DecodedFile> DecodedFiles { get; set; }
        [JsonPropertyName("data")]
        public Metadata Data { get; set; }
    }
}
