using Nancy;
using Nancy.Testing;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Tests.Testing;
using Tests.Utils.Fake.Board;

namespace Tests.App.AL.Controller.Board {
    [TestFixture]
    public class BoardControllerTests : BaseTestFixture {
        [Test]
        public void Get_DataCorrect_GotBoard() {
            var browser = new Browser(new DefaultNancyBootstrapper());

            var board = BoardFaker.Create();

            var result = browser.Get("/api/v1/board/get", with => {
                with.HttpRequest();
                with.Query("board_guid", board.guid);
            }).Result;

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);

            var json = JObject.Parse(result.Body.AsString());

            Assert.AreEqual(board.guid, json["data"]["board"].Value<string>("guid"));
        }
    }
}