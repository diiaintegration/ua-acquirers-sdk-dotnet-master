namespace DiiaClient.SDK.Interfaces.Local
{
    internal interface IValidationService
    {
        Task<bool> ValidateDocumentByBarcode(string branchId, string barcode);
    }
}
