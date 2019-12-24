using App.AL.Utils.Entity;
using App.DL.Enum;
using Nancy;
using Nancy.Testing;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Tests.Testing;
using Tests.Utils.Fake.Decision;

namespace Tests.App.AL.Controller.Decision {
    [TestFixture]
    public class EntityDecisionControllerTests : BaseTestFixture {
        [Test]
        public void GetActive_DataCorrect_GotData() {
            var decision = EntityDecisionFaker.Create();

            Assert.NotNull(decision);

            var entityGuid = EntityUtils.GetEntityGuid(decision.entity_id, decision.entity_type);

            var result = new Browser(new DefaultNancyBootstrapper())
                .Get("/api/v1/entity/decisions/active/get", with => {
                    with.HttpRequest();
                    with.Query("entity_guid", entityGuid);
                    with.Query("entity_type", EntityType.Project.ToString());
                }).Result;

            var body = JObject.Parse(result.Body.AsString());

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);

            Assert.IsNotEmpty(body["data"]["decisions"].ToString());
        }
    }
}