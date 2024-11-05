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
    public class DiiaValidationApiImplTest
    {
        ServerFixture fixture;

        public DiiaValidationApiImplTest(ServerFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        async Task TestRequestDocumentByBarCodeSuccess()
        {
            
            string responseBody = "{ \"success\": true }";
            fixture.server.Given(
                Request.Create().WithPath("/api/v1/acquirers/document-identification").UsingPost()
            )
            .RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json; charset=utf-8")
                    .WithBody(responseBody)
            );

            var barcode = "NWU4ZGVjY2NiNjc5MGQ3M2FkODM4YzVk";
            var branchId = "1d441d305adc2bff98b1cff5bd2aebecb43f71f25";
            var requestBody = string.Format("{2}\"branchId\": \"{0}\",\"barcode\": \"{1}\"{3}",
                branchId, barcode, "{", "}");
            Thread.Sleep(1000);
            var isOk = await fixture.diiaApi.ValidateDocumentByBarcode(branchId, barcode);

            var logsEntries = fixture.server.FindLogEntries(Request.Create().WithPath("/api/v1/acquirers/document-identification").UsingPost());
            var recordedRequest = logsEntries.Last().RequestMessage;

            var actualBody = recordedRequest.BodyAsJson.ToString();
            JToken expected = JsonHelper.RemoveEmptyChildren(JToken.Parse(requestBody));
            JToken actual = JsonHelper.RemoveEmptyChildren(JToken.Parse(actualBody));
            actual.Should().BeEquivalentTo(expected);
            Assert.True(isOk);
            Assert.Equal(string.Format("bearer {0}", fixture.ACCESS_TOKEN), recordedRequest.Headers["Authorization"][0], true);
            Assert.Equal("application/json", recordedRequest.Headers["Accept"][0], true);
            Assert.Equal("application/json; charset=utf-8", recordedRequest.Headers["Content-Type"][0], true);
            Assert.Equal("POST", recordedRequest.Method, true);
            Assert.Equal("/api/v1/acquirers/document-identification", recordedRequest.AbsolutePath, true);
        }


        [Fact]
        async Task TestRequestDocumentByBarCodeFail()
        {
            string responseBody = "{ \"success\": false }";
            fixture.server.Given(
                Request.Create().WithPath("/api/v1/acquirers/document-identification").UsingPost()
            )
            .RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json; charset=utf-8")
                    .WithBody(responseBody)
            );

            var barcode = "NWU4ZGVjY2NiNjc5MGQ3M2FkODM4YzVk";
            var branchId = "1d441d305adc2bff98b1cff5bd2aebecb43f71f25";
            var requestBody = string.Format("{2}\"branchId\": \"{0}\",\"barcode\": \"{1}\"{3}",
                branchId, barcode, "{", "}");

            var isOk = await fixture.diiaApi.ValidateDocumentByBarcode(branchId, barcode);

            var logsEntries = fixture.server.FindLogEntries(Request.Create().WithPath("/api/v1/acquirers/document-identification").UsingPost());
            var recordedRequest = logsEntries.Last().RequestMessage;

            var actualBody = recordedRequest.BodyAsJson.ToString();
            JToken expected = JsonHelper.RemoveEmptyChildren(JToken.Parse(requestBody));
            JToken actual = JsonHelper.RemoveEmptyChildren(JToken.Parse(actualBody));
            actual.Should().BeEquivalentTo(expected);
            Assert.False(isOk);
            Assert.Equal(string.Format("bearer {0}", fixture.ACCESS_TOKEN), recordedRequest.Headers["Authorization"][0], true);
            Assert.Equal("application/json", recordedRequest.Headers["Accept"][0], true);
            Assert.Equal("application/json; charset=utf-8", recordedRequest.Headers["Content-Type"][0], true);
            Assert.Equal("POST", recordedRequest.Method, true);
            Assert.Equal("/api/v1/acquirers/document-identification", recordedRequest.AbsolutePath, true);
        }       
    }
}
