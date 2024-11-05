using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using DiiaClient.SDK.Exception;
using DiiaClient.SDK.Interfaces.Remote;
using DiiaClient.SDK.Services.Remote;

namespace DiiaClient.SDK.Helper
{
    internal class HttpMethodExecutor
    {
        private static readonly MediaTypeWithQualityHeaderValue MEDIA_TYPE_JSON = new ("application/json");

        private readonly ISessionTokenService _sessionTokenServiceImpl;
        private readonly HttpClient httpClient;

        public HttpMethodExecutor(string acquirerToken, string baseDiiaUrl, HttpClient httpClient)
        {
            this.httpClient = httpClient;
            this._sessionTokenServiceImpl = new SessionTokenServiceImpl(acquirerToken, baseDiiaUrl, httpClient);
        }

        internal async Task<T> DoGet<T>(string url)
        {
            var sessionToken = await _sessionTokenServiceImpl.GetSessionToken();

            try
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get, url))
                {
                    request.Headers.Accept.Add(MEDIA_TYPE_JSON);
                    request.Headers.Add("Authorization", $"Bearer {sessionToken}");

                    using (HttpResponseMessage response = await httpClient.SendAsync(request))
                    {
                        response.EnsureSuccessStatusCode();
                        var result = await JsonSerializer.DeserializeAsync<T>(await response.Content.ReadAsStreamAsync());
                        if (result == null)
                        {
                            throw new DiiaClientException("Api call error: " + response);
                        }
                        return result;
                    }
                }
            }
            catch (System.Exception e)
            {
                throw new DiiaClientException("Api call error", e);
            }
        }

        internal async Task<T> DoPost<T>(string url, string content)
        {
            var sessionToken = await _sessionTokenServiceImpl.GetSessionToken();

            try
            {
                using (var request = new HttpRequestMessage(HttpMethod.Post, url))
                {
                    request.Headers.Accept.Add(MEDIA_TYPE_JSON);
                    request.Headers.Add("Authorization", $"Bearer {sessionToken}");
                    request.Content = new StringContent(content, Encoding.UTF8, "application/json");

                    using (HttpResponseMessage response = await httpClient.SendAsync(request))
                    {
                        //response.EnsureSuccessStatusCode();
                        var result = await JsonSerializer.DeserializeAsync<T>(await response.Content.ReadAsStreamAsync());
                        if (result == null)
                        {
                            throw new DiiaClientException("Api call error: " + response);
                        }
                        return result;
                    }
                }
            }
            catch (System.Exception e)
            {
                throw new DiiaClientException("Api call error", e);
            }
        }

        internal async Task DoDelete(string url)
        {
            var sessionToken = await _sessionTokenServiceImpl.GetSessionToken();

            try
            {
                using (var request = new HttpRequestMessage(HttpMethod.Delete, url))
                {
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
                    request.Headers.Add("Authorization", $"Bearer {sessionToken}");

                    using (HttpResponseMessage response = await httpClient.SendAsync(request))
                    {
                        response.EnsureSuccessStatusCode();
                    }
                }
            }
            catch (System.Exception e)
            {
                throw new DiiaClientException("Api call error", e);
            }
        }

        internal async Task<T> DoPut<T>(string url, string content)
        {
            var sessionToken = await _sessionTokenServiceImpl.GetSessionToken();

            try
            {
                using (var request = new HttpRequestMessage(HttpMethod.Put, url))
                {
                    request.Headers.Accept.Add(MEDIA_TYPE_JSON);
                    request.Headers.Add("Authorization", $"Bearer {sessionToken}");
                    request.Content = new StringContent(content, Encoding.UTF8, "application/json");

                    using (HttpResponseMessage response = await httpClient.SendAsync(request))
                    {
                        response.EnsureSuccessStatusCode();

                        var result = await JsonSerializer.DeserializeAsync<T>(await response.Content.ReadAsStreamAsync());
                        if (result == null)
                        {
                            throw new DiiaClientException("Api call error: " + response);
                        }
                        return result;
                    }
                }
            }
            catch (System.Exception e)
            {
                throw new DiiaClientException("Api call error", e);
            }
        }
    }
}
