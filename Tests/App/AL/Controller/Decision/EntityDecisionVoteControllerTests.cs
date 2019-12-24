using Micron.DL.Module.Auth;
using Nancy;
using Nancy.Testing;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Tests.Testing;
using Tests.Utils.Fake.Decision;
using Tests.Utils.Fake.User;

namespace Tests.App.AL.Controller.Decision {
    [TestFixture]
    public class EntityDecisionVoteControllerTests : BaseTestFixture {
        [Test]
        public void Vote_DataCorrect_VoteRecorded() {
            var user = UserFaker.Create();

            var decision = EntityDecisionFaker.Create(user);

            var option = EntityDecisionOptionFaker.Create(decision);

            Assert.IsNull(option.UserVote(user));

            var result = new Browser(new DefaultNancyBootstrapper())
                .Post("/api/v1/entity/decision/vote", with => {
                    with.HttpRequest();
                    with.Query("api_token", Jwt.FromUserId(user.id));
                    with.Query("option_guid", option.guid);
                }).Result;

            JObject.Parse(result.Body.AsString());

            Assert.NotNull(option.UserVote(user));
        }

        [Test]
        public void Vote_WithActiveVote_VoteRewritten() {
            var user = UserFaker.Create();

            var decision = EntityDecisionFaker.Create(user);

            var options = EntityDecisionOptionFaker.CreateMany(2, decision);

            EntityDecisionVoteFaker.Create(options[0], user);
            
            Assert.NotNull(options[0].UserVote(user));
            
            var result = new Browser(new DefaultNancyBootstrapper())
                .Post("/api/v1/entity/decision/vote", with => {
                    with.HttpRequest();
                    with.Query("api_token", Jwt.FromUserId(user.id));
                    with.Query("option_guid", options[1].guid);
                }).Result;
            
            JObject.Parse(result.Body.AsString());
            
            Assert.IsNull(options[0].UserVote(user));
            Assert.NotNull(options[1].UserVote(user));
        }
    }
}