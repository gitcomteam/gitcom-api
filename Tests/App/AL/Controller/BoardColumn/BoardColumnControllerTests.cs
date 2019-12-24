using Nancy;
using Nancy.Testing;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Tests.Testing;
using Tests.Utils.Fake.Board;
using Tests.Utils.Fake.BoardColumn;

namespace Tests.App.AL.Controller.BoardColumn {
    [TestFixture]
    public class BoardControllerTests : BaseTestFixture {
        [Test]
        public void Get_DataCorrect_GotBoardColumn() {
            var browser = new Browser(new DefaultNancyBootstrapper());

            var boardColumn = BoardColumnFaker.Create();

            var result = browser.Get("/api/v1/board_column/get", with => {
                with.HttpRequest();
                with.Query("board_column_guid", boardColumn.guid);
            }).Result;

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);

            var json = JObject.Parse(result.Body.AsString());

            Assert.AreEqual(boardColumn.guid, json["data"]["board_column"].Value<string>("guid"));
        }
    }
}