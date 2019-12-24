using System;
using App.DL.Enum;
using App.DL.Model.Funding;
using UserModel = App.DL.Model.User.User;

namespace App.DL.Repository.Funding {
    public static class FundingTransactionRepository {
        public static FundingTransaction Find(int id) => FundingTransaction.Find(id);

        public static FundingTransaction Find(UserModel user, Invoice invoice, int entityId, EntityType entityType) {
            return FundingTransaction.Find(user, invoice, entityId, entityType);
        }
        
        public static FundingTransaction Find(Invoice invoice) => FundingTransaction.Find(invoice);

        public static FundingTransaction[] Get(
            int entityId, EntityType entityType, CurrencyType currencyType, int limit = 10
        ) {
            return FundingTransaction.Get(entityId, entityType, currencyType, limit);
        }
        
        public static FundingTransaction[] Get(UserModel user, int limit = 10) {
            return FundingTransaction.Get(user, limit);
        }

        public static FundingTransaction Create(
            UserModel from, int entityId, EntityType entityType, Invoice invoice, decimal amount,
            CurrencyType currencyType
        ) {
            return Find(FundingTransaction.Create(from, entityId, entityType, invoice, amount, currencyType));
        }
        
        public static FundingTransaction Create(
            UserModel from, int entityId, EntityType entityType, decimal amount, CurrencyType currencyType
        ) {
            return Find(FundingTransaction.Create(from, entityId, entityType, null, amount, currencyType));
        }

        public static FundingTransaction CreateDeposit(
            UserModel from, Invoice invoice
        ) {
            if (invoice.entity_type != EntityType.UserBalance) {
                throw new Exception("Deposit can be done only for UserBalance, tried for " + invoice.entity_type);
            }
            return Find(FundingTransaction.Create(
                from, invoice.entity_id, invoice.entity_type, invoice, invoice.amount, invoice.currency_type
            ));
        }
    }
}