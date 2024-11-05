using System.Text.Json.Serialization;

namespace DiiaClient.SDK.Models.Remote
{
    [Serializable]
    public class BranchScopes
    {
        [JsonPropertyName("sharing")]
        public List<string> Sharing { get; set; }

        [JsonPropertyName("documentIdentification")]
        public List<string> DocumentIdentification { get; set; }

        [JsonPropertyName("diiaId")]
        public List<string> DiiaId { get; set; }
    }
}
