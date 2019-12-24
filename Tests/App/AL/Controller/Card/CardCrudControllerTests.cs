using System;
using App.DL.Repository.Card;
using Micron.DL.Module.Auth;
using Micron.DL.Module.Misc;
using Nancy;
using Nancy.Testing;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Tests.Testing;
using Tests.Utils.Fake.BoardColumn;
using Tests.Utils.Fake.User;
using Tests.Utils.Fake.Card;

namespace Tests.App.AL.Controller.Card {
    [TestFixture]
    public class BoardCrudControllerTests : BaseTestFixture {
        [Test]
        public void Create_DataCorrect_CardCreated() {
            var user = UserFaker.Create();
            var column = BoardColumnFaker.Create(user);
            var browser = new Browser(new DefaultNancyBootstrapper());

            var cardTitle = "testCard" + Rand.SmallInt();
            var description = "descriptionCard" + Rand.SmallInt();
            var columnOrder = Convert.ToString(Rand.IntRange(1, 25));

            var result = browser.Post("/api/v1/card/create", with => {
                with.HttpRequest();
                with.Query("api_token", Jwt.FromUserId(user.id));
                with.Query("name", cardTitle);
                with.Query("description", description);
                with.Query("column_guid", column.guid);
                with.Query("column_order", columnOrder);
                with.Query("creator_guid", user.guid);
            }).Result;

            Assert.AreEqual(HttpStatusCode.Created, result.StatusCode);

            var json = JObject.Parse(result.Body.AsString());

            var guid = json["data"]["card"].Value<string>("guid") ?? "";

            var createdCard = CardRepository.FindByGuid(guid);

            Assert.NotNull(createdCard);
            Assert.AreEqual(cardTitle, createdCard.name);
            Assert.AreEqual(columnOrder, Convert.ToString(createdCard.column_order));
            Assert.AreEqual(
                createdCard.guid, json["data"]["card"].Value<string>("guid") ?? ""
            );
        }

        [Test]
        public void Patch_DataCorrect_CardUpdated() {
            var browser = new Browser(new DefaultNancyBootstrapper());
            
            var me = UserFaker.Create();
            var card = CardFaker.Create(me);

            var updatedName = "updatedName_" + Rand.SmallInt();
            var updatedDescription = "updatedName_" + Rand.SmallInt();
            var updatedColumnOrder = Convert.ToString(Rand.IntRange(0, 40));

            var result = browser.Patch("/api/v1/card/edit", with => {
                with.HttpRequest();
                with.Query("api_token", Jwt.FromUserId(me.id));
                with.Query("card_guid", card.guid);
                with.Query("name", updatedName);
                with.Query("description", updatedDescription);
                with.Query("column_order", updatedColumnOrder);
            }).Result;
            
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            
            var json = JObject.Parse(result.Body.AsString());
            
            Assert.AreEqual(card.guid, json["data"]["card"].Value<string>("guid"));
            Assert.AreEqual(updatedName, json["data"]["card"].Value<string>("name"));
            Assert.AreEqual(updatedColumnOrder, json["data"]["card"].Value<string>("column_order"));
        }

        [Test]
        public void Delete_DataCorrect_CardDeleted() {
            var browser = new Browser(new DefaultNancyBootstrapper());

            var me = UserFaker.Create();
            var card = CardFaker.Create(me);

            Assert.NotNull(CardRepository.Find(card.id));

            var result = browser.Delete("/api/v1/card/delete", with => {
                with.HttpRequest();
                with.Query("api_token", Jwt.FromUserId(me.id));
                with.Query("card_guid", card.guid);
            }).Result;
            
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            
            Assert.IsNull(CardRepository.Find(card.id));
        }
    }
}