using System.Text.Json.Serialization;

namespace DiiaClient.SDK.Models.Remote
{
    [Serializable]
    public class SignaturePackage
    {
        [JsonPropertyName("request_id")]
        public string RequestId { get; set; }
        [JsonPropertyName("diia_id_action")]
        public string DiiaIdAction { get; set; }
        [JsonPropertyName("signedItems")]
        public List<Signatures> Signatures { get; set; }
    }

    [Serializable]
    public class Signatures
    {
        [JsonPropertyName("name")]
        public string Filename { get; set; }
        [JsonPropertyName("signature")]
        public string Signature { get; set; }

        //for auth
        [JsonPropertyName("requestId")]
        public string RequestId { get; set; }
    }
}
