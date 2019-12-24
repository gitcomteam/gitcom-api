using System;
using System.Globalization;
using App.DL.Enum;
using App.DL.Repository.Decision;
using Micron.DL.Module.Auth;
using Nancy;
using Nancy.Testing;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Tests.Testing;
using Tests.Utils.Fake.Decision;
using Tests.Utils.Fake.Project;
using Tests.Utils.Fake.User;

namespace Tests.App.AL.Controller.Decision {
    public class EntityDecisionCrudControllerTests : BaseTestFixture {
        [Test]
        public void Create_DataCorrect_DecisionCreated() {
            var user = UserFaker.Create();

            var project = ProjectFaker.Create();

            var result = new Browser(new DefaultNancyBootstrapper())
                .Post("/api/v1/entity/decision/create", with => {
                    with.HttpRequest();
                    with.Query("api_token", Jwt.FromUserId(user.id));
                    with.Query("entity_guid", project.guid);
                    with.Query("entity_type", EntityType.Project.ToString());
                    with.Query("title", "test title");
                    with.Query("content", "test content here and longer than 10 characters");
                    with.Query("deadline", DateTime.Now.AddDays(2).ToString(CultureInfo.InvariantCulture));
                    with.Query("options", "['option a', 'option b']");
                }).Result;

            var body = JObject.Parse(result.Body.AsString());

            Assert.AreEqual(HttpStatusCode.Created, result.StatusCode);

            Assert.IsNotEmpty(body["data"]["decision"].ToString());

            var decision = EntityDecisionRepository.FindByGuid(body["data"]["decision"].Value<string>("guid"));

            Assert.NotNull(decision);
            Assert.AreEqual(user.id, decision.creator_id);
            Assert.AreEqual(project.id, decision.entity_id);
            
            Assert.AreEqual(2, decision.Options().Length);
        }

        [Test]
        public void Edit_DataCorrect_StatusUpdated() {
            var user = UserFaker.Create();
            var decision = EntityDecisionFaker.Create(user);

            Assert.AreEqual(DecisionStatus.Open, decision.status);
            Assert.NotNull(decision);

            var result = new Browser(new DefaultNancyBootstrapper())
                .Patch("/api/v1/entity/decision/edit", with => {
                    with.HttpRequest();
                    with.Query("api_token", Jwt.FromUserId(user.id));
                    with.Query("decision_guid", decision.guid);
                    with.Query("new_status", DecisionStatus.Canceled.ToString());
                }).Result;

            var body = JObject.Parse(result.Body.AsString());

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);

            Assert.IsNotEmpty(body["data"]["decision"].ToString());

            decision = decision.Refresh();

            Assert.AreEqual(DecisionStatus.Canceled, decision.status);
        }

        [Test]
        public void Delete_DataCorrect_DecisionDeleted() {
            var user = UserFaker.Create();
            var decision = EntityDecisionFaker.Create(user);

            var result = new Browser(new DefaultNancyBootstrapper())
                .Delete("/api/v1/entity/decision/delete", with => {
                    with.HttpRequest();
                    with.Query("api_token", Jwt.FromUserId(user.id));
                    with.Query("decision_guid", decision.guid);
                }).Result;

            JObject.Parse(result.Body.AsString());
            
            Assert.IsNull(decision.Refresh());
        }
    }
}