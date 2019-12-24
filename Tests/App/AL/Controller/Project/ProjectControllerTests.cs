using Nancy;
using Nancy.Testing;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Tests.Testing;
using Tests.Utils.Fake.Project;

namespace Tests.App.AL.Controller.Project {
    [TestFixture]
    public class ProjectControllerTests : BaseTestFixture {
        [Test]
        public void Get_DataCorrect_GotProject() {
            var browser = new Browser(new DefaultNancyBootstrapper());

            var project = ProjectFaker.Create();

            var result = browser.Get("/api/v1/project/get", with => {
                with.HttpRequest();
                with.Query("project_guid", project.guid);
            }).Result;

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);

            var json = JObject.Parse(result.Body.AsString());

            Assert.AreEqual(project.guid, json["data"]["project"].Value<string>("guid"));
        }
    }
}