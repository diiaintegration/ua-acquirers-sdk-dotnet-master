using System.Collections.Specialized;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using DiiaClient.SDK.Exception;
using DiiaClient.SDK.Helper;
using DiiaClient.SDK.Interfaces.Remote;
using DiiaClient.SDK.Models.Remote;

namespace DiiaClient.SDK.Services.Remote
{
    internal class DiiaOfferApiImpl : IDiiaOfferApi
    {
        private readonly string baseDiiaUrl;
        private readonly HttpMethodExecutor httpMethodExecutor;

        public DiiaOfferApiImpl(string baseDiiaUrl, HttpMethodExecutor httpMethodExecutor)
        {
            this.baseDiiaUrl = baseDiiaUrl;
            this.httpMethodExecutor = httpMethodExecutor;
        }

        /*
           curl -X POST "https://{diia_host}/api/v1/acquirers/branch/{branch_id}/offer"
           -H  "accept: application/json"
           -H  "Authorization: Bearer {session_token}"
           -H  "Content-Type: application/json"
           -d "{ \"name\": \"Назва послуги\", \"scopes\": { \"sharing\": [\"passport\"] } }"
        */
        public async Task<string> CreateOffer(string branchId, Offer offer)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                };
                var content = JsonSerializer.Serialize(offer, options);
                return (await httpMethodExecutor.DoPost<Id>($"{baseDiiaUrl}/api/v1/acquirers/branch/{branchId}/offer", content)).ID;
            }
            catch (System.Exception e)
            {
                throw new DiiaClientException("Offer creation error", e);
            }
        }

        /*
            curl -X GET "https://{diia_host}/api/v1/acquirers/branch/{branch_id}/offers?skip=0&limit=100"
            -H  "accept: application/json"
            -H  "Authorization: Bearer {session_token}"
        */
        public async Task<OfferList> GetOffers(string branchId, long? skip, long? limit)
        {
            try
            {
                NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(String.Empty);
                queryString.Add("skip", skip.ToString());
                queryString.Add("limit", limit.ToString());

                return await httpMethodExecutor.DoGet<OfferList>($"{baseDiiaUrl}/api/v1/acquirers/branch/{branchId}/offers?{queryString}");
            }
            catch (System.Exception e)
            {
                throw new DiiaClientException("Offer request error", e);
            }
        }

        /*
            curl -X DELETE "https://{diia_host}/api/v1/acquirers/branch/{branch_id}/offer/{offer_id}"
            -H "accept: *//*"
            -H "Authorization: Bearer {session_token}"
            -H "Content-Type: application/json"
        */
        public async Task DeleteOffer(string branchId, string offerId)
        {
            try
            {
                await httpMethodExecutor.DoDelete($"{baseDiiaUrl}/api/v1/acquirers/branch/{branchId}/offer/{offerId}");
            }
            catch (System.Exception e)
            {
                throw new DiiaClientException("Offer deletion error", e);
            }
        }
    }
}
