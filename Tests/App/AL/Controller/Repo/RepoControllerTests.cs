using Nancy;
using Nancy.Testing;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Tests.Testing;
using Tests.Utils.Fake.Repo;

namespace Tests.App.AL.Controller.Repo {
    [TestFixture]
    public class RepoControllerTests : BaseTestFixture {
        [Test]
        public void Get_DataCorrect_GotRepository() {
            var browser = new Browser(new DefaultNancyBootstrapper());

            var repo = RepoFaker.Create();

            var result = browser.Get("/api/v1/repository/get", with => {
                with.HttpRequest();
                with.Query("repo_guid", repo.guid);
            }).Result;

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);

            var json = JObject.Parse(result.Body.AsString());

            Assert.AreEqual(repo.guid, json["data"]["repository"].Value<string>("guid"));
        }
    }
}