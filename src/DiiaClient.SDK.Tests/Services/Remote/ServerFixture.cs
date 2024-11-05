using DiiaClient.SDK.Interfaces;
using System;
using System.Net.Http;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using Xunit;

namespace DiiaClient.SDK.Tests.Services.Remote
{
    public class ServerFixture : IDisposable
    {
        public string ACCESS_TOKEN { get; private set; } = "ACCESS_TOKEN";
        protected readonly static string ACQUIRER_TOKEN = "ACQUIRER_TOKEN";
        protected readonly static string AUTH_ACQUIRER_TOKEN = "AUTH_ACQUIRER_TOKEN";
        public WireMockServer server { get; private set; }
        public IDiia diiaApi { get; private set; }

        public ServerFixture()
        {
            if (server == null || !server.IsStarted)
            {
                server = WireMockServer.Start(5555);
                //baseUrl = baseUrl.Substring(0, baseUrl.Length - 1);
                server.Given(
                    Request.Create().WithPath($"/api/v1/auth/acquirer/{ACQUIRER_TOKEN}").UsingGet()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "application/json; charset=utf-8")
                        .WithBody($"{{\"token\": \"{ACCESS_TOKEN}\"}}")
                );
                var cryptoService = new CryptoService.UAPKI.CryptoService("resources\\config\\config.json");
                string baseUrl = server.Url.ToString();
                HttpClient httpClient = new HttpClient();
                diiaApi = new Diia(ACQUIRER_TOKEN, AUTH_ACQUIRER_TOKEN, baseUrl, httpClient, cryptoService);
            }
        }

        public void Dispose()
        {
            server.Stop();
        }
    }

    [CollectionDefinition("Server collection")]
    public class ServerCollection : ICollectionFixture<ServerFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
