using System.Net.Http.Headers;
using System.Text.Json;
using DiiaClient.SDK.Exception;
using DiiaClient.SDK.Interfaces.Remote;
using DiiaClient.SDK.Models.Remote;

namespace DiiaClient.SDK.Services.Remote
{
    public class SessionTokenServiceImpl : ISessionTokenService
    {
        private static readonly long SESSION_TOKEN_TIME_TO_LIVE = 2 * 3600 * 1000L;

        private volatile string sessionToken;
        private long sessionTokenObtainTime = 0L;

        private readonly string baseDiiaUrl;
        private readonly string acquirerToken;
        private readonly HttpClient httpClient;

        public SessionTokenServiceImpl(string acquirerToken, string baseDiiaUrl, HttpClient httpClient)
        {
            this.acquirerToken = acquirerToken;
            this.baseDiiaUrl = baseDiiaUrl;
            this.httpClient = httpClient;
        }

        public async Task<string> GetSessionToken()
        {
            long now = DateTime.Now.Millisecond;
            if (SESSION_TOKEN_TIME_TO_LIVE >= now - sessionTokenObtainTime)
            {
                lock (this)
                {
                    sessionToken = ObtainSessionToken().Result;
                    sessionTokenObtainTime = now;
                }
            }
            return sessionToken;
        }

        /// <summary>
        /// curl -X GET "https://{diia_host}/api/v1/auth/acquirer/{acquirer_token}"
        /// -H  "accept: application/json" -H "Authorization: Basic {auth_acquirer_token}"
        /// </summary>
        /// <returns>sessionToken</returns>
        private async Task<string> ObtainSessionToken()
        {
            try
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get, $"{baseDiiaUrl}/api/v1/auth/acquirer/{acquirerToken}"))
                {
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    request.Headers.Add("Authorization", $"Basic {acquirerToken}");

                    using (HttpResponseMessage response = await httpClient.SendAsync(request))
                    {
                        response.EnsureSuccessStatusCode();
                        var token = await JsonSerializer.DeserializeAsync<SessionToken>(await response.Content.ReadAsStreamAsync());
                        if (token == null || string.IsNullOrEmpty(token.Token))
                        {
                            throw new DiiaClientException("Authentication error: " + response);
                        }
                        return token.Token;
                    }
                }
            }
            catch (System.Exception e)
            {
                throw new DiiaClientException("Authentication error", e);
            }
        }
    }
}
