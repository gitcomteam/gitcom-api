using System;
using App.DL.Repository.BoardColumn;
using Micron.DL.Module.Auth;
using Micron.DL.Module.Misc;
using Nancy;
using Nancy.Testing;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Tests.Testing;
using Tests.Utils.Fake.BoardColumn;
using Tests.Utils.Fake.User;
using Tests.Utils.Fake.Board;

namespace Tests.App.AL.Controller.BoardColumn {
    [TestFixture]
    public class BoardCrudControllerTests : BaseTestFixture {
        [Test]
        public void Create_DataCorrect_BoardColumnCreated() {
            var user = UserFaker.Create();
            var board = BoardFaker.Create(user);
            var browser = new Browser(new DefaultNancyBootstrapper());

            var boardColumnTitle = "testBoard" + Rand.SmallInt();
            var boardOrder = Convert.ToString(Rand.IntRange(0, 25));

            var result = browser.Post("/api/v1/board_column/create", with => {
                with.HttpRequest();
                with.Query("api_token", Jwt.FromUserId(user.id));
                with.Query("name", boardColumnTitle);
                with.Query("board_guid", board.guid);
                with.Query("board_order", boardOrder);
            }).Result;

            Assert.AreEqual(HttpStatusCode.Created, result.StatusCode);

            var json = JObject.Parse(result.Body.AsString());

            var guid = json["data"]["board_column"].Value<string>("guid") ?? "";

            var createdBoardColumn = BoardColumnRepository.FindByGuid(guid);

            Assert.NotNull(createdBoardColumn);
            Assert.AreEqual(boardColumnTitle, createdBoardColumn.name);
            Assert.AreEqual(boardOrder, Convert.ToString(createdBoardColumn.board_order));
            Assert.AreEqual(
                createdBoardColumn.guid, json["data"]["board_column"].Value<string>("guid") ?? ""
            );
        }

        [Test]
        public void Patch_DataCorrect_BoardUpdated() {
            var browser = new Browser(new DefaultNancyBootstrapper());
            
            var me = UserFaker.Create();
            var boardColumn = BoardColumnFaker.Create(me);

            var updatedName = "updatedName_" + Rand.SmallInt();
            var updatedBoardOrder = Convert.ToString(Rand.IntRange(0, 40));

            var result = browser.Patch("/api/v1/board_column/edit", with => {
                with.HttpRequest();
                with.Query("api_token", Jwt.FromUserId(me.id));
                with.Query("board_column_guid", boardColumn.guid);
                with.Query("name", updatedName);
                with.Query("board_order", updatedBoardOrder);
            }).Result;
            
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            
            var json = JObject.Parse(result.Body.AsString());
            
            Assert.AreEqual(boardColumn.guid, json["data"]["board_column"].Value<string>("guid"));
            Assert.AreEqual(updatedName, json["data"]["board_column"].Value<string>("name"));
            Assert.AreEqual(updatedBoardOrder, json["data"]["board_column"].Value<string>("board_order"));
        }

        [Test]
        public void Delete_DataCorrect_BoardColumnDeleted() {
            var browser = new Browser(new DefaultNancyBootstrapper());

            var me = UserFaker.Create();
            var boardColumn = BoardColumnFaker.Create(me);

            Assert.NotNull(BoardColumnRepository.Find(boardColumn.id));

            var result = browser.Delete("/api/v1/board_column/delete", with => {
                with.HttpRequest();
                with.Query("api_token", Jwt.FromUserId(me.id));
                with.Query("board_column_guid", boardColumn.guid);
            }).Result;
            
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            
            Assert.IsNull(BoardColumnRepository.Find(boardColumn.id));
        }
    }
}