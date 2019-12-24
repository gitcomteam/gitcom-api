using App.DL.Enum;
using App.DL.Repository.Funding;
using NUnit.Framework;
using Tests.Testing;
using Tests.Utils.Fake.Funding;
using Tests.Utils.Fake.Project;
using Tests.Utils.Fake.User;

namespace Tests.App.DL.Repository.Funding {
    [TestFixture]
    public class FundingTransactionRepositoryTests : BaseTestFixture {
        [Test]
        public void Create_DataCorrect_GotTransaction() {
            var user = UserFaker.Create();

            var project = ProjectFaker.Create(user);

            var invoice = InvoiceFaker.Create(user, project.id);

            var tx = FundingTransactionRepository.Create(
                user, project.id, EntityType.Project, invoice, 0.05M, CurrencyType.BitCoin
            );

            Assert.True(tx.id > 0);
        }

        [Test]
        public void Create_WithoutInvoice_TransactionCreated() {
            var user = UserFaker.Create();
            
            var project = ProjectFaker.Create(user);
            
            var tx = FundingTransactionRepository.Create(
                user, project.id, EntityType.Project, null, 0.05M, CurrencyType.BitCoin
            );

            Assert.True(tx.id > 0);
        }
    }
}