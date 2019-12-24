using App.DL.Enum;
using App.DL.Model.User;
using App.DL.Repository.User;
using UserModel = App.DL.Model.User.User;

namespace Tests.Utils.Fake.User {
    public static class UserBalanceFaker {
        public static UserBalance Create(
            UserModel user = null, decimal amount = 0, CurrencyType currencyType = CurrencyType.BitCoin
        ) {
            user = user ?? UserFaker.Create();
            var id = UserBalance.Create(user, currencyType, amount);
            return UserBalanceRepository.Find(id);
        }
    }
}