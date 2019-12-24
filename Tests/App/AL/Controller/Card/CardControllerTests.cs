using Nancy;
using Nancy.Testing;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Tests.Testing;
using Tests.Utils.Fake.Card;
using Tests.Utils.Fake.BoardColumn;

namespace Tests.App.AL.Controller.Card {
    [TestFixture]
    public class CardControllerTests : BaseTestFixture {
        [Test]
        public void Get_DataCorrect_GotCard() {
            var browser = new Browser(new DefaultNancyBootstrapper());

            var card = CardFaker.Create();

            var result = browser.Get("/api/v1/card/get", with => {
                with.HttpRequest();
                with.Query("card_guid", card.guid);
            }).Result;

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);

            var json = JObject.Parse(result.Body.AsString());

            Assert.AreEqual(card.guid, json["data"]["card"].Value<string>("guid"));
        }
    }
}