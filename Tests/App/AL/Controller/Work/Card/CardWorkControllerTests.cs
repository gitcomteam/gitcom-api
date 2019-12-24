using Nancy;
using Nancy.Testing;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Tests.Testing;
using Tests.Utils.Fake.Card;
using Tests.Utils.Fake.Work;

namespace Tests.App.AL.Controller.Work.Card {
    public class CardWorkControllerTests : BaseTestFixture {
        [Test]
        public void GetWork_DataCorrect_GotWork() {
            var card = CardFaker.Create();

            var project = card.Column().Board().Project();
            
            var cardWork = CardWorkFaker.Create(project.WorkTypes()[0], card);
            
            var result = new Browser(new DefaultNancyBootstrapper())
                .Get("/api/v1/card/work/get", with => {
                    with.HttpRequest();
                    with.Query("card_guid", card.guid);
                }).Result;

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);

            var json = JObject.Parse(result.Body.AsString());

            var workItems = json["data"].Value<JArray>("work_items");
            
            Assert.NotNull(workItems);
            
            Assert.AreEqual(cardWork.guid, workItems[0].Value<string>("guid"));
        }
    }
}