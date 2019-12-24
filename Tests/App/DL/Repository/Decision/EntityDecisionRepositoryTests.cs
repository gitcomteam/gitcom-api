using System;
using App.DL.Enum;
using App.DL.Repository.Decision;
using NUnit.Framework;
using Tests.Testing;
using Tests.Utils.Fake.Project;
using Tests.Utils.Fake.User;

namespace Tests.App.DL.Repository.Decision {
    public class EntityDecisionRepositoryTests : BaseTestFixture {
        [Test]
        public void Create_DataCorrect_DecisionCreated() {
            var user = UserFaker.Create();
            var project = ProjectFaker.Create(user);

            var title = "test title";
            var content = "test content";

            var decision = EntityDecisionRepository.Create(
                user, project.id, EntityType.Project, title, content, DateTime.Now
            );

            Assert.NotNull(decision);

            Assert.AreEqual(title, decision.title);
            Assert.AreEqual(content, decision.content);
        }

        [Test]
        public void GetActive_DataCorrect_GotDecisions() {
            var user = UserFaker.Create();
            var project = ProjectFaker.Create(user);

            var title = "test title";
            var content = "test content";

            EntityDecisionRepository.Create(
                user, project.id, EntityType.Project, title, content, DateTime.Now
            );

            var decisions = EntityDecisionRepository.GetActive(project.id, EntityType.Project);

            Assert.AreEqual(1, decisions.Length);
        }

        [Test]
        public void UpdateStatus_DataCorrect_DecisionClosed() {
            var user = UserFaker.Create();
            var project = ProjectFaker.Create(user);

            var title = "test title";
            var content = "test content";

            EntityDecisionRepository.Create(
                user, project.id, EntityType.Project, title, content, DateTime.Now
            );
            
            var decision = EntityDecisionRepository.Create(
                user, project.id, EntityType.Project, title, content, DateTime.Now
            );
            
            Assert.AreEqual(DecisionStatus.Open, decision.status);

            var result = EntityDecisionRepository.UpdateStatus(decision, DecisionStatus.Closed);
            
            Assert.True(result);
            
            decision = decision.Refresh();
            
            Assert.AreEqual(DecisionStatus.Closed, decision.status);
        }
    }
}