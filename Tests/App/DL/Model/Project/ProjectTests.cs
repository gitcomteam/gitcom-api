using System.Linq;
using NUnit.Framework;
using Tests.Testing;
using Tests.Utils.Fake.Card;

namespace Tests.App.DL.Model.Project {
    public class ProjectTests : BaseTestFixture {
        [Test]
        public void Cards_Ok() {
            var card = CardFaker.Create();
            var project = card.Project();

            var foundCard = project.Cards().Where(c => c.id == card.id).ToArray()[0];
            Assert.AreEqual(card.id, foundCard.id);
        }
    }
}