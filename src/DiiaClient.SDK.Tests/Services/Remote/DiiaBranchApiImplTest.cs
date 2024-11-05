using DiiaClient.SDK.Models.Remote;
using Xunit;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using System.Threading.Tasks;
using DiiaClient.SDK.Tests.Helper;
using System.Linq;
using Newtonsoft.Json.Linq;
using FluentAssertions;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
namespace DiiaClient.SDK.Tests.Services.Remote
{
    [Collection("Server collection")]
    public class DiiaBranchApiImplTest
    {
        ServerFixture fixture;

        public DiiaBranchApiImplTest(ServerFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        async Task TestCreateBranch() {
            string responseBody = "{ \"_id\": \"5e8decccb92d8d73ad838c5d\" }";
            fixture.server.Given(
                Request.Create().WithPath("/api/v2/acquirers/branch").UsingPost()
            )
            .RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json; charset=utf-8")
                    .WithBody(responseBody)
            );
            Branch newBranch = StubsHelper.loadAsObject<Branch>(Properties.Resources.new_branch);
            var expectedBody = StubsHelper.LoadAsString(Properties.Resources.new_branch);

            var branch = await fixture.diiaApi.CreateBranch(newBranch);

            var logsEntries = fixture.server.FindLogEntries(Request.Create().WithPath("/api/v2/acquirers/branch").UsingPost());
            var recordedRequest = logsEntries.First().RequestMessage;

            var actualBody = recordedRequest.BodyAsJson.ToString();
            JToken expected = JsonHelper.RemoveEmptyChildren(JToken.Parse(expectedBody));
            JToken actual = JsonHelper.RemoveEmptyChildren(JToken.Parse(actualBody));
            actual.Should().BeEquivalentTo(expected);
            Assert.Equal("5e8decccb92d8d73ad838c5d", branch.Id);
            //Assert.Equal(expectedBody, actualBody, true);
            Assert.Equal(string.Format("bearer {0}", fixture.ACCESS_TOKEN), recordedRequest.Headers["Authorization"][0], true);
            Assert.Equal("application/json", recordedRequest.Headers["Accept"][0], true);
            Assert.Equal("application/json; charset=utf-8", recordedRequest.Headers["Content-Type"][0], true);
            Assert.Equal("POST", recordedRequest.Method, true);
            Assert.Equal("/api/v2/acquirers/branch", recordedRequest.AbsolutePath, true);
        }

        [Fact]
        async Task TestGetBranchById()
        {
            string responseBody = StubsHelper.LoadAsString(Properties.Resources.existing_branch);
            fixture.server.Given(
                Request.Create().WithPath("/api/v2/acquirers/branch/*").UsingGet()
            )
            .RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json; charset=utf-8")
                    .WithBody(responseBody)
            );
            Branch expectedBranch = StubsHelper.loadAsObject<Branch>(Properties.Resources.existing_branch);
            var branchId = "1d441d305adc2bff982fde159f8c91706a68a80b1cff5bd2aebecb43f71f25";

            Branch actual = await fixture.diiaApi.GetBranch(branchId);

            var logsEntries = fixture.server.FindLogEntries(Request.Create().WithPath("/api/v2/acquirers/branch/*").UsingGet());
            var recordedRequest = logsEntries.First().RequestMessage;

            actual.Should().BeEquivalentTo(expectedBranch);
            Assert.Equal(string.Format("bearer {0}", fixture.ACCESS_TOKEN), recordedRequest.Headers["Authorization"][0], true);
            Assert.Equal("application/json", recordedRequest.Headers["Accept"][0], true);
            Assert.Equal("GET", recordedRequest.Method, true);
            Assert.Equal(string.Format("/api/v2/acquirers/branch/{0}", branchId), recordedRequest.AbsolutePath, true);
        }

        [Fact]
        async Task TestDeleteBranchById()
        {
            fixture.server.Given(
                Request.Create().WithPath("/api/v2/acquirers/branch/*").UsingDelete()
            )
            .RespondWith(
                Response.Create()
                    .WithStatusCode(200)
            );
            var branchId = "1d441d305adc2bff982fde159f8c91706a68a80b1cff5bd2aebecb43f71f25";

            await fixture.diiaApi.DeleteBranch(branchId);

            var logsEntries = fixture.server.FindLogEntries(Request.Create().WithPath("/api/v2/acquirers/branch/*").UsingDelete());
            var recordedRequest = logsEntries.First().RequestMessage;

            Assert.Equal(string.Format("bearer {0}", fixture.ACCESS_TOKEN), recordedRequest.Headers["Authorization"][0], true);
            Assert.Equal("*/*", recordedRequest.Headers["Accept"][0], true);
            Assert.Equal("DELETE", recordedRequest.Method, true);
            Assert.Equal(string.Format("/api/v2/acquirers/branch/{0}", branchId), recordedRequest.AbsolutePath, true);
        }

        [Fact]
        async Task TestGetBranchList()
        {
            string responseBody = StubsHelper.LoadAsString(Properties.Resources.branch_list);
            fixture.server.Given(
                Request.Create().WithPath("/api/v2/acquirers/branches").UsingGet()
            )
            .RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json; charset=utf-8")
                    .WithBody(responseBody)
            );
            BranchList expected = StubsHelper.loadAsObject<BranchList>(Properties.Resources.branch_list);

