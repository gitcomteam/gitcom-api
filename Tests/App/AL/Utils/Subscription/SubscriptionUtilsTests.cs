using App.AL.Utils.Subscription;
using App.DL.Enum;
using App.DL.Repository.Funding;
using App.DL.Repository.Subscription;
using App.DL.Repository.User;
using Micron.DL.Module.Misc;
using NUnit.Framework;
using Tests.Testing;
using Tests.Utils.Fake.Project;
using Tests.Utils.Fake.User;

namespace Tests.App.AL.Utils.Subscription {
    public class SubscriptionUtilsTests : BaseTestFixture {
        [Test]
        public void FundEntity_DataCorrect_EntityFunded() {
            var user = UserFaker.Create();

            var amount = Rand.SmallDecimal();

            var balance = UserBalanceRepository.FindOrCreate(user, CurrencyType.BitCoin);
            balance.UpdateBalance(amount);
            
            var info = UserSubscriptionInfoRepository.FindOrCreate(user);
            info.UpdateSelectedAmount(amount).Refresh();
            
            ProjectFaker.Create();

            SubscriptionUtils.PeriodPay(user);
            
            var txs = FundingTransactionRepository.Get(user);
            
            Assert.True(txs.Length > 0);
            Assert.True(txs[0].amount > 0);
        }
    }
}