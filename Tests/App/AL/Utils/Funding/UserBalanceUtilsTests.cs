using System;
using App.AL.Utils.Funding;
using App.DL.Enum;
using App.DL.Model.User;
using App.DL.Repository.Funding;
using NUnit.Framework;
using Tests.Testing;
using Tests.Utils.Fake.Funding;
using Tests.Utils.Fake.User;

namespace Tests.App.AL.Utils.Funding {
    [TestFixture]
    public class UserBalanceUtilsTests : BaseTestFixture {
        [Test]
        public void Deposit_DataCorrect_BalanceIncreasedAndTxCreated() {
            var user = UserFaker.Create();
            var invoice = InvoiceFaker.Create(user, user.id, EntityType.UserBalance);

            Assert.IsNull(FundingTransactionRepository.Find(user, invoice, user.id, EntityType.UserBalance));
            
            var balance = UserBalanceUtils.Deposit(user, invoice);

            var tx = FundingTransactionRepository.Find(user, invoice, user.id, EntityType.UserBalance);
            
            Assert.IsNotNull(tx);
            
            Assert.AreEqual(user.id, tx.entity_id);
            Assert.AreEqual(user.id, tx.from_user_id);
            Assert.AreEqual(EntityType.UserBalance, tx.entity_type);
            
            Assert.AreEqual(invoice.amount, balance.balance);
        }
        
        [Test]
        public void Deposit_2Deposits_SumCorrect() {
            var user = UserFaker.Create();
            var invoice = InvoiceFaker.Create(user, user.id, EntityType.UserBalance);

            Assert.IsNull(FundingTransactionRepository.Find(user, invoice, user.id, EntityType.UserBalance));
            
            var balance = UserBalanceUtils.Deposit(user, invoice);

            var tx = FundingTransactionRepository.Find(user, invoice, user.id, EntityType.UserBalance);
            
            Assert.IsNotNull(tx);

            Assert.AreEqual(invoice.amount, balance.balance);
            
            var invoice2 = InvoiceFaker.Create(user, user.id, EntityType.UserBalance);
            
            balance = UserBalanceUtils.Deposit(user, invoice2);
            
            Assert.AreEqual(invoice.amount + invoice2.amount, balance.balance);
        }

        [Test]
        public void Deposit_InvoiceExists_ThrowsException() {
            var user = UserFaker.Create();
            var invoice = InvoiceFaker.Create(user, user.id, EntityType.UserBalance);

            Assert.IsNull(FundingTransactionRepository.Find(user, invoice, user.id, EntityType.UserBalance));

            FundingTransactionRepository.CreateDeposit(user, invoice);

            Assert.Throws<Exception>(
                () => { UserBalanceUtils.Deposit(user, invoice); });
        }
    }
}