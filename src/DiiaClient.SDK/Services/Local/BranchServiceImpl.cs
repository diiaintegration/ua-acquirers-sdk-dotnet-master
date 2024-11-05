using DiiaClient.SDK.Interfaces.Local;
using DiiaClient.SDK.Interfaces.Remote;
using DiiaClient.SDK.Models.Remote;

namespace DiiaClient.SDK.Services.Local
{
    internal class BranchServiceImpl : IBranchService
    {
        private readonly IDiiaBranchApi diiaBranchApi;

        public BranchServiceImpl(IDiiaBranchApi diiaBranchApi)
        {
            this.diiaBranchApi = diiaBranchApi;
        }

        public async Task<BranchList> GetBranches(long? skip, long? limit)
        {
            return await diiaBranchApi.GetBranches(skip, limit);
        }

        public async Task<Branch> GetBranch(string branchId)
        {
            return await diiaBranchApi.GetBranchById(branchId);
        }

        public async Task DeleteBranch(string branchId)
        {
            await diiaBranchApi.DeleteBranchById(branchId);
        }

        public async Task<Branch> CreateBranch(Branch request)
        {
            string branch = await diiaBranchApi.CreateBranch(request);
            request.Id = branch;
            return request;
        }

        public async Task<Branch> UpdateBranch(Branch request)
        {
            return await diiaBranchApi.UpdateBranch(request);
        }
    }
}
