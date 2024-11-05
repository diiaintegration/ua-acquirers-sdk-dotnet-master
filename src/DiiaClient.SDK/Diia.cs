using DiiaClient.CryptoAPI;
using DiiaClient.SDK.Exception;
using DiiaClient.SDK.Helper;
using DiiaClient.SDK.Interfaces;
using DiiaClient.SDK.Interfaces.Local;
using DiiaClient.SDK.Models.Local;
using DiiaClient.SDK.Models.Remote;
using DiiaClient.SDK.Services.Local;
using DiiaClient.SDK.Services.Remote;

namespace DiiaClient.SDK
{
    public class Diia : IDiia
    {
        private IDocumentService DocumentService { get; set; }
        private ISharingService SharingService { get; set; }
        private IBranchService BranchService { get; set; }
        private IOfferService OfferService { get; set; }
        private IValidationService ValidationService { get; set; }
        private ISignService SignService { get; set; }

        /// <summary>
        /// Main Diia class constructor.
        /// </summary>
        /// <param name="acquirerToken">A token used to identify the Partner.</param>
        /// <param name="baseDiiaUrl">Base URL to Diia REST API.</param>
        /// <param name="httpClient">Preconfigured implementation of HttpClient.</param>
        /// <param name="cryptoService">Preconfigured implementation of ICryptoService</param>
        public Diia(string acquirerToken, string baseDiiaUrl, HttpClient httpClient, ICryptoService cryptoService)
        {
            var httpMethodExecutor = new HttpMethodExecutor(acquirerToken, baseDiiaUrl, httpClient);

            DocumentService = new DocumentServiceImpl(cryptoService);
            SharingService = new SharingServiceImpl(new DiiaSharingApiImpl(baseDiiaUrl, httpMethodExecutor));
            BranchService = new BranchServiceImpl(new DiiaBranchApiImpl(baseDiiaUrl, httpMethodExecutor));
            OfferService = new OfferServiceImpl(new DiiaOfferApiImpl(baseDiiaUrl, httpMethodExecutor));
            ValidationService = new ValidationServiceImpl(new DiiaValidationApiImpl(baseDiiaUrl, httpMethodExecutor));
            SignService = new SignServiceImpl(new DiiaSignApiImpl(cryptoService, SharingService));
        }

        public Diia(string acquirerToken, string diiaHost, ICryptoService cryptoService)
        : this(acquirerToken, diiaHost, new HttpClient(), cryptoService)
        {
        }

        /// <summary>
        /// Get branches list.
        /// </summary>
        /// <param name="skip">Number of branches to be skipped.</param>
        /// <param name="limit">Max number of branches in response.</param>
        /// <returns>Branches list.</returns>
        /// <exception cref="DiiaClientException"></exception>
        public async Task<BranchList> GetBranches(long? skip, long? limit)
        {
            return await BranchService.GetBranches(skip, limit);
        }
        /// <summary>
        /// Get branch by id.
        /// </summary>
        /// <param name="branchId">Branch ID.</param>
        /// <returns>branch</returns>
        /// <exception cref="DiiaClientException"></exception>
        public async Task<Branch> GetBranch(string branchId)
        {
            return await BranchService.GetBranch(branchId);
        }

        /// <summary>
        /// Delete branch by id.
        /// </summary>
        /// <param name="branchId">Branch ID.</param>
        /// <exception cref="DiiaClientException"></exception>
        public async Task DeleteBranch(string branchId)
        {
            await BranchService.DeleteBranch(branchId);
        }

        /// <summary>
        /// Create new branch.
        /// </summary>
        /// <param name="branch">Branch object.</param>
        /// <returns>Created branch.</returns>
        /// <exception cref="DiiaClientException"></exception>
        public async Task<Branch> CreateBranch(Branch branch)
        {
            return await BranchService.CreateBranch(branch);
        }

        /// <summary>
        /// Update existing branch.
        /// </summary>
        /// <param name="branch">Updated branch instance.</param>
        /// <returns>Updated branch.</returns>
        /// <exception cref="DiiaClientException"></exception>
        public async Task<Branch> UpdateBranch(Branch branch)
        {
            return await BranchService.UpdateBranch(branch);
        }

        /// <summary>
        /// Get offers list on the branch.
        /// There may be a lots of offers on one branch.
        /// So it's recommended to limiting requested offers count.
        /// </summary>
        /// <param name="branchId">Branch ID.</param>
        /// <param name="skip">Number of offers to be skipped.</param>
        /// <param name="limit">Max number of offers in response.</param>
        /// <returns>Offers list.</returns>
        /// <exception cref="DiiaClientException"></exception>
        public async Task<OfferList> GetOffers(string branchId, long? skip, long? limit)
        {
            return await OfferService.GetOffers(branchId, skip, limit);
        }

        /// <summary>
        /// Create new offer on the branch.
        /// </summary>
        /// <param name="branchId">Branch ID.</param>
        /// <param name="offer">Offer object.</param>
        /// <returns>Created offer.</returns>
        /// <exception cref="DiiaClientException"></exception>
        public async Task<string> CreateOffer(string branchId, Offer offer)
        {
            return await OfferService.CreateOffer(branchId, offer);
        }

        /// <summary>
        /// Delete offer.
        /// </summary>
        /// <param name="offerId">Offer ID.</param>
        /// <param name="branchId">Branch ID where the offer was created.</param>
        /// <exception cref="DiiaClientException"></exception>
        public async Task DeleteOffer(string branchId, string offerId)
        {
            await OfferService.DeleteOffer(branchId, offerId);
        }

