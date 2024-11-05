using DiiaClient.SDK.Models.Remote;

namespace DiiaClient.SDK.Interfaces.Local
{
    internal interface IOfferService
    {
        Task<OfferList> GetOffers(string branchId, long? skip, long? limit);
        Task<string> CreateOffer(string branchId, Offer offer);
        Task DeleteOffer(string branchId, string offerId);
    }
}
