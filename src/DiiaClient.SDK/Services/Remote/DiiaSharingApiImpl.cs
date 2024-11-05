using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using DiiaClient.SDK.Exception;
using DiiaClient.SDK.Helper;
using DiiaClient.SDK.Interfaces.Remote;
using DiiaClient.SDK.Models.Local;
using DiiaClient.SDK.Models.Remote;

namespace DiiaClient.SDK.Services.Remote
{
    internal class DiiaSharingApiImpl : IDiiaSharingApi
    {
        private readonly string baseDiiaUrl;
        private readonly HttpMethodExecutor httpMethodExecutor;

        public DiiaSharingApiImpl(string baseDiiaUrl, HttpMethodExecutor httpMethodExecutor)
        {
            this.baseDiiaUrl = baseDiiaUrl;
            this.httpMethodExecutor = httpMethodExecutor;
        }

        /*
            curl -X POST "https://{diia_host}/api/v1/acquirers/document-request"
            -H  "accept: application/json"
            -H  "Authorization: Bearer {session_token}"
            -H  "Content-Type: application/json"
            -d "{ \"branchId\": \"{branch_id}\", \"barcode\": \"{barcode}\", \"requestId\": \"{request_id}\" }"
        */
        public async Task<bool> RequestDocumentByBarCode(string branchId, string barcode, string requestId)
        {
            try
            {
                var documentRequest = new DocumentRequest()
                { BranchId = branchId, Barcode = barcode, RequestId = requestId };

                var options = new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic)
                };
                var content = JsonSerializer.Serialize(documentRequest, options);
                return (await httpMethodExecutor.DoPost<SimpleResponse>($"{baseDiiaUrl}/api/v1/acquirers/document-request", content)).Success;
            }
            catch (System.Exception e)
            {
                throw new DiiaClientException("Document request error", e);
            }
        }

        /*
            curl -X POST "https://{diia_host}/api/v1/acquirers/document-request"
            -H  "accept: application/json"
            -H  "Authorization: Bearer {session_token}"
            -H  "Content-Type: application/json"
            -d "{ \"branchId\": \"{branch_id}\", \"qrcode\": \"{qrcode}\", \"requestId\": \"{request_id}\" }"
        */
        public async Task<bool> RequestDocumentByQRCode(string branchId, string qrcode, string requestId)
        {
            try
            {
                var documentRequest = new DocumentRequest()
                { BranchId = branchId, Qrcode = qrcode, RequestId = requestId };

                var options = new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic)
                };
                var content = JsonSerializer.Serialize(documentRequest, options);
                return (await httpMethodExecutor.DoPost<SimpleResponse>($"{baseDiiaUrl}/api/v1/acquirers/document-request", content)).Success;
            }
            catch (System.Exception e)
            {
                throw new DiiaClientException("Document request error", e);
            }
        }

        /*
            curl -X POST "https://{diia_host}/api/v2/acquirers/branch/{branch_id}/offer-request/dynamic"
            -H  "accept: application/json"
            -H  "Authorization: Bearer {session_token}"
            -H  "Content-Type: application/json"
            -d "{ \"offerId\": \"{offer_id}\", \"requestId\": \"{request_id}\" }"
        */
        public async Task<string> GetDeepLink(string branchId, string offerId, string requestId, string returnLink = null, List<HashedFile> hashedFiles = null)
        {
            try
            {
                var deepLinkRequest = new DeepLinkRequest()
                {
                    RequestId = requestId,
                    OfferId = offerId,
                    ReturnLink = returnLink
                };
                
                if (hashedFiles?.Count > 0)
                {
                    deepLinkRequest.Data = new() {HashedFilesSign = new() {Hashs = hashedFiles}};
                }

                var options = new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                };
                var content = JsonSerializer.Serialize(deepLinkRequest, options);
                return (await httpMethodExecutor.DoPost<DeepLink>($"{baseDiiaUrl}/api/v2/acquirers/branch/{branchId}/offer-request/dynamic", content)).Deeplink;
            }
            catch (System.Exception e)
            {
                throw new DiiaClientException("DeepLink request error", e);
            }
        }
    }
}
