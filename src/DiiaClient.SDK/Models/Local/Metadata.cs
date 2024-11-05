using System.Text.Json.Serialization;
using static DiiaClient.SDK.Models.Remote.DocumentTypeConstant;

namespace DiiaClient.SDK.Models.Local
{
    [Serializable]
    public class Metadata
    {
        [JsonPropertyName("requestId")]
        public string RequestId { get; set; }
        [JsonPropertyName("documentTypes")]
        public List<string> DocumentTypes { get; set; }
        [JsonPropertyName("data")]
        public Data Data { get; set; }
    }

    [Serializable]
    public class Data
    {
        [JsonPropertyName(DOC_TYPE_INTERNAL_PASSPORT)]
        public List<InternalPassport> InternalPassport { get; set; }
        [JsonPropertyName(DOC_TYPE_FOREIGN_PASSPORT)]
        public List<ForeignPassport> ForeignPassport { get; set; }
        [JsonPropertyName(DOC_TYPE_TAXPAYER_CARD)]
        public List<TaxpayerCard> TaxpayerCard { get; set; }
        [JsonPropertyName(DOC_TYPE_REFERENCE_INTERNALLY_DISPLACED_PERSON)]
        public List<ReferenceInternallyDisplacedPerson> ReferenceInternallyDisplacedPerson { get; set; }
        [JsonPropertyName(DOC_TYPE_BIRTH_CERTIFICATE)]
        public List<BirthCertificate> BirthCertificate { get; set; }
        [JsonPropertyName(DOC_TYPE_DRIVER_LICENSE)]
        public List<DriverLicense> DriverLicense { get; set; }
        [JsonPropertyName(DOC_TYPE_VEHICLE_LICENSE)]
        public List<VehicleLicense> VehicleLicense { get; set; }
    }
}
