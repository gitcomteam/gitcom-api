using NUnit.Framework;
using Tests.Testing;
using Tests.Utils.Fake.Decision;

namespace Tests.App.DL.Model.Decision {
    public class EntityDecisionTests : BaseTestFixture {
        [Test]
        public void Options_DataCorrect_Got2Options() {
            var decision = EntityDecisionFaker.Create();
            EntityDecisionOptionFaker.CreateMany(2, decision);
            
            Assert.AreEqual(2, decision.Options().Length);
        }
    }
}