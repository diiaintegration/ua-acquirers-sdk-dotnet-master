using System.Text.Json.Serialization;

namespace DiiaClient.SDK.Models.Remote
{
    [Serializable]
    internal class DocumentRequest
    {
        [JsonPropertyName("branchId")]
        public string BranchId { get; set; }
        [JsonPropertyName("barcode")]
        public string Barcode { get; set; }
        [JsonPropertyName("qrcode")]
        public string Qrcode { get; set; }
        [JsonPropertyName("requestId")]
        public string RequestId { get; set; }
    }
}