        /// <summary>
        /// Get deep link to start document sharing procedure using online scheme.
        /// </summary>
        /// <param name="branchId">Branch ID.</param>
        /// <param name="offerId">Offer ID, Offer with sharing scopes.</param>
        /// <param name="requestId">Unique request id to identify document sharing action;
        /// it will be sent in http-header with document pack.</param>
        /// <returns>URL, the deep link that should be opened on mobile device
        /// where Diia application is installed.</returns>
        /// <exception cref="DiiaClientException"></exception>
        public async Task<string> GetDeepLink(string branchId, string offerId, string requestId)
        {
            return await SharingService.GetDeepLink(branchId, offerId, requestId);
        }

        /// <summary>
        /// Get deep link for sign files.
        /// </summary>
        /// <param name="branchId">Branch ID.</param>
        /// <param name="offerId">Offer ID, offer with `diia_id:hashedFilesSigning` scopes.</param>
        /// <param name="requestId">Unique request id to identify sign action;
        /// it will be sent in http-header with signatures pack.</param>
        /// <param name="files">List of files for sign.</param>
        /// <returns>URL, the deep link that should be opened on mobile device
        /// where Diia application is installed.</returns>
        /// <exception cref="DiiaClientException"></exception>
        public async Task<string> GetSignDeepLink(string branchId, string offerId, string requestId, List<Models.Remote.File> files)
        {
            return await SignService.GetSignDeepLink(branchId, offerId, requestId, files);
        }

        /// <summary>
        /// Get authorization deep link.
        /// </summary>
        /// <param name="branchId">Branch ID.</param>
        /// <param name="offerId">Offer ID, offer with `diia_id:auth` scopes.</param>
        /// <param name="requestId">Unique request id to identify document sharing action;
        /// hash from request_id will be sent in http-header.</param>
        /// <param name="returnLink">Link where the customer should be redirected after authorization</param>
        /// <returns>AuthDeepLink instance.</returns>
        /// <exception cref="DiiaClientException"></exception>
        public async Task<AuthDeepLink> GetAuthDeepLink(string branchId, string offerId, string requestId, string returnLink = null)
        {
            return await SignService.GetAuthDeepLink(branchId, offerId, requestId, returnLink);
        }

        /// <summary>
        /// Validate document by barcode (on the back-side of document).
        /// </summary>
        /// <param name="branchId">Branch ID.</param>
        /// <param name="barcode">Barcode.</param>
        /// <returns>Sign of document validity.</returns>
        /// <exception cref="DiiaClientException"></exception>
        public async Task<bool> ValidateDocumentByBarcode(string branchId, string barcode)
        {
            return await ValidationService.ValidateDocumentByBarcode(branchId, barcode);
        }

        /// <summary>
        /// Initiate document sharing procedure using document barcode.
        /// </summary>
        /// <param name="branchId">Branch ID.</param>
        /// <param name="barCode">Barcode.</param>
        /// <param name="requestId">Unique request id to identify document sharing action;
        /// it will be sent in http-header with document pack.</param>
        /// <returns>Sign of successful request.</returns>
        /// <exception cref="DiiaClientException"></exception>
        public async Task<bool> RequestDocumentByBarCode(string branchId, string barCode, string requestId)
        {
            return await SharingService.RequestDocumentByBarCode(branchId, barCode, requestId);
        }

        // <summary>
        // Initiate document sharing procedure using document QR code.
        // </summary>
        // <param name="branchId">Branch ID.</param>
        // <param name="qrCode">QR code data.</param>
        // <param name="requestId">Unique request id to identify document sharing action;
        /// it will be sent in http-header with document pack</param>
        // <returns>Sign of successful request.</returns>
        /// <exception cref="DiiaClientException"></exception>
        //public bool requestDocumentByQRCode(string branchId, string qrCode, string requestId)
        //{
        //    return SharingService.requestDocumentByQRCode(branchId, qrCode, requestId);
        //}

        /// <summary>
        /// Unpacking the documents pack received from Diia, check signatures and decipher documents.
        /// </summary>
        /// <param name="headers">All http-headers from the request from Diia application.</param>
        /// <param name="multipartBody">List of EncodedFile based on multipart-body from Diia application request.</param>
        /// <param name="encodedJsonData">Encoded json metadata.</param>
        /// <returns>A collection of received documents in PDF format and it's data in json format.</returns>
        /// <exception cref="DiiaClientException"></exception>
        public DocumentPackage DecodeDocumentPackage(Dictionary<string, string> headers, List<EncodedFile> multipartBody, string encodedJsonData)
        {
            return DocumentService.ProcessDocumentPackage(headers, multipartBody, encodedJsonData);
        }

        /// <summary>
        /// Unpacking signed items received from Diia.
        /// </summary>
        /// <param name="headers">All http-headers from the request from Diia application.</param>
        /// <param name="encodedData">Base64 encodeData from Diia request.</param>
        /// <returns>A collection of received signatures.</returns>
        /// <exception cref="DiiaClientException"></exception>
        public SignaturePackage DecodeSignaturePackage(Dictionary<string, string> headers, string encodedData)
        {
            return SignService.DecodeSignaturePackage(headers, encodedData);
        }
    }
}