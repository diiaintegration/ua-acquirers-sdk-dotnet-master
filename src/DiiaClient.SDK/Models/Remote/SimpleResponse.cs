using System.Text.Json.Serialization;

namespace DiiaClient.SDK.Models.Remote
{
    [Serializable]
    internal class SimpleResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        // error details in case success==false
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("message")]
        public string Message { get; set; }
        [JsonPropertyName("code")]
        public int Code { get; set; }
        [JsonPropertyName("data")]
        public DataError Data { get; set; }
    }
    internal class DataError
    {
        [JsonPropertyName("errors")]
        public Error Errors { get; set; }
        [JsonPropertyName("now")]
        public string Now { get; set; }
        [JsonPropertyName("expirationDate")]
        public string ExpirationDate { get; set; }
        [JsonPropertyName("data")]
        public dynamic Data { get; set; }
    }
    internal class Error
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("message")]
        public string Message { get; set; }
        [JsonPropertyName("field")]
        public string Field { get; set; }
        [JsonPropertyName("expected")]
        public dynamic Expected { get; set; }
        [JsonPropertyName("actual")]
        public dynamic actual { get; set; }
    }
}
