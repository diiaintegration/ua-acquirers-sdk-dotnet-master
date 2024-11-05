using DiiaClient.SDK.Models.Local;
using DiiaClient.SDK.Models.Remote;

namespace DiiaClient.SDK.Interfaces
{
    public interface IDiia
    {
        public Task<BranchList> GetBranches(long? skip, long? limit);
        public Task<Branch> GetBranch(string branchId);
        public Task DeleteBranch(string branchId);
        public Task<Branch> CreateBranch(Branch branch);
        public Task<Branch> UpdateBranch(Branch branch);
        
        public Task<OfferList> GetOffers(string branchId, long? skip, long? limit);
        public Task<string> CreateOffer(string branchId, Offer offer);
        public Task DeleteOffer(string branchId, string offerId);
        
        public Task<bool> ValidateDocumentByBarcode(string branchId, string barcode);
        
        public Task<bool> RequestDocumentByBarCode(string branchId, string barCode, string requestId);
        //public bool requestDocumentByQRCode(string branchId, string qrCode, string requestId);
        public Task<string> GetDeepLink(string branchId, string offerId, string requestId);
        public DocumentPackage DecodeDocumentPackage(Dictionary<string, string> headers, List<EncodedFile> multipartBody, string encodedJsonData);
        
        public Task<string> GetSignDeepLink(string branchId, string offerId, string requestId, List<Models.Remote.File> files);
        public Task<AuthDeepLink> GetAuthDeepLink(string branchId, string offerId, string requestId, string returnLink = null);
        public SignaturePackage DecodeSignaturePackage(Dictionary<string, string> headers, string encodedData);
    }
}