            BranchList actual = await fixture.diiaApi.GetBranches(null, null);

            var logsEntries = fixture.server.FindLogEntries(Request.Create().WithPath("/api/v2/acquirers/branches").UsingGet());
            var recordedRequest = logsEntries.Last().RequestMessage;

            actual.Should().BeEquivalentTo(expected);
            Assert.Equal(string.Format("bearer {0}", fixture.ACCESS_TOKEN), recordedRequest.Headers["Authorization"][0], true);
            Assert.Equal("application/json", recordedRequest.Headers["Accept"][0], true);
            Assert.Equal("GET", recordedRequest.Method, true);
            Assert.Equal("/api/v2/acquirers/branches", recordedRequest.AbsolutePath, true);
        }

        [Fact]
        async Task TestGetBranchListSkipQuery()
        {
            string responseBody = StubsHelper.LoadAsString(Properties.Resources.branch_list);
            fixture.server.Given(
                Request.Create().WithPath("/api/v2/acquirers/branches").UsingGet()
            )
            .RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json; charset=utf-8")
                    .WithBody(responseBody)
            );
            BranchList expected = StubsHelper.loadAsObject<BranchList>(Properties.Resources.branch_list);
            long skipValue = 3L;
            BranchList actual = await fixture.diiaApi.GetBranches(skipValue, null);

            var logsEntries = fixture.server.FindLogEntries(Request.Create().WithPath("/api/v2/acquirers/branches").UsingGet());
            var recordedRequest = logsEntries.Last().RequestMessage;

            //actual.Should().BeEquivalentTo(expected);
            Assert.Equal(string.Format("bearer {0}", fixture.ACCESS_TOKEN), recordedRequest.Headers["Authorization"][0], true);
            Assert.Equal("application/json", recordedRequest.Headers["Accept"][0], true);
            Assert.Equal("GET", recordedRequest.Method, true);
            Assert.NotNull(recordedRequest.AbsolutePath);
            Assert.Equal(skipValue.ToString(), recordedRequest.Query["skip"][0], true);
        }

        [Fact]
        async Task TestGetBranchListLimitQuery()
        {
            string responseBody = StubsHelper.LoadAsString(Properties.Resources.branch_list);
            fixture.server.Given(
                Request.Create().WithPath("/api/v2/acquirers/branches").UsingGet()
            )
            .RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json; charset=utf-8")
                    .WithBody(responseBody)
            );
            BranchList expected = StubsHelper.loadAsObject<BranchList>(Properties.Resources.branch_list);
            long limitValue = 75555L;
            BranchList actual = await fixture.diiaApi.GetBranches(null, limitValue);

            var logsEntries = fixture.server.FindLogEntries(Request.Create().WithPath("/api/v2/acquirers/branches").UsingGet());
            var recordedRequest = logsEntries.Last().RequestMessage;

            //actual.Should().BeEquivalentTo(expected);
            Assert.Equal(string.Format("bearer {0}", fixture.ACCESS_TOKEN), recordedRequest.Headers["Authorization"][0], true);
            Assert.Equal("application/json", recordedRequest.Headers["Accept"][0], true);
            Assert.Equal("GET", recordedRequest.Method, true);
            Assert.NotNull(recordedRequest.AbsolutePath);
            Assert.Equal(limitValue.ToString(), recordedRequest.Query["limit"][0], true);
        }

        [Fact]
        async Task TestUpdateBranch()
        {
            string responseBody = "{ \"_id\": \"1d441d305adc2bff982fde159f8c91706a68a80b1cff5bd2aebecb43f71f25\" }";
            fixture.server.Given(
                Request.Create().WithPath("/api/v2/acquirers/branch/*").UsingPut()
            )
            .RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json; charset=utf-8")
                    .WithBody(responseBody)
            );
            Branch newBranch = StubsHelper.loadAsObject<Branch>(Properties.Resources.existing_branch);
            var expectedBody = StubsHelper.LoadAsString(Properties.Resources.existing_branch);

            await fixture.diiaApi.UpdateBranch(newBranch);

            var logsEntries = fixture.server.FindLogEntries(Request.Create().WithPath("/api/v2/acquirers/branch/*").UsingPut());
            var recordedRequest = logsEntries.First().RequestMessage;

            var actualBody = recordedRequest.BodyAsJson.ToString();
            JToken expected = JsonHelper.RemoveEmptyChildren(JToken.Parse(expectedBody));
            JToken actual = JsonHelper.RemoveEmptyChildren(JToken.Parse(actualBody));
            actual.Should().BeEquivalentTo(expected);
            //Assert.Equal(expectedBody, actualBody, true);
            Assert.Equal(string.Format("bearer {0}", fixture.ACCESS_TOKEN), recordedRequest.Headers["Authorization"][0], true);
            Assert.Equal("application/json", recordedRequest.Headers["Accept"][0], true);
            Assert.Equal("application/json; charset=utf-8", recordedRequest.Headers["Content-Type"][0], true);
            Assert.Equal("PUT", recordedRequest.Method, true);
            Assert.Equal(string.Format("/api/v2/acquirers/branch/{0}", newBranch.Id), recordedRequest.AbsolutePath, true);
        }
    }
}
