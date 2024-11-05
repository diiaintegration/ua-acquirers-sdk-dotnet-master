using DiiaClient.SDK.Models.Local;
using DiiaClient.SDK.Models.Remote;

namespace DiiaClient.SDK.Interfaces.Local
{
    internal interface IDocumentService
    {
        //Processing documents pack: decrypt, check sign
        DocumentPackage ProcessDocumentPackage(Dictionary<string, string> headers, List<EncodedFile> multipartBody, string encodedJsonData);

    }
}
