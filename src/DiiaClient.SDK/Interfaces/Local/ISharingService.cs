using DiiaClient.SDK.Models.Local;

namespace DiiaClient.SDK.Interfaces.Local
{
    internal interface ISharingService
    {
        Task<string> GetDeepLink(string branchId, string offerId, string requestId, string returnLink = null, List<HashedFile> hashedFiles = null);
        Task<bool> RequestDocumentByBarCode(string branchId, string barCode, string requestId);
        Task<bool> RequestDocumentByQRCode(string branchId, string qrCode, string requestId);
    }
}
