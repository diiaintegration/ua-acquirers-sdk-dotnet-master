using DiiaClient.SDK.Models.Remote;

namespace DiiaClient.SDK.Interfaces.Local
{
    internal interface ISignService
    {
        public Task<string> GetSignDeepLink(string branchId, string offerId, string requestId, List<Models.Remote.File> files);

        public Task<AuthDeepLink> GetAuthDeepLink(string branchId, string offerId, string requestId, string returnLink = null);

        public SignaturePackage DecodeSignaturePackage(Dictionary<string, string> headers, string encodedData);
    }
}
