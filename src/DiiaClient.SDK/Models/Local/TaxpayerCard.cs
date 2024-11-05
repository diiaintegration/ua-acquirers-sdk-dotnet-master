using System.Text.Json.Serialization;

namespace DiiaClient.SDK.Models.Local
{
    /// <summary>
    /// РНОКПП (ІПН).
    /// Taxpayer card
    /// </summary>
    [Serializable]
    public class TaxpayerCard
    {
        [JsonPropertyName("creationDate")]
        public string CreationDate { get; set; }
        [JsonPropertyName("docNumber")]
        public string DocNumber { get; set; }
        [JsonPropertyName("lastNameUA")]
        public string LastNameUA { get; set; }
        [JsonPropertyName("firstNameUA")]
        public string FirstNameUA { get; set; }
        [JsonPropertyName("middleNameUA")]
        public string MiddleNameUA { get; set; }
        [JsonPropertyName("birthday")]
        public string Birthday { get; set; }
        [JsonPropertyName("fileName")]
        public string FileName { get; set; }
    }
}
