using App.DL.Enum;
using App.DL.Model.Funding;

namespace App.DL.Repository.Funding {
    public static class CurrencyWalletRepository {
        public static CurrencyWallet Find(int id) {
            return CurrencyWallet.Find(id);
        }

        public static CurrencyWallet FindByGuid(string guid) {
            return CurrencyWallet.FindBy("guid", guid);
        }
        
        public static CurrencyWallet FindRandom(CurrencyType type) {
            return CurrencyWallet.FindRandomByEnum("currency_type", type.ToString());
        }

        public static CurrencyWallet Create(string address, CurrencyType currencyType) {
            return Find(CurrencyWallet.Create(address, currencyType));
        }
    }
}