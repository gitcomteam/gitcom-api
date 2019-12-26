using App.DL.Enum;
using App.DL.Model.User;
using Micron.DL.Module.Misc;
using NUnit.Framework;
using Tests.Testing;
using Tests.Utils.Fake.User;

namespace Tests.App.DL.Model.User {
    [TestFixture]
    public class UserBalanceTests : BaseTestFixture {
        [Test]
        public void Create_DataCorrect_BalanceCreated() {
            var user = UserFaker.Create();
            var id = UserBalance.Create(user, CurrencyType.BitCoin);
            var balance = UserBalance.Find(id);
            Assert.NotNull(balance);
            Assert.AreEqual(user.id, balance.User().id);
        }
        
        [Test]
        public void Create_WithAmount_BalanceEquals() {
            var randomBalance = Rand.SmallDecimal();
            var user = UserFaker.Create();
            var id = UserBalance.Create(user, CurrencyType.BitCoin, randomBalance);
            var balance = UserBalance.Find(id);
            Assert.NotNull(balance);
            Assert.AreEqual(user.id, balance.User().id);
            Assert.AreEqual(randomBalance, balance.balance);
        }

        [Test]
        public void GetPositive_DataCorrect_GotOne() {
            var user = UserFaker.Create();

            var amount = Rand.SmallDecimal();
            
            UserBalanceFaker.Create(user, amount);

            var balances = UserBalance.GetPositive(user);
            
            Assert.AreEqual(amount, balances[0].balance);
        }
    }
}