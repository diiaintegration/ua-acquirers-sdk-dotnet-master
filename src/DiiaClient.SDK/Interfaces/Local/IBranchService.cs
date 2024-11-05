using DiiaClient.SDK.Models.Remote;

namespace DiiaClient.SDK.Interfaces.Local
{
    internal interface IBranchService
    {
        Task<BranchList> GetBranches(long? skip, long? limit);
        Task<Branch> GetBranch(string branchId);
        Task DeleteBranch(string branchId);
        Task<Branch> CreateBranch(Branch request);
        Task<Branch> UpdateBranch(Branch request);
    }
}
