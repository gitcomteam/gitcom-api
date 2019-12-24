using App.DL.Repository.Funding;
using NUnit.Framework;
using Tests.Testing;
using Tests.Utils.Fake.Funding;
using Tests.Utils.Fake.User;

namespace Tests.App.DL.Repository.Funding {
    [TestFixture]
    public class FundingBalanceRepositoryTests : BaseTestFixture {
        [Test]
        public void Create_DataCorrect_BalanceAndTransactionCreated() {
            var invoice = InvoiceFaker.Create();
            
            var balance = FundingBalanceRepository.Create(
                invoice.entity_id, invoice.entity_type, invoice.currency_type
            );
            FundingBalanceRepository.AddFunds(balance, invoice);
            
            Assert.NotNull(balance);

            var txs = FundingTransactionRepository.Get(invoice.entity_id, invoice.entity_type, invoice.currency_type);
            
            Assert.AreEqual(1, txs.Length);

            var transaction = txs[0];

            balance = balance.Refresh();
            
            Assert.AreEqual(invoice.amount, balance.balance);
            
            Assert.AreEqual(balance.entity_id, transaction.entity_id);
            Assert.AreEqual(balance.entity_type, transaction.entity_type);
            Assert.AreEqual(balance.currency_type, transaction.currency_type);
        }
        
        [Test]
        public void AddFunds_DataCorrect_AddFunds() {
            var user = UserFaker.Create();
            var invoice = InvoiceFaker.Create(user);
            
            var balance = FundingBalanceRepository.Create(
                invoice.entity_id, invoice.entity_type, invoice.currency_type
            );
            FundingBalanceRepository.AddFunds(balance, invoice);
            
            Assert.NotNull(balance);

            balance = balance.Refresh();
            
            Assert.AreEqual(invoice.amount, balance.balance);
            
            var newInvoice = InvoiceFaker.Create(user, invoice.entity_id);

            balance = FundingBalanceRepository.AddFunds(balance, newInvoice).Refresh();
            
            Assert.AreEqual(invoice.amount + newInvoice.amount, balance.balance);
        }
    }
}