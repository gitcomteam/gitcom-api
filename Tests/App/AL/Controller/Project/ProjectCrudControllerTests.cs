using App.DL.Repository.Project;
using Micron.DL.Module.Auth;
using Micron.DL.Module.Misc;
using Nancy;
using Nancy.Testing;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Tests.Testing;
using Tests.Utils.DB;
using Tests.Utils.Fake.Project;
using Tests.Utils.Fake.User;
using Tests.Utils.Fake.Repo;

namespace Tests.App.AL.Controller.Project {
    [TestFixture]
    public class ProjectCrudControllerTests : BaseTestFixture {
        [Test]
        public void Create_DataCorrect_ProjectCreated() {
            var user = UserFaker.Create();
            var repository = RepoFaker.Create();
            var browser = new Browser(new DefaultNancyBootstrapper());

            var projectTitle = "testProject" + Rand.SmallInt();

            var result = browser.Post("/api/v1/project/create", with => {
                with.HttpRequest();
                with.Query("api_token", Jwt.FromUserId(user.id));
                with.Query("name", projectTitle);
                with.Query("creator_guid", user.guid);
                with.Query("repository_guid", repository.guid);
            }).Result;

            Assert.AreEqual(HttpStatusCode.Created, result.StatusCode);

            var json = JObject.Parse(result.Body.AsString());

            var guid = json["data"]["project"].Value<string>("guid") ?? "";

            var createdProject = ProjectRepository.FindByGuid(guid);

            Assert.NotNull(createdProject);
            Assert.AreEqual(projectTitle, createdProject.name);
            Assert.AreEqual(
                createdProject.guid, json["data"]["project"].Value<string>("guid") ?? ""
            );
        }

        [Test]
        public void Patch_DataCorrect_ProjectUpdated() {
            var browser = new Browser(new DefaultNancyBootstrapper());

            var project = ProjectFaker.Create();

            var updatedName = "updatedName_" + Rand.SmallInt();

            var result = browser.Patch("/api/v1/project/edit", with => {
                with.HttpRequest();
                with.Query("api_token", Jwt.FromUserId(UserFaker.Create().id));
                with.Query("project_guid", project.guid);
                with.Query("name", updatedName);
            }).Result;

            var body = result.Body.AsString();
            
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            
            var json = JObject.Parse(result.Body.AsString());
            
            Assert.AreEqual(project.guid, json["data"]["project"].Value<string>("guid"));
            Assert.AreEqual(updatedName, json["data"]["project"].Value<string>("name"));
        }

        [Test]
        public void Delete_DataCorrect_ProjectDeleted() {
            var browser = new Browser(new DefaultNancyBootstrapper());

            var project = ProjectFaker.Create();
            
            Assert.NotNull(ProjectRepository.FindByGuid(project.guid));

            var result = browser.Delete("/api/v1/project/delete", with => {
                with.HttpRequest();
                with.Query("api_token", Jwt.FromUserId(UserFaker.Create().id));
                with.Query("project_guid", project.guid);
            }).Result;
            
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            
            Assert.IsNull(ProjectRepository.FindByGuid(project.guid));
        }
    }
}