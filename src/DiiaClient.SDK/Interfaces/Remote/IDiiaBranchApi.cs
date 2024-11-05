using DiiaClient.SDK.Models.Remote;

namespace DiiaClient.SDK.Interfaces.Remote
{
    public interface IDiiaBranchApi
    {
        Task<string> CreateBranch(Branch request);
        Task<Branch> GetBranchById(string branchId);
        Task DeleteBranchById(string branchId);
        Task<BranchList> GetBranches(long? skip, long? limit);
        Task<Branch> UpdateBranch(Branch branch);
    }
}
