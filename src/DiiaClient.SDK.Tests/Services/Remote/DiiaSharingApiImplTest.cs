using Xunit;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using System.Threading.Tasks;
using DiiaClient.SDK.Tests.Helper;
using System.Linq;
using Newtonsoft.Json.Linq;
using FluentAssertions;
using System.Threading;

namespace DiiaClient.SDK.Tests.Services.Remote
{
    [Collection("Server collection")]
    public class DiiaSharingApiImplTest
    {
        ServerFixture fixture;

        public DiiaSharingApiImplTest(ServerFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        async Task TestRequestDocumentByBarCodeSuccess()
        {
            string responseBody = "{ \"success\": true }";
            fixture.server.Given(
                Request.Create().WithPath("/api/v1/acquirers/document-request").UsingPost()
            )
            .RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json; charset=utf-8")
                    .WithBody(responseBody)
            );

            var requestId = "bff98b1cff5bd2aad838";
            var barcode = "NWU4ZGVjY2NiNjc5MGQ3M2FkODM4YzVk";
            var branchId = "1d441d305adc2bff98b1cff5bd2aebecb43f71f25";
            var requestBody = string.Format("{3}\"branchId\": \"{0}\",\"barcode\": \"{1}\", \"requestId\": \"{2}\"{4}",
                branchId, barcode, requestId, "{", "}");
            Thread.Sleep(1000);
            var isOk = await fixture.diiaApi.RequestDocumentByBarCode(branchId, barcode, requestId);

            var logsEntries = fixture.server.FindLogEntries(Request.Create().WithPath("/api/v1/acquirers/document-request").UsingPost());
            var recordedRequest = logsEntries.First().RequestMessage;

            var actualBody = recordedRequest.BodyAsJson.ToString();
            JToken expected = JsonHelper.RemoveEmptyChildren(JToken.Parse(requestBody));
            JToken actual = JsonHelper.RemoveEmptyChildren(JToken.Parse(actualBody));
            actual.Should().BeEquivalentTo(expected);
            Assert.True(isOk);
            Assert.Equal(string.Format("bearer {0}", fixture.ACCESS_TOKEN), recordedRequest.Headers["Authorization"][0], true);
            Assert.Equal("application/json", recordedRequest.Headers["Accept"][0], true);
            Assert.Equal("application/json; charset=utf-8", recordedRequest.Headers["Content-Type"][0], true);
            Assert.Equal("POST", recordedRequest.Method, true);
            Assert.Equal("/api/v1/acquirers/document-request", recordedRequest.AbsolutePath, true);
        }


        [Fact]
        async Task TestRequestDocumentByBarCodeFail()
        {
            string responseBody = "{ \"success\": false }";
            fixture.server.Given(
                Request.Create().WithPath("/api/v1/acquirers/document-request").UsingPost()
            )
            .RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json; charset=utf-8")
                    .WithBody(responseBody)
            );

            var requestId = "bff98b1cff5bd2aad838";
            var barcode = "NWU4ZGVjY2NiNjc5MGQ3M2FkODM4YzVk";
            var branchId = "1d441d305adc2bff98b1cff5bd2aebecb43f71f25";
            var requestBody = string.Format("{3}\"branchId\": \"{0}\",\"barcode\": \"{1}\", \"requestId\": \"{2}\"{4}",
                branchId, barcode, requestId, "{", "}");

            var isOk = await fixture.diiaApi.RequestDocumentByBarCode(branchId, barcode, requestId);

            var logsEntries = fixture.server.FindLogEntries(Request.Create().WithPath("/api/v1/acquirers/document-request").UsingPost());
            var recordedRequest = logsEntries.First().RequestMessage;

            var actualBody = recordedRequest.BodyAsJson.ToString();
            JToken expected = JsonHelper.RemoveEmptyChildren(JToken.Parse(requestBody));
            JToken actual = JsonHelper.RemoveEmptyChildren(JToken.Parse(actualBody));
            actual.Should().BeEquivalentTo(expected);
            Assert.False(isOk);
            Assert.Equal(string.Format("bearer {0}", fixture.ACCESS_TOKEN), recordedRequest.Headers["Authorization"][0], true);
            Assert.Equal("application/json", recordedRequest.Headers["Accept"][0], true);
            Assert.Equal("application/json; charset=utf-8", recordedRequest.Headers["Content-Type"][0], true);
            Assert.Equal("POST", recordedRequest.Method, true);
            Assert.Equal("/api/v1/acquirers/document-request", recordedRequest.AbsolutePath, true);
        }

        [Fact]
        async Task TestGetDeepLink()
        {
            string responseBody = "{\"deeplink\": \"https://diia.app/acquirers/offer-request/dynamic/9954\"}";
            fixture.server.Given(
                Request.Create().WithPath("/api/v2/acquirers/branch/*/offer-request/dynamic").UsingPost()
            )
            .RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json; charset=utf-8")
                    .WithBody(responseBody)
            );

            var branchId = "1d441d305adc2bff98b1cff5bd2aebecb43f71f25";
            var offerId = "5e8decccb6790d73ad838c5d";
            var requestId = "bff98b1cff5bd2aad838";
            var requestBody = string.Format("{2}\"offerId\": \"{0}\", \"requestId\": \"{1}\"{3}", offerId, requestId, "{", "}");

            var deepLink = await fixture.diiaApi.GetDeepLink(branchId, offerId, requestId);

            var logsEntries = fixture.server.FindLogEntries(Request.Create().WithPath("/api/v2/acquirers/branch/*/offer-request/dynamic").UsingPost());
            var recordedRequest = logsEntries.First().RequestMessage;

            var actualBody = recordedRequest.BodyAsJson.ToString();
            JToken expected = JsonHelper.RemoveEmptyChildren(JToken.Parse(requestBody));
            JToken actual = JsonHelper.RemoveEmptyChildren(JToken.Parse(actualBody));
            actual.Should().BeEquivalentTo(expected);
            Assert.Equal("https://diia.app/acquirers/offer-request/dynamic/9954", deepLink);
            Assert.Equal(string.Format("bearer {0}", fixture.ACCESS_TOKEN), recordedRequest.Headers["Authorization"][0], true);
            Assert.Equal("application/json", recordedRequest.Headers["Accept"][0], true);
            Assert.Equal("application/json; charset=utf-8", recordedRequest.Headers["Content-Type"][0], true);
            Assert.Equal("POST", recordedRequest.Method, true);
            Assert.Equal(string.Format("/api/v2/acquirers/branch/{0}/offer-request/dynamic", branchId), recordedRequest.AbsolutePath, true);
        }
    }
}
