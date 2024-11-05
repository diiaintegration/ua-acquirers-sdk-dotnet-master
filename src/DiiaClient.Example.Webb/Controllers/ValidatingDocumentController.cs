using DiiaClient.Example.Web.Authorization;
using DiiaClient.Example.Web.Models;
using DiiaClient.SDK.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DiiaClient.Example.Web.Controllers
{
    [Authorize]
    public class ValidatingDocumentController : Controller
    {
        private readonly IDiia diia;

        public ValidatingDocumentController(IDiia diia)
        {
            this.diia = diia;
        }

        [HttpGet]
        public IActionResult ValidatingPage()
        {
            try
            {
                var model = new BarcodeModel();
                return View("ValidatingByBarcode", model);
            }
            catch (Exception e)
            {
                var model = new ErrorViewModel();
                model.ErrorMessage = e.Message;
                return View("Error", model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ValidateDocument(BarcodeModel barcodeModel)
        {
            try
            {
                var branches = await diia.GetBranches(0L, 1L);
                var branchId = branches.Branches.FirstOrDefault().Id;
                var isDocumentValid = await diia.ValidateDocumentByBarcode(branchId, barcodeModel.Barcode);
                return View(isDocumentValid ? "ValidationSuccess" : "ValidationFailed");
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
