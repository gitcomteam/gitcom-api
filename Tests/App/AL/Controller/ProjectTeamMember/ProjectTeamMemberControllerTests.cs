using Nancy;
using Nancy.Testing;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Tests.Testing;
using Tests.Utils.Fake.ProjectTeamMember;

namespace Tests.App.AL.Controller.ProjectTeamMember {
    [TestFixture]
    public class ProjectTeamMemberControllerTests : BaseTestFixture {
        [Test]
        public void Get_DataCorrect_GotProjectTeamMember() {
            var browser = new Browser(new DefaultNancyBootstrapper());

            var teamMember = ProjectTeamMemberFaker.Create();

            var result = browser.Get("/api/v1/project_team_member/get", with => {
                with.HttpRequest();
                with.Query("project_guid", teamMember.Project().guid);
                with.Query("user_guid", teamMember.User().guid);
            }).Result;

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);

            var json = JObject.Parse(result.Body.AsString());

            Assert.AreEqual(teamMember.Project().guid,
                json["data"]["project_team_member"].Value<string>("project_guid"));
        }
    }
}