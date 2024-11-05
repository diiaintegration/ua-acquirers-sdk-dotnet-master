using DiiaClient.SDK.Models.Remote;
using Xunit;
using System;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using System.Threading.Tasks;
using DiiaClient.SDK.Tests.Helper;
using System.Linq;
using Newtonsoft.Json.Linq;
using FluentAssertions;

namespace DiiaClient.SDK.Tests.Services.Remote
{
    [Collection("Server collection")]
    public class DiiaOfferApiImplTest
    {
        ServerFixture fixture;

        public DiiaOfferApiImplTest(ServerFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        async Task TestCreateOffer()
        {
            string responseBody = "{ \"_id\": \"5e8decccb92d8d73ad838c5d\" }";
            fixture.server.Given(
                Request.Create().WithPath("/api/v1/acquirers/branch/*/offer").UsingPost()
            )
            .RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json; charset=utf-8")
                    .WithBody(responseBody)
            );
            Offer newOffer = StubsHelper.loadAsObject<Offer>(Properties.Resources.new_offer);
            var expectedBody = StubsHelper.LoadAsString(Properties.Resources.new_offer);
            string branchId = "1d441d305adc2bff98b1cff5bd2aebecb43f71f25";
            var offerId = await fixture.diiaApi.CreateOffer(branchId, newOffer);

            var logsEntries = fixture.server.FindLogEntries(Request.Create().WithPath("/api/v1/acquirers/branch/*/offer").UsingPost());
            var recordedRequest = logsEntries.First().RequestMessage;

            var actualBody = recordedRequest.BodyAsJson.ToString();
            JToken expected = JsonHelper.RemoveEmptyChildren(JToken.Parse(expectedBody));
            JToken actual = JsonHelper.RemoveEmptyChildren(JToken.Parse(actualBody));
            actual.Should().BeEquivalentTo(expected);
            Assert.Equal("5e8decccb92d8d73ad838c5d", offerId);
            //Assert.Equal(expectedBody, actualBody, true);
            Assert.Equal(string.Format("bearer {0}", fixture.ACCESS_TOKEN), recordedRequest.Headers["Authorization"][0], true);
            Assert.Equal("application/json", recordedRequest.Headers["Accept"][0], true);
            Assert.Equal("application/json; charset=utf-8", recordedRequest.Headers["Content-Type"][0], true);
            Assert.Equal("POST", recordedRequest.Method, true);
            Assert.Equal(String.Format("/api/v1/acquirers/branch/{0}/offer", branchId), recordedRequest.AbsolutePath, true);
        }


        [Fact]
        async Task TestDeleteOffer()
        {
            fixture.server.Given(
                Request.Create().WithPath("/api/v1/acquirers/branch/*/offer/*").UsingDelete()
            )
            .RespondWith(
                Response.Create()
                    .WithStatusCode(200)
            );
            var branchId = "1d441d305adc2bff98b1cff5bd2aebecb43f71f25";
            var offerId = "5e8decccb6790d73ad838c5d";
            await fixture.diiaApi.DeleteOffer(branchId, offerId);

            var logsEntries = fixture.server.FindLogEntries(Request.Create().WithPath("/api/v1/acquirers/branch/*/offer/*").UsingDelete());
            var recordedRequest = logsEntries.First().RequestMessage;

            Assert.Equal(string.Format("bearer {0}", fixture.ACCESS_TOKEN), recordedRequest.Headers["Authorization"][0], true);
            Assert.Equal("*/*", recordedRequest.Headers["Accept"][0], true);
            Assert.Equal("DELETE", recordedRequest.Method, true);
            Assert.Equal(string.Format("/api/v1/acquirers/branch/{0}/offer/{1}", branchId, offerId), recordedRequest.AbsolutePath, true);
        }

