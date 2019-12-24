using Micron.DL.Module.Auth;
using Nancy;
using Nancy.Testing;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Tests.Testing;
using Tests.Utils.Fake.Card;
using Tests.Utils.Fake.User;

namespace Tests.App.AL.Controller.Work.Card {
    public class CardWorkCrudControllerTests : BaseTestFixture {
        [Test]
        public void SubmitWork_DataCorrect_WorkCreated() {
            var card = CardFaker.Create();

            var project = card.Column().Board().Project();

            var workType = project.WorkTypes()[0];

            var result = new Browser(new DefaultNancyBootstrapper())
                .Post("/api/v1/card/work/submit", with => {
                    with.HttpRequest();
                    with.Query("api_token", Jwt.FromUserId(UserFaker.Create().id));
                    with.Query("card_guid", card.guid);
                    with.Query("work_type_guid", workType.guid);
                    with.Query("proof", "some proof");
                }).Result;

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);

            var json = JObject.Parse(result.Body.AsString());
            
            Assert.AreEqual(card.guid, json["data"]["work_item"].Value<string>("card_guid"));
        }
    }
}