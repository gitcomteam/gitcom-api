using System.Collections.Generic;
using App.DL.Enum;
using App.DL.Model.Funding;
using App.DL.Repository.Funding;
using Micron.DL.Module.Misc;
using Tests.Utils.Fake.User;

namespace Tests.Utils.Fake.Funding {
    public class FundingTransactionFaker {
        public static FundingTransaction Create(int entityId = 0, EntityType entityType = EntityType.Project) {
            return FundingTransactionRepository.Create(
                UserFaker.Create(),
                entityId == 0 ? Rand.Int() : entityId,
                entityType,
                InvoiceFaker.Create(),
                0.01M * Rand.SmallInt(),
                CurrencyType.Ethereum
            );
        }

        public static List<FundingTransaction> CreateMany(
            int amount, int entityId = 0, EntityType entityType = EntityType.Project
        ) {
            var result = new List<FundingTransaction>();
            
            for (ushort i = 0; i < amount; i++) {
                result.Add(Create(entityId, entityType));
            }

            return result;
        }
    }
}