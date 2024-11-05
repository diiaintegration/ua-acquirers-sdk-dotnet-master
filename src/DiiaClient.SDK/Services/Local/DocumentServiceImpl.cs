using System.Text.Json;
using DiiaClient.CryptoAPI;
using DiiaClient.SDK.Exception;
using DiiaClient.SDK.Interfaces.Local;
using DiiaClient.SDK.Models.Local;
using DiiaClient.SDK.Models.Remote;

namespace DiiaClient.SDK.Services.Local
{
    internal class DocumentServiceImpl : IDocumentService
    {
        private readonly ICryptoService cryptoService;
        private static readonly string HeaderNameRequestId = "X-Document-Request-Trace-Id";

        public DocumentServiceImpl(ICryptoService cryptoService)
        {
            this.cryptoService = cryptoService;
        }

        public DocumentPackage ProcessDocumentPackage(Dictionary<string, string> headers, List<EncodedFile> multipartBody, string encodedJsonData)
        {
            try
            {
                var documentPackage = new DocumentPackage();
                documentPackage.RequestId = headers.First(x => x.Key.Equals(HeaderNameRequestId, StringComparison.OrdinalIgnoreCase)).Value;
                var decrypted = cryptoService.Decrypt(encodedJsonData);
                Metadata decryptedMetaData = JsonSerializer.Deserialize<Metadata>(decrypted);
                documentPackage.Data = decryptedMetaData;

                List<DecodedFile> decodedFiles = new List<DecodedFile>();
                foreach (var encodedFile in multipartBody)
                {
                    DecodedFile decodedFile = new DecodedFile();
                    decodedFile.FileName = encodedFile.Name;
                    decodedFile.Data = cryptoService.Decrypt(encodedFile.Data);
                    decodedFiles.Add(decodedFile);
                }

                documentPackage.DecodedFiles = decodedFiles;

                return documentPackage;
            }
            catch (System.Exception ex)
            {
                throw new DiiaClientException($"Failed ProcessDocumentPackage {ex}");
            }
        }
    }
}
