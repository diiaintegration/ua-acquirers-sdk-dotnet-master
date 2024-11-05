using System.Text.Json.Serialization;

namespace DiiaClient.SDK.Models.Local
{
    /// <summary>
    /// Тех паспорт.
    /// Vehicle license
    /// </summary>
    [Serializable]
    public class VehicleLicense
    {
        [JsonPropertyName("licensePlate")]
        public string LicensePlate { get; set; }
        [JsonPropertyName("docNumber")]
        public string DocNumber { get; set; }
        [JsonPropertyName("brand")]
        public string Brand { get; set; }
        [JsonPropertyName("model")]
        public string Model { get; set; }
        [JsonPropertyName("vin")]
        public string Vin { get; set; }
        [JsonPropertyName("color")]
        public string Color { get; set; }
        [JsonPropertyName("kindBody")]
        public string KindBody { get; set; }
        [JsonPropertyName("makeYear")]
        public string MakeYear { get; set; }
        [JsonPropertyName("totalWeight")]
        public string TotalWeight { get; set; }
        [JsonPropertyName("ownWeight")]
        public string OwnWeight { get; set; }
        [JsonPropertyName("capacity")]
        public string Capacity { get; set; }
        [JsonPropertyName("fuel")]
        public string Fuel { get; set; }
        [JsonPropertyName("rankCategory")]
        public string RankCategory { get; set; }
        [JsonPropertyName("seatsNumber")]
        public string SeatsNumber { get; set; }
        [JsonPropertyName("standingNumber")]
        public string StandingNumber { get; set; }
        [JsonPropertyName("dateFirstReg")]
        public string DateFirstReg { get; set; }
        [JsonPropertyName("dateReg")]
        public string DateReg { get; set; }
        [JsonPropertyName("ownerType")]
        public string OwnerType { get; set; }
        [JsonPropertyName("lastNameUA")]
        public string LastNameUA { get; set; }
        [JsonPropertyName("firstNameUA")]
        public string FirstNameUA { get; set; }
        [JsonPropertyName("middleNameUA")]
        public string MiddleNameUA { get; set; }
        [JsonPropertyName("birthday")]
        public string Birthday { get; set; }
        [JsonPropertyName("address")]
        public string Address { get; set; }
        [JsonPropertyName("fileName")]
        public string FileName { get; set; }
    }
}
