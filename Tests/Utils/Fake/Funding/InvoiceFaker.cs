using App.DL.Enum;
using App.DL.Model.Funding;
using App.DL.Repository.Funding;
using Micron.DL.Module.Misc;
using Tests.Utils.Fake.Project;
using Tests.Utils.Fake.User;
using UserModel = App.DL.Model.User.User;

namespace Tests.Utils.Fake.Funding {
    public class InvoiceFaker {
        public static Invoice Create(
            UserModel user = null, int? entityId = null, EntityType entityType = EntityType.Project,
            CurrencyType currencyType = CurrencyType.BitCoin, InvoiceStatus status = InvoiceStatus.Created
        ) {
            return InvoiceRepository.Create(
                user ?? UserFaker.Create(),
                entityId ?? ProjectFaker.Create().id,
                entityType,
                0.001M * Rand.SmallInt(),
                currencyType,
                status,
                CurrencyWalletFaker.Create(currencyType)
            );
        }
    }
}