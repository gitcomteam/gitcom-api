using App.DL.Enum;
using App.DL.Model.Funding;
using App.DL.Repository.Funding;
using Micron.DL.Module.Misc;

namespace Tests.Utils.Fake.Funding {
    public class CurrencyWalletFaker {
        public static CurrencyWallet Create(CurrencyType currencyType, string address = "") {
            address ??= Rand.RandomString();
            return CurrencyWalletRepository.Create(address, currencyType);
        }
    }
}