using System.Text.Json.Serialization;

namespace DiiaClient.SDK.Models.Local
{
    /// <summary>
    /// Біометричний закордонний паспорт або закордонний паспорт.
    /// Biometric foreign passport or simple foreign passport
    /// </summary>
    [Serializable]
    public class ForeignPassport
    {
        [JsonPropertyName("taxpayerNumber")]
        public string TaxpayerNumber { get; set; }
        [JsonPropertyName("residenceUA")]
        public string ResidenceUA { get; set; }
        [JsonPropertyName("docNumber")]
        public string DocNumber { get; set; }
        [JsonPropertyName("genderUA")]
        public string GenderUA { get; set; }
        [JsonPropertyName("nationalityUA")]
        public string NationalityUA { get; set; }
        [JsonPropertyName("lastNameUA")]
        public string LastNameUA { get; set; }
        [JsonPropertyName("firstNameUA")]
        public string FirstNameUA { get; set; }
        [JsonPropertyName("middleNameUA")]
        public string MiddleNameUA { get; set; }
        [JsonPropertyName("birthday")]
        public string Birthday { get; set; }
        [JsonPropertyName("birthPlaceUA")]
        public string BirthPlaceUA { get; set; }
        [JsonPropertyName("issueDate")]
        public string IssueDate { get; set; }
        [JsonPropertyName("expirationDate")]
        public string ExpirationDate { get; set; }
        [JsonPropertyName("recordNumber")]
        public string RecordNumber { get; set; }
        [JsonPropertyName("departmentUA")]
        public string DepartmentUA { get; set; }
        [JsonPropertyName("countryCode")]
        public string CountryCode { get; set; }
        [JsonPropertyName("genderEN")]
        public string GenderEN { get; set; }
        [JsonPropertyName("lastNameEN")]
        public string LastNameEN { get; set; }
        [JsonPropertyName("firstNameEN")]
        public string FirstNameEN { get; set; }
        [JsonPropertyName("departmentEN")]
        public string DepartmentEN { get; set; }
        [JsonPropertyName("fileName")]
        public string FileName { get; set; }
    }
}
