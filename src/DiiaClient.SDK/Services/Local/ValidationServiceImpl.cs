using DiiaClient.SDK.Interfaces.Local;
using DiiaClient.SDK.Interfaces.Remote;

namespace DiiaClient.SDK.Services.Local
{
    internal class ValidationServiceImpl : IValidationService
    {
        private readonly IDiiaValidationApi diiaValidationApi;

        public ValidationServiceImpl(IDiiaValidationApi diiaValidationApi)
        {
            this.diiaValidationApi = diiaValidationApi;
        }

        public async Task<bool> ValidateDocumentByBarcode(string branchId, string barcode)
        {
            return await diiaValidationApi.ValidateDocumentByBarcode(branchId, barcode);
        }
    }
}
