using System.Text.Json.Serialization;

namespace DiiaClient.SDK.Models.Remote
{
    [Serializable]
    internal class Id
    {
        [JsonPropertyName("_id")]
        public string ID { get; set; }
    }
}
