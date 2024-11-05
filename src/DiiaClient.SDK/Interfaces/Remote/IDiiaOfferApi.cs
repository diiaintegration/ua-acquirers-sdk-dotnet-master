using DiiaClient.SDK.Models.Remote;

namespace DiiaClient.SDK.Interfaces.Remote
{
    public interface IDiiaOfferApi
    {
        Task<string> CreateOffer(string branchId, Offer offer);
        Task<OfferList> GetOffers(string branchId, long? skip, long? limit);
        Task DeleteOffer(string branchId, string offerId);
    }
}
