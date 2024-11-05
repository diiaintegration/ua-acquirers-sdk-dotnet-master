using DiiaClient.SDK.Interfaces.Local;
using DiiaClient.SDK.Interfaces.Remote;
using DiiaClient.SDK.Models.Remote;
using File = DiiaClient.SDK.Models.Remote.File;

namespace DiiaClient.SDK.Services.Local
{
    internal class SignServiceImpl : ISignService
    {
        private readonly IDiiaSignApi diiaSignApi;
        

        public SignServiceImpl(IDiiaSignApi diiaSignApi)
        {
            this.diiaSignApi = diiaSignApi;
        }

        public async Task<string> GetSignDeepLink(string branchId, string offerId, string requestId, List<File> files)
        {
            return await diiaSignApi.GetSignDeepLink(branchId, offerId, requestId, files);
        }

        public async Task<AuthDeepLink> GetAuthDeepLink(string branchId, string offerId, string requestId, string returnLink = null)
        {
            return await diiaSignApi.GetAuthDeepLink(branchId, offerId, requestId, returnLink);
        }

        public SignaturePackage DecodeSignaturePackage(Dictionary<string, string> headers, string encodedData)
        {
            return diiaSignApi.DecodeSignaturePackage(headers, encodedData);
        }
    }
}
