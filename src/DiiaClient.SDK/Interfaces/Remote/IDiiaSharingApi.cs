using DiiaClient.SDK.Models.Local;

namespace DiiaClient.SDK.Interfaces.Remote
{
    public interface IDiiaSharingApi
    {
        Task<bool> RequestDocumentByBarCode(string branchId, string barcode, string requestId);
        Task<bool> RequestDocumentByQRCode(string branchId, string qrcode, string requestId);
        Task<string> GetDeepLink(string branchId, string offerId, string requestId, string returnLink = null, List<HashedFile> hashedFiles = null);
    }
}
