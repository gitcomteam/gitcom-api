using App.DL.Enum;
using App.DL.Model.Funding;
using App.DL.Model.User;
using App.DL.Repository.Funding;

namespace App.AL.Utils.Funding {
    public static class FundingBalanceUtils {
        // TODO: add functionality for user balances
        public static FundingBalance FundEntity(Invoice invoice) {
            if (invoice.status != InvoiceStatus.Confirmed) {
                return null;
            }

            var balance = FundingBalanceRepository.Find(invoice.entity_id, invoice.entity_type, invoice.currency_type);

            balance = balance ?? FundingBalanceRepository.Create(
                          invoice.entity_id, invoice.entity_type, invoice.currency_type
                      );

            if (invoice.entity_type == EntityType.UserBalance) { }
            else {
                balance = FundingBalanceRepository.AddFunds(balance, invoice);
            }

            return balance.Refresh();
        }

        public static FundingBalance FundEntity(
            User from, int entityId, EntityType entityType, decimal amount, CurrencyType currencyType
        ) {
            var balance = FundingBalanceRepository.FindOrCreate(entityId, entityType, currencyType);

            balance = FundingBalanceRepository.AddFunds(from, balance, amount);

            return balance.Refresh();
        }
    }
}