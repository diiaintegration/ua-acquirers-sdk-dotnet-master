using System.Text.Json.Serialization;

namespace DiiaClient.SDK.Models.Remote
{
    [Serializable]
    public class BranchList
    {
        [JsonPropertyName("total")]
        public long Total { get; set; }
        [JsonPropertyName("branches")]
        public List<Branch> Branches { get; set; }
    }
}
