using App.DL.Repository.ProjectTeamMember;
using Micron.DL.Module.Auth;
using Nancy;
using Nancy.Testing;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Tests.Testing;
using Tests.Utils.Fake.Project;
using Tests.Utils.Fake.ProjectTeamMember;
using Tests.Utils.Fake.User;

namespace Tests.App.AL.Controller.ProjectTeamMember {
    [TestFixture]
    public class ProjectTeamMemberCrudControllerTests : BaseTestFixture {
        [Test]
        public void Create_DataCorrect_ProjectTeamMemberCreated() {
            var user = UserFaker.Create();
            var project = ProjectFaker.Create(user);
            var browser = new Browser(new DefaultNancyBootstrapper());

            var result = browser.Post("/api/v1/project_team_member/create", with => {
                with.HttpRequest();
                with.Query("api_token", Jwt.FromUserId(user.id));
                with.Query("project_guid", project.guid);
                with.Query("user_guid", user.guid);
            }).Result;

            Assert.AreEqual(HttpStatusCode.Created, result.StatusCode);

            var createdTeamMember = ProjectTeamMemberRepository.Find(project, user);

            Assert.NotNull(createdTeamMember);
            Assert.AreEqual(project.id, createdTeamMember.project_id);
            Assert.AreEqual(user.id, createdTeamMember.user_id);
        }

        [Test]
        public void Delete_DataCorrect_ProjectTeamMemberDeleted() {
            var browser = new Browser(new DefaultNancyBootstrapper());

            var user = UserFaker.Create();
            
            var teamMember = ProjectTeamMemberFaker.Create();

            ProjectTeamMemberFaker.Create(teamMember.Project(), user);

            Assert.NotNull(ProjectTeamMemberRepository.Find(teamMember.Project(), teamMember.User()));

            var result = browser.Delete("/api/v1/project_team_member/delete", with => {
                with.HttpRequest();
                with.Query("api_token", Jwt.FromUserId(user.id));
                with.Query("project_guid", teamMember.Project().guid);
                with.Query("user_guid", teamMember.User().guid);
            }).Result;

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);

            Assert.IsNull(ProjectTeamMemberRepository.Find(teamMember.Project(), teamMember.User()));
        }
    }
}