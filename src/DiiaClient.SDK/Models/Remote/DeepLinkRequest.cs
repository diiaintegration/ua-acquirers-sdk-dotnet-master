using System.Text.Json.Serialization;
using DiiaClient.SDK.Models.Local;

namespace DiiaClient.SDK.Models.Remote
{
    [Serializable]
    internal class DeepLinkRequest
    {
        [JsonPropertyName("offerId")]
        public string OfferId { get; set; }

        [JsonPropertyName("requestId")]
        public string RequestId { get; set; }
        
        [JsonPropertyName("returnLink")]
        public string ReturnLink { get; set; }
        
        [JsonPropertyName("data")]
        public HashedFilesSigning Data { get; set; }
    }

    [Serializable]
    internal class HashedFilesSigning
    {
        [JsonPropertyName("hashedFilesSigning")]
        public HashedFiles HashedFilesSign { get; set; }
    }

    [Serializable]
    internal class HashedFiles
    {
        [JsonPropertyName("hashedFiles")]
        public List<HashedFile> Hashs { get; set; }
    }
}
