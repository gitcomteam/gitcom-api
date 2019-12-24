using App.DL.Enum;
using Nancy;
using Nancy.Testing;
using NUnit.Framework;
using Tests.Testing;
using Tests.Utils.Fake.Card;
using Tests.Utils.Fake.Funding;

namespace Tests.App.AL.Controller.Funding.Balance {
    public class FundingBalanceControllerTests : BaseTestFixture {
        [Test]
        public void Get_DataCorrect_GotCardBalances() {
            var card = CardFaker.Create();

            FundingBalanceFaker.Create(card.id, EntityType.Card, CurrencyType.Ethereum, 0.2M);
            FundingBalanceFaker.Create(card.id, EntityType.Card, CurrencyType.BitCoin);
            
            var result = new Browser(new DefaultNancyBootstrapper())
                .Get("/api/v1/entity/funding/balances/get", with => {
                    with.HttpRequest();
                    with.Query("entity_guid", card.guid);
                    with.Query("entity_type", EntityType.Card.ToString());
                }).Result;
            
            Assert.AreEqual(HttpStatusCode.OK,result.StatusCode);
        }
    }
}