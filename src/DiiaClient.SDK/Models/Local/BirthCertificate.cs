using System.Text.Json.Serialization;

namespace DiiaClient.SDK.Models.Local
{
    /// <summary>
    /// Свідоцтво про народження дитини.
    /// Child's birth certificate
    /// </summary>
    [Serializable]
    public class BirthCertificate
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("document")]
        public Document Document { get; set; }
        [JsonPropertyName("child")]
        public Child Child { get; set; }
        [JsonPropertyName("parents")]
        public Parents Parents { get; set; }
        [JsonPropertyName("act")]
        public Act Act { get; set; }
        [JsonPropertyName("fileName")]
        public string FileName { get; set; }
    }

    [Serializable]
    public class Document
    {
        [JsonPropertyName("series")]
        public string Series { get; set; }
        [JsonPropertyName("number")]
        public string Number { get; set; }
        [JsonPropertyName("department")]
        public string Department { get; set; }
        [JsonPropertyName("issueDate")]
        public string IssueDate { get; set; }
    }

    [Serializable]
    public class Child
    {
        [JsonPropertyName("lastName")]
        public string LastName { get; set; }
        [JsonPropertyName("firstName")]
        public string FirstName { get; set; }
        [JsonPropertyName("middleName")]
        public string MiddleName { get; set; }
        [JsonPropertyName("birthDate")]
        public string BirthDate { get; set; }
        [JsonPropertyName("birthPlace")]
        public string BirthPlace { get; set; }
        [JsonPropertyName("currentRegistrationPlaceUA")]
        public string CurrentRegistrationPlaceUA { get; set; }
        [JsonPropertyName("citizenship")]
        public string Citizenship { get; set; }
    }

    [Serializable]
    public class Parents
    {
        [JsonPropertyName("father")]
        public Parent Father { get; set; }
        [JsonPropertyName("mother")]
        public Parent Mother { get; set; }
    }

    [Serializable]
    public class Parent
    {
        [JsonPropertyName("fullName")]
        public string FullName { get; set; }
        [JsonPropertyName("citizenship")]
        public string Citizenship { get; set; }
    }

    [Serializable]
    public class Act
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("registrationPlace")]
        public string RegistrationPlace { get; set; }
    }
}
