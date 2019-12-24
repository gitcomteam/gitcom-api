using App.DL.Enum;
using App.DL.Repository.Repo;
using Micron.DL.Module.Auth;
using Micron.DL.Module.Misc;
using Nancy;
using Nancy.Testing;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Tests.Testing;
using Tests.Utils.Fake.Repo;
using Tests.Utils.Fake.User;

namespace Tests.App.AL.Controller.Repo {
    [TestFixture]
    public class RepoCrudControllerTests : BaseTestFixture {
        [Test]
        public void Create_DataCorrect_RepoCreated() {
            var user = UserFaker.Create();
            var browser = new Browser(new DefaultNancyBootstrapper());

            var repoTitle = "testRepo" + Rand.SmallInt();

            var result = browser.Post("/api/v1/repository/create", with => {
                with.HttpRequest();
                with.Query("api_token", Jwt.FromUserId(user.id));
                with.Query("title", repoTitle);
                with.Query("repo_url", "randomUrl" + Rand.SmallInt());
                with.Query("service_type", RepoServiceType.GitHub.ToString());
            }).Result;

            Assert.AreEqual(HttpStatusCode.Created, result.StatusCode);

            var json = JObject.Parse(result.Body.AsString());

            var guid = json["data"]["repository"].Value<string>("guid") ?? "";

            var createdRepository = RepoRepository.FindByGuid(guid);

            Assert.NotNull(createdRepository);
            Assert.AreEqual(repoTitle, createdRepository.title);
            Assert.AreEqual(
                user.guid, json["data"]["repository"]["creator"].Value<string>("guid") ?? ""
            );
        }

        [Test]
        public void Patch_DataCorrect_RepositoryUpdated() {
            var browser = new Browser(new DefaultNancyBootstrapper());

            var repo = RepoFaker.Create();

            var updatedTitle = "updatedTitle_" + Rand.SmallInt();
            var updatedRepoUrl = "https://github.com/someuser/repo_" + Rand.SmallInt();
            
            var result = browser.Patch("/api/v1/repository/edit", with => {
                with.HttpRequest();
                with.Query("api_token", Jwt.FromUserId(UserFaker.Create().id));
                with.Query("repo_guid", repo.guid);
                with.Query("title", updatedTitle);
                with.Query("repo_url", updatedRepoUrl);
            }).Result;
            
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);

            var json = JObject.Parse(result.Body.AsString());
            
            Assert.AreEqual(repo.guid, json["data"]["repository"].Value<string>("guid"));
            Assert.AreEqual(updatedTitle, json["data"]["repository"].Value<string>("title"));
            Assert.AreEqual(updatedRepoUrl, json["data"]["repository"].Value<string>("repo_url"));
        }

        [Test]
        public void Delete_DataCorrect_RepositoryDeleted() {
            var browser = new Browser(new DefaultNancyBootstrapper());

            var repo = RepoFaker.Create();

            var result = browser.Delete("/api/v1/repository/delete", with => {
                with.HttpRequest();
                with.Query("api_token", Jwt.FromUserId(UserFaker.Create().id));
                with.Query("repo_guid", repo.guid);
            }).Result;
            
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);

            var json = JObject.Parse(result.Body.AsString());
            
            Assert.AreEqual(repo.guid, json["data"]["repository"].Value<string>("guid"));
            
            Assert.IsNull(RepoRepository.FindByGuid(repo.guid));
        }
    }
}