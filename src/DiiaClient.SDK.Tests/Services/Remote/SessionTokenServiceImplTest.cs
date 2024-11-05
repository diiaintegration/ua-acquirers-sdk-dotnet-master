using DiiaClient.SDK.Services.Remote;
using WireMock.Server;
using System.Net.Http;
using Xunit;
using System;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using System.Threading.Tasks;
using System.Linq;

namespace DiiaClient.SDK.Tests.Services.Remote
{
    public class SessionTokenServiceImplTest : SessionTokenServiceImplTestsBase
    {

        [Fact]
        async Task TestGetSessionToken()
        {
            string responseBody = "{ \"token\": \"bXi124jbs3cFas...Sjf\" }";
            server.Given(
                Request.Create().WithPath("/api/v1/auth/acquirer/*").UsingGet()
            )
            .RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json; charset=utf-8")
                    .WithBody(responseBody)
            );

            var sessionToken = await sessionTokenService.GetSessionToken();
            Assert.True(server.LogEntries.ToList().Count > 0);

            var logsEntries = server.FindLogEntries(Request.Create().WithPath("/api/v1/auth/acquirer/*").UsingGet());
            var recordedRequest = logsEntries.First().RequestMessage;


            Assert.Equal("bXi124jbs3cFas...Sjf", sessionToken);
            Assert.Equal(string.Format("basic {0}", ACQUIRER_TOKEN), recordedRequest.Headers["Authorization"][0], true);
            Assert.Equal("application/json", recordedRequest.Headers["Accept"][0], true);
            Assert.Equal("GET", recordedRequest.Method, true);
            Assert.Equal(string.Format("/api/v1/auth/acquirer/{0}", ACQUIRER_TOKEN), recordedRequest.AbsolutePath, true);
        }

    }

    public abstract class SessionTokenServiceImplTestsBase : IDisposable
    {
        protected readonly static string ACCESS_TOKEN = "ACCESS_TOKEN";
        protected readonly static string ACQUIRER_TOKEN = "ACQUIRER_TOKEN";
        protected readonly static string AUTH_ACQUIRER_TOKEN = "AUTH_ACQUIRER_TOKEN";
        protected static WireMockServer server;
        protected SessionTokenServiceImpl sessionTokenService;
        protected SessionTokenServiceImplTestsBase()
        {
            server = WireMockServer.Start(5559);
            string baseUrl = server.Url.ToString();
            //baseUrl = baseUrl.Substring(0, baseUrl.Length - 1);
            HttpClient httpClient = new HttpClient();
            sessionTokenService = new SessionTokenServiceImpl(ACQUIRER_TOKEN, AUTH_ACQUIRER_TOKEN, baseUrl, httpClient);

        }

        public void Dispose()
        {
            server.Stop();
        }
    }
}
