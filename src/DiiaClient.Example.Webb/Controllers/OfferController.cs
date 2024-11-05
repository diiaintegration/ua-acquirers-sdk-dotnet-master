using DiiaClient.Example.Web.Authorization;
using DiiaClient.Example.Web.Models;
using DiiaClient.SDK.Interfaces;
using DiiaClient.SDK.Models.Remote;
using Microsoft.AspNetCore.Mvc;

namespace DiiaClient.Example.Web.Controllers
{
    [Authorize]
    public class OfferController : Controller
    {
        private readonly IDiia diia;

        public OfferController(IDiia diia)
        {
            this.diia = diia;
        }

        [HttpGet]
        public IActionResult AllOffersPage()
        {
            try
            {
                return View("AllOffersPage");
            }
            catch (Exception e)
            {
                var model = new ErrorViewModel();
                model.ErrorMessage = e.Message;
                return View("Error", model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetAllOffers(string branchId)
        {
            try
            {
                var model = new AllOffersModel();
                model.Offers = await diia.GetOffers(branchId, null, null);
                return View("AllOffers", model);
            }
            catch (Exception e)
            {
                var model = new ErrorViewModel();
                model.ErrorMessage = e.Message;
                return View("Error", model);
            }
        }

        [HttpGet]
        public IActionResult CreateOfferPage()
        {
            try
            {
                var model = new CreateOfferModel();
                return View("CreateOffer", model);
            }
            catch (Exception e)
            {
                var model = new ErrorViewModel();
                model.ErrorMessage = e.Message;
                return View("Error", model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateOffer(CreateOfferModel offerModel)
        {
            try
            {
                var sharing = offerModel.Sharing?.Split(",")
                        .Where(x => !string.IsNullOrEmpty(x))
                        .Select(x => x.Trim())
                        .ToList();

                var offer = new Offer()
                {
                    Name = offerModel.Name.Trim(),
                    ReturnLink = offerModel.ReturnLink?.Trim(),
                    Scopes = new OfferScopes()
                    { 
                        Sharing = sharing
                    }
                };

                if (!string.IsNullOrEmpty(offerModel.DiiaId))
                {
                    offer.Scopes.DiiaId = new List<string>() { offerModel.DiiaId.Trim() };
                }

                var createdOfferId = await diia.CreateOffer(offerModel.BranchId, offer);
                if (string.IsNullOrEmpty(createdOfferId))
                { 
                    throw new Exception("Error creating offer.");
                }
                var model = new IdModel();
                model.OfferId = createdOfferId;
                model.BranchId = offerModel.BranchId;
                return View("CreateOfferSuccess", model);
            }
            catch (Exception e)
            {
                var model = new ErrorViewModel();
                model.ErrorMessage = e.Message;
                return View("Error", model);
            }
        }

        [HttpGet]
        public IActionResult DeleteOfferPage()
        {
            try
            {
                return View("DeleteOffer");
            }
            catch (Exception e)
            {
                var model = new ErrorViewModel();
                model.ErrorMessage = e.Message;
                return View("Error", model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteOffer(string branchId, string offerId)
        {
            try
            {
                await diia.DeleteOffer(branchId, offerId);
                return View("DeleteOfferSuccess");
            }
            catch (Exception e)
            {
                var model = new ErrorViewModel();
                model.ErrorMessage = e.Message;
                return View("Error", model);
            }
        }
    }
}
