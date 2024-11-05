using DiiaClient.SDK.Interfaces.Local;
using DiiaClient.SDK.Interfaces.Remote;
using DiiaClient.SDK.Models.Local;

namespace DiiaClient.SDK.Services.Local
{
    internal class SharingServiceImpl : ISharingService
    {
        private readonly IDiiaSharingApi diiaSharingApi;

        public SharingServiceImpl(IDiiaSharingApi diiaSharingApi)
        {
            this.diiaSharingApi = diiaSharingApi;
        }

        public async Task<string> GetDeepLink(string branchId, string offerId, string requestId, string returnLink = null, List<HashedFile> hashedFiles = null)
        {
            return await diiaSharingApi.GetDeepLink(branchId, offerId, requestId, returnLink, hashedFiles);
        }

        public async Task<bool> RequestDocumentByBarCode(string branchId, string barCode, string requestId)
        {
            return await diiaSharingApi.RequestDocumentByBarCode(branchId, barCode, requestId);
        }

        public async Task<bool> RequestDocumentByQRCode(string branchId, string qrCode, string requestId)
        {
            return await diiaSharingApi.RequestDocumentByQRCode(branchId, qrCode, requestId);
        }
    }
}
