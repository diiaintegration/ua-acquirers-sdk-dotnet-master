using DiiaClient.SDK.Interfaces.Local;
using DiiaClient.SDK.Interfaces.Remote;
using DiiaClient.SDK.Models.Remote;

namespace DiiaClient.SDK.Services.Local
{
    internal class OfferServiceImpl : IOfferService
    {
        private readonly IDiiaOfferApi diiaOfferApi;

        public OfferServiceImpl(IDiiaOfferApi diiaOfferApi)
        {
            this.diiaOfferApi = diiaOfferApi;
        }

        public async Task<OfferList> GetOffers(string branchId, long? skip, long? limit)
        {
            return await diiaOfferApi.GetOffers(branchId, skip, limit);
        }

        public async Task<string> CreateOffer(string branchId, Offer offer)
        {
            return await diiaOfferApi.CreateOffer(branchId, offer);
        }

        public async Task DeleteOffer(string branchId, string offerId)
        {
            await diiaOfferApi.DeleteOffer(branchId, offerId);
        }
    }
}
