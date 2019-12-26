using Micron.DL.Module.Auth;
using Micron.DL.Module.Misc;
using Nancy;
using Nancy.Testing;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Tests.Testing;
using Tests.Utils.Fake.Project;
using Tests.Utils.Fake.User;

namespace Tests.App.AL.Controller.Project {
    [TestFixture]
    public class ProjectCrudControllerTests : BaseTestFixture {
        [Test]
        public void Patch_DataCorrect_ProjectUpdated() {
            var browser = new Browser(new DefaultNancyBootstrapper());

            var project = ProjectFaker.Create();

            var updatedDescription = "updatedDescription_" + Rand.SmallInt();

            var result = browser.Patch("/api/v1/project/edit", with => {
                with.HttpRequest();
                with.Query("api_token", Jwt.FromUserId(UserFaker.Create().id));
                with.Query("project_guid", project.guid);
                with.Query("description", updatedDescription);
            }).Result;
            
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            
            var json = JObject.Parse(result.Body.AsString());
            
            Assert.AreEqual(project.guid, json["data"]["project"].Value<string>("guid"));
            Assert.AreEqual(updatedDescription, json["data"]["project"].Value<string>("description"));
        }
    }
}