using System.Text.Json.Serialization;

namespace DiiaClient.SDK.Models.Local
{
    /// <summary>
    /// Довідка внутрішньо переміщеної особи (ВПО).
    /// Internally displaced person certificate
    /// </summary>
    [Serializable]
    public class ReferenceInternallyDisplacedPerson
    {
        [JsonPropertyName("docType")]
        public string DocType { get; set; }
        [JsonPropertyName("docNumber")]
        public string DocNumber { get; set; }
        [JsonPropertyName("department")]
        public string Department { get; set; }
        [JsonPropertyName("lastName")]
        public string LastName { get; set; }
        [JsonPropertyName("firstName")]
        public string FirstName { get; set; }
        [JsonPropertyName("middleName")]
        public string MiddleName { get; set; }
        [JsonPropertyName("issueDate")]
        public string IssueDate { get; set; }
        [JsonPropertyName("birthday")]
        public string BirthDate { get; set; }
        [JsonPropertyName("gender")]
        public string Gender { get; set; }
        [JsonPropertyName("legalRepresentative")]
        public string LegalRepresentative { get; set; }
        [JsonPropertyName("docIdentity")]
        public DocIdentity DocIdentity { get; set; }
        [JsonPropertyName("address")]
        public Address Address { get; set; }
        [JsonPropertyName("fileName")]
        public string FileName { get; set; }
    }

    [Serializable]
    public class DocIdentity
    {
        [JsonPropertyName("number")]
        public string Number { get; set; }
        [JsonPropertyName("issueDate")]
        public string IssueDate { get; set; }
        [JsonPropertyName("department")]
        public string Department { get; set; }
    }

    [Serializable]
    public class Address
    {
        [JsonPropertyName("birth")]
        public string Birth { get; set; }
        [JsonPropertyName("registration")]
        public string Registration { get; set; }
        [JsonPropertyName("actual")]
        public string Actual { get; set; }
    }
}
