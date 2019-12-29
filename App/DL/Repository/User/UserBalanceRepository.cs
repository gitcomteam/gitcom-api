using App.DL.Enum;
using App.DL.Model.Funding;
using App.DL.Model.User;
using UserModel = App.DL.Model.User.User;

namespace App.DL.Repository.User {
    public static class UserBalanceRepository {
        public static UserBalance Find(int id) {
            return UserBalance.Find(id);
        }
        
        public static UserBalance Find(UserModel user) {
            return UserBalance.FindByUserId(user.id);
        }
        
        public static UserBalance Find(UserModel user, CurrencyType currencyType) => UserBalance.Find(user, currencyType);

        public static UserBalance FindOrCreate(Invoice invoice) {
            return FindOrCreate(
                UserRepository.Find(invoice.user_id),
                invoice.currency_type
            );
        }

        public static UserBalance FindOrCreate(UserModel user, CurrencyType currencyType) {
            return Find(user) ?? Create(
                       UserRepository.Find(user.id),
                       currencyType
                   );
        }

        public static UserBalance Create(UserModel user, CurrencyType currencyType, decimal setBalance = 0) {
            return Find(UserBalance.Create(user, currencyType, setBalance));
        }

        public static UserBalance[] GetPositive(UserModel user, ushort limit = 10) {
            return UserBalance.GetPositive(user, limit);
        }

        public static UserBalance UpdateBalance(UserBalance item, decimal balance) {
            return item.UpdateBalance(balance);
        }
    }
}