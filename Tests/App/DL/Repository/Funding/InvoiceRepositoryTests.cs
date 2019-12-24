using App.DL.Enum;
using App.DL.Repository.Funding;
using Micron.DL.Module.Misc;
using NUnit.Framework;
using Tests.Testing;
using Tests.Utils.Fake.Funding;
using Tests.Utils.Fake.User;

namespace Tests.App.DL.Repository.Funding {
    [TestFixture]
    public class InvoiceRepositoryTests : BaseTestFixture {
        [Test]
        public void Create_DataCorrect_GotInvoice() {
            var user = UserFaker.Create();
            var result = InvoiceRepository.Create(
                user,
                Rand.Int(),
                EntityType.Project,
                0.0001M * Rand.SmallInt(),
                CurrencyType.BitCoin,
                InvoiceStatus.Created,
                CurrencyWalletFaker.Create(CurrencyType.BitCoin)
            );
            Assert.NotNull(result);
            Assert.AreEqual(user.id, result.User().id);
        }

        [Test]
        public void UpdateStatus_DataCorrect_StatusUpdated() {
            var invoice = InvoiceFaker.Create();
            
            Assert.AreEqual(InvoiceStatus.Created, invoice.status);
            
            InvoiceRepository.UpdateStatus(invoice, InvoiceStatus.RequiresConfirmation);
            
            Assert.AreEqual(InvoiceStatus.RequiresConfirmation, invoice.Refresh().status);
        }
    }
}