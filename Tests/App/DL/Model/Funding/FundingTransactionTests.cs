using System.Linq;
using App.DL.Enum;
using App.DL.Model.Funding;
using App.DL.Repository.Funding;
using Micron.DL.Module.Misc;
using NUnit.Framework;
using Tests.Testing;
using Tests.Utils.Fake.Funding;
using Tests.Utils.Fake.Project;
using Tests.Utils.Fake.User;

namespace Tests.App.DL.Model.Funding {
    [TestFixture]
    public class FundingTransactionTests : BaseTestFixture {
        [Test]
        public void Create_DataCorrect_GotCorrectId() {
            var user = UserFaker.Create();

            var project = ProjectFaker.Create(user);
            
            var invoice = InvoiceFaker.Create(user);
            
            var id = FundingTransaction.Create(
                user, project.id, EntityType.Project, invoice, 0.05M, CurrencyType.BitCoin
            );

            Assert.True(id > 0);

            var tx = FundingTransactionRepository.Find(id);
            
            Assert.NotNull(tx);
        }

        [Test]
        public void GetLatest_DataCorrect_GotTransactions() {
            var amount = 10;
            var entityId = Rand.Int();

            var txs = FundingTransactionFaker.CreateMany(amount, entityId).Take(amount).ToList();
            
            var result = FundingTransaction.GetLatest(entityId, EntityType.Project, amount);

            foreach (var item in result) {
                Assert.NotNull(txs.FirstOrDefault(x => x.guid == item.guid));
            }
        }
    }
}