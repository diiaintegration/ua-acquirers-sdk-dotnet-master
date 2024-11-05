using System.Text.Json;
using DiiaClient.SDK;
using DiiaClient.SDK.Interfaces;
using DiiaClient.SDK.Models.Local;
using DiiaClient.SDK.Models.Remote;
using DiiaClient.SDK.Services.Remote;

namespace DiiaClient.Example
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();
        public readonly static string acquirerToken = "";
        public readonly static string authAcquirerToken = "";
        public readonly static string diiaHost = "";
        public readonly static IDiia diiaClient = new Diia(acquirerToken, authAcquirerToken, diiaHost, client, new CryptoService.UAPKI.CryptoService("resources\\config\\config.json"));

        static async Task Main(string[] args)
        {
            //await getSessionToken();
            
            #region Auth
            //create new branch
            string branchIdAuth = await createBranchAuth();
            //string branchIdHfs = "56b1fb12f234d3413e3bedb88f928d59c24d872bda06fbcef9ed21a2501bea9255c4f81a59a8eab0811e8e52b1df4c578691cf46e56326bd57720ff1e0ab9bc2";
            await getBranch(branchIdAuth);
            var offerAuth = new Offer()
            {
                Name = "Hello Auth",
                ReturnLink = "https://test-superapp.alfabank.kiev.ua/mob/net/diia/doc/upload",
                Scopes = new OfferScopes()
                {
                    DiiaId = new List<string>() { "auth" }
                }
            }; 
            string offerIdAuth = await createOffer(branchIdAuth, offerAuth);
            //"8f18106d275e456bc2e3951be450a18bd672bfa41e146670eb6350e5852a90f558334fbb438666efba4438263c42224012bca18d6a4cb38e78eb8ada67dfc898"
            OfferList offersAuth = await getOffers(branchIdAuth, 0, 10);
            #endregion

            #region Sign
            //create new branch
            string branchIdHfs = await createBranchHFS();
            //string branchIdHfs = "478d31fda137b3194437b8148775ee47ccc431704f682d7f2dc643f98ca7a42a5bfaf0892709c0db3c80fed793ad1fc76f2c2537cd309feb33be585fc89f8dd5";
            await getBranch(branchIdHfs);
            var offerHFS = new Offer()
            {
                Name = "HelloHFS",
                ReturnLink = "https://test-superapp.alfabank.kiev.ua/mob/net/diia/doc/upload",
                Scopes = new OfferScopes()
                {
                    DiiaId = new List<string>() { "hashedFilesSigning" }
                }
            }; //"fbe9d2fb55935d93177f46a46b89cb33842e29364972c74fab9aad29b90252fd43683e5f522cecbe4686d84322da41466d76df9e6afe4f25d7a7e26dccf44641"
            string offerIdHFS = await createOffer(branchIdHfs, offerHFS);
            OfferList offersHFS = await getOffers(branchIdHfs, 0, 10);
            #endregion

            #region Document
            string branchId = await createBranch();
            //get all branches
            BranchList branchList = await getBranches(0, 100);
            //update branch if need it
            await updateBranch(branchList.Branches.First(x=>x.Id == branchId));
            //get branch
            await getBranch(branchList.Branches.First().Id);
            //check is valid document
            var isValid = await validateDocumentByBarcode(branchList.Branches.First().Id, "8169516997999");

            //create offer
            var offer = new Offer()
            {
                Name = "Hello world",
                ReturnLink = "https://test-superapp.alfabank.kiev.ua/mob/net/diia/doc/upload",
                Scopes = new OfferScopes()
                {
                    Sharing = new List<string>() { "internal-passport", "foreign-passport" }
                }
            };
            string offerId = await createOffer(branchId, offer);
            //get offers
            OfferList offers = await getOffers(branchId, 0, 10);
            //gen requestId
            var requestId = Guid.NewGuid().ToString(); 
            //get link
            var deepLink = await getDeepLink(branchId, offerId, requestId);
            Console.WriteLine(deepLink);
            //delete offer
            deleteOffer(branchId, offerId).Wait();

            requestId = Guid.NewGuid().ToString();
            //get document by barcode
            var requested = requestDocumentByBarCode(branchId, "5579455552341", requestId);

            //delete branch
            //deleteBranch(id);
            foreach (var item in branchList.Branches)
            {
                await deleteBranch(item.Id);
            }
            //get all branches after delete
            branchList = await getBranches(0, 10);
            #endregion

            Console.ReadKey();
        }

        private static async Task getSessionToken()
        {
            SessionTokenServiceImpl sessionTokenServiceImpl =
                new SessionTokenServiceImpl(acquirerToken, authAcquirerToken, diiaHost, client);
            
            string token = await sessionTokenServiceImpl.GetSessionToken();
            
            Console.WriteLine($"Token: {token}");
        }

        private static async Task<string> createBranch()
        {
            var str =
                "{\"customFullName\":\"ФОП Харламов Владислав Михайлович\", \"customFullAddress\":\"м. Київ, вул. Ніжинська, 29Б\"," +
                "\"name\":\"ФОП Харламов Владислав Михайлович\", \"email\":\"vladislav.kharlamov@alfabank.kiev.ua\", \"region\":\"Київська обл.\"," +
                "\"district\":\"Києво-Святошинський р-н\", \"location\":\"м. Київ\", \"street\":\"вул. Ніжинська\"," +
                "\"house\":\"29Д\", \"deliveryTypes\": [\"api\"], \"offerRequestType\": \"dynamic\"," +
                "\"scopes\":{\"sharing\":[\"passport\",\"internal-passport\",\"foreign-passport\"], \"identification\":[]," +
                "\"documentIdentification\":[\"internal-passport\",\"foreign-passport\"]}}";
            Branch branch = JsonSerializer.Deserialize<Branch>(str);


            Branch branchList = await diiaClient.CreateBranch(branch);
            return branchList.Id;
        }

        private static async Task<string> createBranchHFS()
        {
            var str =
                "{\"customFullName\":\"ФОП Харламов Владислав Михайлович\", \"customFullAddress\":\"м. Київ, вул. Ніжинська, 29Б\"," +
                "\"name\":\"ФОП Харламов Владислав Михайлович\", \"email\":\"vladislav.kharlamov@alfabank.kiev.ua\", \"region\":\"Київська обл.\"," +
                "\"district\":\"Києво-Святошинський р-н\", \"location\":\"м. Київ\", \"street\":\"вул. Ніжинська\"," +
                "\"house\":\"29Д\", \"deliveryTypes\": [\"api\"], \"offerRequestType\": \"dynamic\"," +
                "\"scopes\":{\"diiaId\":[\"hashedFilesSigning\"]}}";
            Branch branch = JsonSerializer.Deserialize<Branch>(str);


            Branch branchList = await diiaClient.CreateBranch(branch);
            return branchList.Id;
        }

        private static async Task<string> createBranchAuth()
        {
            var str =
                "{\"customFullName\":\"ФОП Харламов Владислав Михайлович\", \"customFullAddress\":\"м. Київ, вул. Ніжинська, 29Б\"," +
                "\"name\":\"ФОП Харламов Владислав Михайлович\", \"email\":\"vladislav.kharlamov@alfabank.kiev.ua\", \"region\":\"Київська обл.\"," +
                "\"district\":\"Києво-Святошинський р-н\", \"location\":\"м. Київ\", \"street\":\"вул. Ніжинська\"," +
                "\"house\":\"29Д\", \"deliveryTypes\": [\"api\"], \"offerRequestType\": \"dynamic\"," +
                "\"scopes\":{\"diiaId\":[\"auth\"]}}";
            Branch branch = JsonSerializer.Deserialize<Branch>(str);


            Branch branchList = await diiaClient.CreateBranch(branch);
            return branchList.Id;
        }

        private static async Task<BranchList> getBranches(long skip, long limit)
        {
            BranchList branchList = await diiaClient.GetBranches(skip, limit);
            return branchList;
        }

        private static async Task getBranch(string branchId)
        {
            Branch branch = await diiaClient.GetBranch(branchId);
        }

        private static async Task deleteBranch(string branchId)
        {
            await diiaClient.DeleteBranch(branchId);
        }

        private static async Task<Branch> updateBranch(Branch branch)
        {
            branch.CustomFullAddress = "м. Київ, вул. Ніжинська, 29Д";
            Branch _updatedBranch = await diiaClient.UpdateBranch(branch);
            return _updatedBranch;
        }

        private static async Task<OfferList> getOffers(string branchId, long skip, long limit)
        {
            OfferList _offers = await diiaClient.GetOffers(branchId, skip, limit);
            return _offers;
        }

        private static async Task<string> createOffer(string branchId, Offer offer)
        {
            string _offer = await diiaClient.CreateOffer(branchId, offer);
            return _offer;
        }

        private static async Task deleteOffer(string branchId, string offerId)
        {
            await diiaClient.DeleteOffer(branchId, offerId);
        }

        private static async Task<bool> validateDocumentByBarcode(string branchId, string barcode)
        {
            bool valid = await diiaClient.ValidateDocumentByBarcode(branchId, barcode);
            return valid;
        }

        private static async Task<bool> requestDocumentByBarCode(string branchId, string barcode, string requestId)
        {
            bool valid = await diiaClient.RequestDocumentByBarCode(branchId, barcode, requestId);
            return valid;
        }

        private static async Task<string> getDeepLink(string branchId, string offerId, string requestId)
        {
            string link = await diiaClient.GetDeepLink(branchId, offerId, requestId);
            return link;
        }

        private static void decodeDocumentPackage(Dictionary<string, string> headers, List<EncodedFile> multipartBody, string encodedJsonData)
        {
            DocumentPackage document = diiaClient.DecodeDocumentPackage(headers, multipartBody, encodedJsonData);
        }
    }
}