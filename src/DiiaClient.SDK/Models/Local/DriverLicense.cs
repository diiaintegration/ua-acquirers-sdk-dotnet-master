using System.Text.Json.Serialization;

namespace DiiaClient.SDK.Models.Local
{
    /// <summary>
    /// Посвідчення водія.
    /// Driver license
    /// </summary>
    [Serializable]
    public class DriverLicense
    {
        [JsonPropertyName("expirationDate")]
        public string ExpirationDate { get; set; }
        [JsonPropertyName("categories")]
        public string Categories { get; set; }
        [JsonPropertyName("serialNumber")]
        public string SerialNumber { get; set; }
        [JsonPropertyName("lastNameUA")]
        public string LastNameUA { get; set; }
        [JsonPropertyName("firstNameUA")]
        public string FirstNameUA { get; set; }
        [JsonPropertyName("middleNameUA")]
        public string MiddleNameUA { get; set; }
        [JsonPropertyName("birthday")]
        public string Birthday { get; set; }
        [JsonPropertyName("department")]
        public string Department { get; set; }
        [JsonPropertyName("fileName")]
        public string FileName { get; set; }
    }
}
