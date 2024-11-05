namespace DiiaClient.SDK.Interfaces.Remote
{
    public interface IDiiaValidationApi
    {
        Task<bool> ValidateDocumentByBarcode(string branchId, string barcode);
    }
}
