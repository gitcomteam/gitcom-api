using App.AL.Utils.Funding;
using App.DL.Enum;
using App.DL.Repository.Funding;
using Micron.DL.Module.Misc;
using NUnit.Framework;
using Tests.Testing;
using Tests.Utils.Fake.Project;
using Tests.Utils.Fake.User;

namespace Tests.App.AL.Utils.Funding {
    public class SubscriptionFundingUtilsTests : BaseTestFixture {
        [Test]
        public void FundEntity_DataCorrect_EntityFundedTxCreated() {
            var user = UserFaker.Create();

            var project = ProjectFaker.Create();

            var amount = Rand.SmallDecimal();

            var balance =
                SubscriptionFundingUtils.FundEntity(user, project.id, EntityType.Project, amount, CurrencyType.BitCoin);

            Assert.AreEqual(amount, balance.balance);

            var txs = FundingTransactionRepository.Get(project.id, EntityType.Project, CurrencyType.BitCoin);

            Assert.AreEqual(1, txs.Length);
            Assert.AreEqual(amount, txs[0].amount);
        }
    }
}