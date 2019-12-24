using App.AL.Utils.Entity;
using App.DL.Enum;
using App.DL.Model.Funding;
using App.DL.Model.User;

namespace App.AL.Utils.Funding {
    public static class SubscriptionFundingUtils {
        public static FundingBalance FundEntity(
            User from, int id, EntityType type, decimal amount, CurrencyType currency
        ) {
            if (!EntityUtils.IsEntityExists(id, type) || amount == 0) {
                return null;
            }
            
            return FundingBalanceUtils.FundEntity(from, id, type, amount, currency);
        }
    }
}