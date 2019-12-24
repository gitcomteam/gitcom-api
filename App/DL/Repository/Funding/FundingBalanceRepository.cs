using App.AL.Utils.Funding;
using App.DL.Enum;
using App.DL.Model.Funding;

namespace App.DL.Repository.Funding {
    public static class FundingBalanceRepository {
        public static FundingBalance Find(int id) => FundingBalance.Find(id);
        
        public static FundingBalance FindByGuid(string guid) => FundingBalance.FindByGuid(guid);
        
        public static FundingBalance FindBy(string col, int val) => FundingBalance.FindBy(col, val);
        
        public static FundingBalance Find(int entityId, EntityType entityType, CurrencyType currencyType)
            => FundingBalance.Find(entityId, entityType, currencyType);

        public static FundingBalance FindOrCreate(int entityId, EntityType entityType, CurrencyType currencyType) {
            return FundingBalance.Find(entityId, entityType, currencyType) ?? Create(entityId, entityType, currencyType);
        }

        public static FundingBalance[] Get(int entityId, EntityType type, int limit = 10) {
            return FundingBalance.Get(entityId, type, limit);
        }

        public static FundingBalance AddFunds(FundingBalance balance, Invoice invoice) {
            FundingTransactionUtils.CreateTxFromInvoice(invoice);
            return balance.UpdateBalance(invoice.amount);
        }

        public static FundingBalance AddFunds(Model.User.User from, FundingBalance balance, decimal amount) {
            FundingTransactionRepository.Create(
                from, balance.entity_id, balance.entity_type, amount, balance.currency_type
            );
            return balance.UpdateBalance(balance.balance + amount);
        }

        public static FundingBalance Create(int entityId, EntityType entityType, CurrencyType currencyType) {
            return FundingBalance.Find(FundingBalance.Create(entityId, entityType, currencyType));
        }
    }
}