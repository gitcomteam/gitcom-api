using App.AL.Utils.Funding;
using App.DL.Enum;
using App.DL.Repository.User;
using NUnit.Framework;
using Tests.Testing;
using Tests.Utils.Fake.Funding;
using Tests.Utils.Fake.User;

namespace Tests.App.AL.Utils.Funding {
    [TestFixture]
    public class InvoiceUtilsTests : BaseTestFixture {
        [Test]
        public void ProcessConfirmedInvoice_DataCorrect_InvoiceProcessed() {
            var user = UserFaker.Create();

            var balance = UserBalanceRepository.FindOrCreate(user, CurrencyType.BitCoin);
            
            var invoice = InvoiceFaker.Create(user, balance.id, EntityType.UserBalance);

            invoice = invoice.UpdateStatus(InvoiceStatus.Confirmed).Refresh();

            invoice = InvoiceUtils.ProcessConfirmedInvoice(invoice).Refresh();

            balance = balance.Refresh();
            
            Assert.NotNull(balance);
            
            Assert.AreEqual(InvoiceStatus.Done, invoice.status);
            Assert.AreEqual(invoice.amount, balance.balance);
        }
        
        [Test]
        public void ProcessConfirmedInvoice_2Invoices_BalanceSumCorrect() {
            var user = UserFaker.Create();

            var balance = UserBalanceRepository.FindOrCreate(user, CurrencyType.BitCoin);
            
            var invoice = InvoiceFaker.Create(user, balance.id, EntityType.UserBalance);
            invoice = invoice.UpdateStatus(InvoiceStatus.Confirmed).Refresh();

            InvoiceUtils.ProcessConfirmedInvoice(invoice).Refresh();

            var invoice2 = InvoiceFaker.Create(user, balance.id, EntityType.UserBalance);
            invoice2 = invoice2.UpdateStatus(InvoiceStatus.Confirmed).Refresh();

            InvoiceUtils.ProcessConfirmedInvoice(invoice2).Refresh();
            
            balance = balance.Refresh();
            
            Assert.AreEqual(InvoiceStatus.Done, invoice.Refresh().status);
            Assert.AreEqual(InvoiceStatus.Done, invoice2.Refresh().status);
            Assert.AreEqual(invoice.amount + invoice2.amount, balance.balance);
        }
    }
}