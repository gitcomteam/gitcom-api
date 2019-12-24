using App.DL.Repository.Board;
using Micron.DL.Module.Auth;
using Micron.DL.Module.Misc;
using Nancy;
using Nancy.Testing;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Tests.Testing;
using Tests.Utils.Fake.Project;
using Tests.Utils.Fake.User;
using Tests.Utils.Fake.Board;

namespace Tests.App.AL.Controller.Board {
    [TestFixture]
    public class BoardCrudControllerTests : BaseTestFixture {
        [Test]
        public void Create_DataCorrect_BoardCreated() {
            var creator = UserFaker.Create();
            var project = ProjectFaker.Create(creator);
            var browser = new Browser(new DefaultNancyBootstrapper());

            var boardTitle = "testBoard" + Rand.SmallInt();
            var boardDescription = "testDescription" + Rand.SmallInt();

            var result = browser.Post("/api/v1/board/create", with => {
                with.HttpRequest();
                with.Query("api_token", Jwt.FromUserId(creator.id));
                with.Query("name", boardTitle);
                with.Query("description", boardDescription);
                with.Query("creator_guid", creator.guid);
                with.Query("project_guid", project.guid);
            }).Result;
            
            Assert.AreEqual(HttpStatusCode.Created, result.StatusCode);

            var json = JObject.Parse(result.Body.AsString());

            var guid = json["data"]["board"].Value<string>("guid") ?? "";

            var createdBoard = BoardRepository.FindByGuid(guid);

            Assert.NotNull(createdBoard);
            Assert.AreEqual(boardTitle, createdBoard.name);
            Assert.AreEqual(boardDescription, createdBoard.description);
            Assert.AreEqual(
                createdBoard.guid, json["data"]["board"].Value<string>("guid") ?? ""
            );
        }

        [Test]
        public void Patch_DataCorrect_BoardUpdated() {
            var browser = new Browser(new DefaultNancyBootstrapper());
            
            var me = UserFaker.Create();
            var board = BoardFaker.Create(me);

            var updatedName = "updatedName_" + Rand.SmallInt();
            var updatedDescription = "updatedDescription_" + Rand.SmallInt();

            var result = browser.Patch("/api/v1/board/edit", with => {
                with.HttpRequest();
                with.Query("api_token", Jwt.FromUserId(me.id));
                with.Query("board_guid", board.guid);
                with.Query("name", updatedName);
                with.Query("description", updatedDescription);
            }).Result;
            
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            
            var json = JObject.Parse(result.Body.AsString());
            
            Assert.AreEqual(board.guid, json["data"]["board"].Value<string>("guid"));
            Assert.AreEqual(updatedName, json["data"]["board"].Value<string>("name"));
            Assert.AreEqual(updatedDescription, json["data"]["board"].Value<string>("description"));
        }

        [Test]
        public void Delete_DataCorrect_BoardDeleted() {
            var browser = new Browser(new DefaultNancyBootstrapper());

            var me = UserFaker.Create();
            var board = BoardFaker.Create(me);

            Assert.NotNull(BoardRepository.Find(board.id));

            var result = browser.Delete("/api/v1/board/delete", with => {
                with.HttpRequest();
                with.Query("api_token", Jwt.FromUserId(me.id));
                with.Query("board_guid", board.guid);
            }).Result;
            
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            
            Assert.IsNull(BoardRepository.Find(board.id));
        }
    }
}