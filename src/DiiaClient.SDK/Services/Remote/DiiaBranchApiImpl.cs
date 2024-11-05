using System.Collections.Specialized;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using DiiaClient.SDK.Exception;
using DiiaClient.SDK.Helper;
using DiiaClient.SDK.Interfaces.Remote;
using DiiaClient.SDK.Models.Remote;

namespace DiiaClient.SDK.Services.Remote
{
    internal class DiiaBranchApiImpl : IDiiaBranchApi
    {
        private readonly string baseDiiaUrl;
        private readonly HttpMethodExecutor httpMethodExecutor;

        public DiiaBranchApiImpl(string baseDiiaUrl, HttpMethodExecutor httpMethodExecutor)
        {
            this.baseDiiaUrl = baseDiiaUrl;
            this.httpMethodExecutor = httpMethodExecutor;
        }

        /*
            curl -X POST "{diia_host}/api/v2/acquirers/branch"
            -H "accept: application/json"
            -H "Authorization: Bearer {session_token}"
            -H "Content-Type: application/json"
            -d "{\"customFullName\":\"Повна назва запитувача документа\", \"customFullAddress\":\"Повна адреса відділення\",
            \"name\":\"Назва відділення\", \"email\":\"test@email.com\", \"region\":\"Київська обл.\",
            \"district\":\"Києво-Святошинський р-н\", \"location\":\"м. Вишневе\", \"street\":\"вул. Київська\",
            \"house\":\"2г\", \"deliveryTypes\": [\"api\"], \"offerRequestType\": \"dynamic\",
            \"scopes\":{\"sharing\":[\"passport\",\"internal passport\",\"foreign-passport\"], \"identification\":[],
            \"documentIdentification\":[\"internal-passport\",\"foreign passport\"]}}"
         */
        public async Task<string> CreateBranch(Branch request)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                };
                var content = JsonSerializer.Serialize(request, options); 
                return (await httpMethodExecutor.DoPost<Id>($"{baseDiiaUrl}/api/v2/acquirers/branch", content)).ID;
            }
            catch (System.Exception e)
            {
                throw new DiiaClientException("Branch creation error", e);
            }
        }

        /*
            curl -X DELETE "{diia_host}/api/v2/acquirers/branch/{branch_id}"
            -H "Accept: *//*"
            -H "Authorization: Bearer {session_token}"
            -H "Content-Type: application/json"
        */
        public async Task<Branch> GetBranchById(string branchId)
        {
            try
            {
                return await httpMethodExecutor.DoGet<Branch>($"{baseDiiaUrl}/api/v2/acquirers/branch/{branchId}");
            }
            catch (System.Exception e)
            {
                throw new DiiaClientException("Get branch error", e);
            }
        }

        /*
            curl -X DELETE "{diia_host}/api/v2/acquirers/branch/{branch_id}"
            -H "Accept: *//*"
            -H "Authorization: Bearer {session_token}"
            -H "Content-Type: application/json"
        */
        public async Task DeleteBranchById(string branchId)
        {
            try
            {
                await httpMethodExecutor.DoDelete($"{baseDiiaUrl}/api/v2/acquirers/branch/{branchId}");
            }
            catch (System.Exception e)
            {
                throw new DiiaClientException("Delete branch error", e);
            }
        }

        /*
            curl -X GET "{diia_host}/api/v2/acquirers/branches?skip=0&limit=2"
            -H "accept: application/json"
            -H "Authorization: Bearer {session_token}"
        */
        public async Task<BranchList> GetBranches(long? skip, long? limit)
        {
            try
            {
                NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(String.Empty);
                queryString.Add("skip", skip.ToString());
                queryString.Add("limit", limit.ToString());

                return await httpMethodExecutor.DoGet<BranchList>($"{baseDiiaUrl}/api/v2/acquirers/branches?{queryString}");
            }
            catch (System.Exception e)
            {
                throw new DiiaClientException("Get branches error", e);
            }
        }

        /*
           curl -X PUT "{diia_host}/api/v2/acquirers/branch/{branch_id}"
           -H "Accept: application/json"
           -H "Authorization: Bearer {session_token}"
           -H "Content-Type: application/json"
           -d "{\"customFullName\":\"Повна назва запитувача документа\", \"customFullAddress\":\"Повна адреса відділення\",
           \"name\":\"Назва відділення\", \"email\":\"test@email.com\", \"region\":\"Київська обл.\",
           \"district\":\"Києво-Святошинський р-н\", \"location\":\"м. Вишневе\", \"street\":\"вул. Київська\",
           \"house\":\"2г\", \"deliveryTypes\": [\"api\"], \"offerRequestType\": \"dynamic\",
           \"scopes\":{\"sharing\":[\"passport\",\"internal passport\",\"foreign-passport\"], \"identification\":[],
           \"documentIdentification\":[\"internal-passport\",\"foreign passport\"]}}"
        */
        public async Task<Branch> UpdateBranch(Branch branch)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic)
                };
                var content = JsonSerializer.Serialize(branch, options);
                return await httpMethodExecutor.DoPut<Branch>($"{baseDiiaUrl}/api/v2/acquirers/branch/{branch.Id}", content);
            }
            catch (System.Exception e)
            {
                throw new DiiaClientException("Update branch error", e);
            }
        }
    }
}
