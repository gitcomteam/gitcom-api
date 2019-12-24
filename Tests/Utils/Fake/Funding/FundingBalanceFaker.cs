using App.DL.Enum;
using App.DL.Model.Funding;
using App.DL.Repository.Funding;
using Micron.DL.Module.Misc;

namespace Tests.Utils.Fake.Funding {
    public class FundingBalanceFaker {
        public static FundingBalance Create(
            int entityId = 0, EntityType entityType = EntityType.Project, 
            CurrencyType currencyType = CurrencyType.Ethereum, decimal balance = 0.1M
        ) {
            var newBalance = FundingBalanceRepository.Create(
                entityId == 0 ? Rand.Int() : entityId,
                entityType,
                currencyType
            );
            newBalance.UpdateBalance(balance);
            return newBalance.Refresh();
        }
    }
}