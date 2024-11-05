using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using DiiaClient.SDK.Exception;
using DiiaClient.SDK.Helper;
using DiiaClient.SDK.Interfaces.Remote;
using DiiaClient.SDK.Models.Remote;

namespace DiiaClient.SDK.Services.Remote
{
    internal class DiiaValidationApiImpl : IDiiaValidationApi
    {
        private readonly string baseDiiaUrl;
        private readonly HttpMethodExecutor httpMethodExecutor;

        public DiiaValidationApiImpl(string baseDiiaUrl, HttpMethodExecutor httpMethodExecutor)
        {
            this.baseDiiaUrl = baseDiiaUrl;
            this.httpMethodExecutor = httpMethodExecutor;
        }

        /*
            curl -X POST "https://{diia_host}/api/v1/acquirers/document-identification"
            -H "accept: application/json"
            -H "Authorization: Bearer {session_token}"
            -H "Content-Type: application/json"
            -d "{\"branchId\":\"{branch_id}\",\"barcode\":\"{barcode}\"}"
        */
        public async Task<bool> ValidateDocumentByBarcode(string branchId, string barcode)
        {
            try
            {
                var documentRequest = new DocumentRequest()
                { BranchId = branchId, Barcode = barcode };

                var options = new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic)
                };
                var content = JsonSerializer.Serialize(documentRequest, options);
                return (await httpMethodExecutor.DoPost<SimpleResponse>($"{baseDiiaUrl}/api/v1/acquirers/document-identification", content)).Success;
            }
            catch (System.Exception e)
            {
                throw new DiiaClientException("Document validation error", e);
            }
        }
    }
}
