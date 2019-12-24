using App.DL.Repository.Card;
using System;
using NUnit.Framework;
using Tests.Testing;
using Tests.Utils.Fake.User;
using Tests.Utils.Fake.Card;
using Micron.DL.Module.Misc;
using Tests.Utils.Fake.BoardColumn;

namespace Tests.App.DL.Model.Card {
    [TestFixture]
    public class CardTests : BaseTestFixture {
        [Test]
        public void Create_Card()
        {
            var user = UserFaker.Create();
            var column = BoardColumnFaker.Create(user);
            var name = "testName" + Rand.Int();
            var columnOrder = Rand.IntRange(1, 30);
            
            var card = CardRepository.CreateAndGet(name, "",columnOrder, column, user);
            card = CardRepository.Find(name, user, column);
            Assert.AreEqual(card.name, name);
            Assert.AreEqual(card.column_id, column.id);
        }
    }
}