        [Fact]
        async Task TestGetOfferList()
        {
            string responseBody = StubsHelper.LoadAsString(Properties.Resources.offer_list);
            fixture.server.Given(
                Request.Create().WithPath("/api/v1/acquirers/branch/*/offers").UsingGet()
            )
            .RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json; charset=utf-8")
                    .WithBody(responseBody)
            );
            OfferList expected = StubsHelper.loadAsObject<OfferList>(Properties.Resources.offer_list);
            var branchId = "1d441d305adc2bff98b1cff5bd2aebecb43f71f25";
            OfferList actual = await fixture.diiaApi.GetOffers(branchId, null, null);

            var logsEntries = fixture.server.FindLogEntries(Request.Create().WithPath("/api/v1/acquirers/branch/*/offers").UsingGet());
            var recordedRequest = logsEntries.First().RequestMessage;

            actual.Should().BeEquivalentTo(expected);
            Assert.Equal(string.Format("bearer {0}", fixture.ACCESS_TOKEN), recordedRequest.Headers["Authorization"][0], true);
            Assert.Equal("application/json", recordedRequest.Headers["Accept"][0], true);
            Assert.Equal("GET", recordedRequest.Method, true);
            Assert.Equal(string.Format("/api/v1/acquirers/branch/{0}/offers", branchId), recordedRequest.AbsolutePath, true);
        }

        [Fact]
        async Task TestGetOfferListSkipQuery()
        {
            string responseBody = StubsHelper.LoadAsString(Properties.Resources.offer_list);
            fixture.server.Given(
                Request.Create().WithPath("/api/v1/acquirers/branch/*/offers").UsingGet()
            )
            .RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json; charset=utf-8")
                    .WithBody(responseBody)
            );
            OfferList expected = StubsHelper.loadAsObject<OfferList>(Properties.Resources.offer_list);
            var branchId = "1d441d305adc2bff98b1cff5bd2aebecb43f71f25";
            long skipValue = 3L;
            OfferList actual = await fixture.diiaApi.GetOffers(branchId, skipValue, null);

            var logsEntries = fixture.server.FindLogEntries(Request.Create().WithPath("/api/v1/acquirers/branch/*/offers").UsingGet());
            var recordedRequest = logsEntries.Last().RequestMessage;

            actual.Should().BeEquivalentTo(expected);
            Assert.Equal(string.Format("bearer {0}", fixture.ACCESS_TOKEN), recordedRequest.Headers["Authorization"][0], true);
            Assert.Equal("application/json", recordedRequest.Headers["Accept"][0], true);
            Assert.Equal("GET", recordedRequest.Method, true);
            Assert.Equal(skipValue.ToString(), recordedRequest.Query["skip"][0], true);
        }

        [Fact]
        async Task TestGetOfferListLimitQuery()
        {
            string responseBody = StubsHelper.LoadAsString(Properties.Resources.offer_list);
            fixture.server.Given(
                Request.Create().WithPath("/api/v1/acquirers/branch/*/offers").UsingGet()
            )
            .RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json; charset=utf-8")
                    .WithBody(responseBody)
            );
            OfferList expected = StubsHelper.loadAsObject<OfferList>(Properties.Resources.offer_list);
            var branchId = "1d441d305adc2bff98b1cff5bd2aebecb43f71f25";
            long limitValue = 75555L;
            OfferList actual = await fixture.diiaApi.GetOffers(branchId, null, limitValue);

            var logsEntries = fixture.server.FindLogEntries(Request.Create().WithPath("/api/v1/acquirers/branch/*/offers").UsingGet());
            var recordedRequest = logsEntries.Last().RequestMessage;

            actual.Should().BeEquivalentTo(expected);
            Assert.Equal(string.Format("bearer {0}", fixture.ACCESS_TOKEN), recordedRequest.Headers["Authorization"][0], true);
            Assert.Equal("application/json", recordedRequest.Headers["Accept"][0], true);
            Assert.Equal("GET", recordedRequest.Method, true);
            Assert.Equal(limitValue.ToString(), recordedRequest.Query["limit"][0], true);
        }
    }
}
