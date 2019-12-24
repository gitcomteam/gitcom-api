using System;
using App.DL.Enum;
using App.DL.Model.Funding;
using App.DL.Model.User;
using App.DL.Repository.Funding;
using App.DL.Repository.User;

namespace App.AL.Utils.Funding {
    public static class UserBalanceUtils {
        public static UserBalance Deposit(User user, Invoice invoice) {
            if (invoice.entity_type != EntityType.UserBalance) {
                throw new Exception("Deposit can be done only for UserBalance");
            }
            
            var existingTx = FundingTransactionRepository.Find(user, invoice, user.id, EntityType.UserBalance);
            
            var balance = UserBalanceRepository.FindOrCreate(invoice);

            if (existingTx != null) {
                throw new Exception("Transaction for specified invoice already exists");
            }
            
            FundingTransactionUtils.CreateTxFromInvoice(invoice);
            UserBalanceRepository.UpdateBalance(balance, balance.balance + invoice.amount);

            return balance.Refresh();
        }
    }
}