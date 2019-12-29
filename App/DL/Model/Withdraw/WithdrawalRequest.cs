using System;
using System.Linq;
using App.DL.Enum;
using App.DL.Repository.User;
using Dapper;

namespace App.DL.Model.Withdraw {
    public class WithdrawalRequest : Micron.DL.Model.Model {
        public int id;

        public string guid;

        public int user_id;

        public decimal amount;

        public string address;

        public bool paid;

        public CurrencyType currency_type;

        public DateTime created_at;

        public static WithdrawalRequest Find(int id)
            => Connection().Query<WithdrawalRequest>(
                "SELECT * FROM withdrawal_requests WHERE id = @id LIMIT 1", new {id}
            ).FirstOrDefault();
        
        public static WithdrawalRequest[] Get(User.User user)
            => Connection().Query<WithdrawalRequest>(
                "SELECT * FROM withdrawal_requests WHERE user_id = @user_id LIMIT 100",
                new {user_id = user.id}
            ).ToArray();
        
        public static int Create(User.User user, CurrencyType currencyType, decimal amount, string address) {
            return ExecuteScalarInt(
                $@"INSERT INTO withdrawal_requests(guid, user_id, currency_type, amount, address)
                           VALUES (@guid, @user_id, '{currencyType.ToString()}', @amount, @address);
                           SELECT currval('withdrawal_requests_id_seq');"
                , new {
                    guid = Guid.NewGuid().ToString(), user_id = user.id, amount, address
                }
            );
        }

        public User.User User() => UserRepository.Find(user_id);
    }
}