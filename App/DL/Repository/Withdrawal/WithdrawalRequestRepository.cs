using App.DL.Enum;
using App.DL.Model.Withdraw;

namespace App.DL.Repository.Withdrawal {
    public static class WithdrawalRequestRepository {
        public static WithdrawalRequest Find(int id) => WithdrawalRequest.Find(id);

        public static WithdrawalRequest[] Get(Model.User.User user) => WithdrawalRequest.Get(user);

        public static WithdrawalRequest Create(Model.User.User user, CurrencyType currencyType, decimal amount)
            => Find(WithdrawalRequest.Create(user, currencyType, amount));
    }
}