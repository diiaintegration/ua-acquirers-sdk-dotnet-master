using Microsoft.AspNetCore.Mvc;
using DiiaClient.SDK.Interfaces;
using DiiaClient.SDK.Models.Remote;
using DiiaClient.Example.Web.Models;
using DiiaClient.Example.Web.Authorization;

namespace DiiaClient.Exemple.Web.Controllers
{
    [Authorize]
    public class BranchController : Controller
    {
        private readonly IDiia diia;

        public BranchController(IDiia diia)
        {
            this.diia = diia;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBranches()
        {
            try
            {
                var model = new AllBranchesModel();
                model.Branches = await diia.GetBranches(0, 10);
                return View("AllBranches", model);
            }
            catch (Exception e)
            {
                var model = new ErrorViewModel();
                model.ErrorMessage = e.Message + e.InnerException;
                return View("Error", model);
            }
        }

        [HttpGet]
        public IActionResult CreateBranchPage()
        {
            try
            {
                var model = new CreateBranchModel();
                return View("CreateBranch", model);
            }
            catch (Exception e)
            {
                var model = new ErrorViewModel();
                model.ErrorMessage = e.Message;
                return View("Error", model);
            }          
        }

        [HttpPost]
        public async Task<IActionResult> CreateBranch(CreateBranchModel branchModel)
        {
            try
            {
                Branch branch = new Branch();
                branch.Name = branchModel.Name;
                branch.Email = branchModel.Email;
                branch.Region = branchModel.Region;
                branch.District = branchModel.District;
                branch.Location = branchModel.Location;
                branch.Street = branchModel.Street;
                branch.House = branchModel.House;
                branch.CustomFullName = branchModel.CustomFullName;
                branch.CustomFullAddress = branchModel.CustomFullAddress;
                branch.OfferRequestType = "dynamic";
                branch.DeliveryTypes = new List<string>() { "api" };

                BranchScopes scopes = new BranchScopes();
                scopes.Sharing = branchModel.Sharing?.Split(",")
                        .Where(x => !string.IsNullOrEmpty(x))
                        .Select(x => x.Trim())
                        .ToList();
                scopes.DocumentIdentification = branchModel.DocumentIdentification?.Split(",")
                        .Where(x => !string.IsNullOrEmpty(x))
                        .Select(x => x.Trim())
                        .ToList();
                scopes.DiiaId = branchModel.DiiaId?.Split(",")
                    .Where(x => !string.IsNullOrEmpty(x))
                    .Select(x => x.Trim())
                    .ToList();

                branch.Scopes = scopes;

                var createdBranch = await diia.CreateBranch(branch);
                if (string.IsNullOrEmpty(createdBranch?.Id))
                {
                    throw new Exception("Error creating branch.");
                }
                var model = new IdModel();
                model.BranchId = createdBranch.Id;
                return View("CreateBranchSuccess", model);
            }
            catch (Exception e)
            {
                var model = new ErrorViewModel();
                model.ErrorMessage = e.Message;
                return View("Error", model);
            }
        }

        public IActionResult GetBranchPage()
        {
            try
            {
                return View("GetBranch");
            }
            catch (Exception e)
            {
                var model = new ErrorViewModel();
                model.ErrorMessage = e.Message;
                return View("Error", model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetBranch(string branchId)
        {
            try
            {
                var model = new BranchModel();
                model.Branch = await diia.GetBranch(branchId);
                return View("GetBranchSuccess", model);
            }
            catch (Exception e)
            {
                var model = new ErrorViewModel();
                model.ErrorMessage = e.Message;
                return View("Error", model);
            }
        }

        [HttpGet]
        public IActionResult DeleteBranchPage()
        {
            try
            {
                return View("DeleteBranch");
            }
            catch (Exception e)
            {
                var model = new ErrorViewModel();
                model.ErrorMessage = e.Message;
                return View("Error", model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteBranch(string branchId)
        {
            try
            {
                await diia.DeleteBranch(branchId);
                return View("DeleteBranchSuccess");
            }
            catch (Exception e)
            {
                var model = new ErrorViewModel();
                model.ErrorMessage = e.Message;
                return View("Error", model);
            }
        }

        [HttpGet]
        public IActionResult UpdateBranchPage()
        {
            try
            {
                return View("UpdateBranch", new CreateBranchModel());
            }
            catch (Exception e)
            {
                var model = new ErrorViewModel();
                model.ErrorMessage = e.Message;
                return View("Error", model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateBranch(CreateBranchModel branchModel)
        {
            try
            {
                Branch branch = new Branch
                {
                    Id = branchModel.Id,
                    Name = branchModel.Name,
                    Email = branchModel.Email,
                    Region = branchModel.Region,
                    District = branchModel.District,
                    Location = branchModel.Location,
                    Street = branchModel.Street,
                    House = branchModel.House,
                    CustomFullName = branchModel.CustomFullName,
                    CustomFullAddress = branchModel.CustomFullAddress,
                    OfferRequestType = "dynamic",
                    DeliveryTypes = new List<string>() { "api" }
                };

                BranchScopes scopes = new BranchScopes();
                scopes.Sharing = branchModel.Sharing?.Split(",")
                        .Where(x => !string.IsNullOrEmpty(x))
                        .Select(x => x.Trim())
                        .ToList();
                scopes.DocumentIdentification = branchModel.DocumentIdentification?.Split(",")
                        .Where(x => !string.IsNullOrEmpty(x))
                        .Select(x => x.Trim())
                        .ToList();
                scopes.DiiaId = branchModel.DiiaId?.Split(",")
                    .Where(x => !string.IsNullOrEmpty(x))
                    .Select(x => x.Trim())
                    .ToList();

                branch.Scopes = scopes;
                var model = new BranchModel();
                model.Branch = await diia.UpdateBranch(branch);
                model.Branch = await diia.GetBranch(model.Branch.Id);
                return View("UpdateBranchSuccess", model);
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
