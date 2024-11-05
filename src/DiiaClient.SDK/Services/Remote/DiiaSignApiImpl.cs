using System.Text;
using System.Text.Json;
using DiiaClient.CryptoAPI;
using DiiaClient.SDK.Exception;
using DiiaClient.SDK.Interfaces.Local;
using DiiaClient.SDK.Interfaces.Remote;
using DiiaClient.SDK.Models.Local;
using DiiaClient.SDK.Models.Remote;
using File = DiiaClient.SDK.Models.Remote.File;

namespace DiiaClient.SDK.Services.Remote
{
    internal class DiiaSignApiImpl : IDiiaSignApi
    {
        private static readonly string DiiaIdActionHeader = "x-diia-id-action";
        private static readonly string HeaderNameRequestId = "X-Document-Request-Trace-Id";

        private readonly ICryptoService cryptoService;
        private readonly ISharingService diiaSharingService;

        public DiiaSignApiImpl(ICryptoService cryptoService, ISharingService diiaSharingService)
        {
            this.cryptoService = cryptoService;
            this.diiaSharingService = diiaSharingService;

        }

        public async Task<string> GetSignDeepLink(string branchId, string offerId, string requestId, List<File> files)
        {
            var hashedFiles = new List<HashedFile>();

            foreach (var file in files)
            {
                var base64 = Convert.ToBase64String(file.Data);
                var fileHash = cryptoService.CalcHash(base64);
                hashedFiles.Add(new() { FileName = file.FileName, FileHash = fileHash });
            }

            return await diiaSharingService.GetDeepLink(branchId, offerId, requestId, hashedFiles: hashedFiles);
        }

        public async Task<AuthDeepLink> GetAuthDeepLink(string branchId, string offerId, string requestId,
            string returnLink = null)
        {
            var requestIdHash = cryptoService.CalcHash(requestId);

            return new AuthDeepLink()
            {
                DeepLink = await diiaSharingService.GetDeepLink(branchId, offerId, requestIdHash,
                    returnLink: returnLink),
                RequestId = requestId,
                RequestIdHash = requestIdHash
            };
        }

        public SignaturePackage DecodeSignaturePackage(Dictionary<string, string> headers, string encodedData)
        {
            try
            {
                var signaturePackage = new SignaturePackage();
                var diiaIdAction = headers.First(x => x.Key.Equals(DiiaIdActionHeader, StringComparison.OrdinalIgnoreCase)).Value;
                var requestId = headers.First(x => x.Key.Equals(HeaderNameRequestId, StringComparison.OrdinalIgnoreCase)).Value;
                var requestDataBytes = Convert.FromBase64String(encodedData);
                
                if (diiaIdAction == DiiaIDAction.HASHED_FILES_SIGNING)
                {
                    signaturePackage = JsonSerializer.Deserialize<SignaturePackage>(requestDataBytes);
                    signaturePackage.Signatures.ForEach(x => x.Filename = $"{x.Filename.Split('.')[0]}.p7s");
                }
                else if (diiaIdAction == DiiaIDAction.AUTH)
                {
                    // set filename to auth.p7s, because encodedData["requestId"]
                    // is crypto-hash and not url/filesystem-safe
                    // encodedData["requestId"] == request_id from headers
                    signaturePackage.Signatures = new()
                    {
                        JsonSerializer.Deserialize<Signatures>(requestDataBytes)
                    };
                    signaturePackage.Signatures.FirstOrDefault().Filename = "auth.p7s";
                }

                signaturePackage.DiiaIdAction = diiaIdAction;
                signaturePackage.RequestId = requestId;
                return signaturePackage;
            }
            catch (System.Exception ex)
            {
                throw new DiiaClientException($"Failed DecodeSignaturePackage {ex}");
            }
        }
    }
}
