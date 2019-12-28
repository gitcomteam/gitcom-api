using System;
using System.Linq;
using App.DL.Enum;
using App.DL.Repository.User;
using Dapper;
using UserModel = App.DL.Model.User.User;

namespace App.DL.Model.User {
    public class UserBalance : Micron.DL.Model.Model {
        public int id;

        public string guid;

        public int user_id;

        public decimal balance;

        public CurrencyType currency_type;

        public int? crypto_token_id;

        public DateTime created_at;

        public DateTime updated_at;

        public User User() => UserRepository.Find(user_id);

        public static UserBalance Find(int id)
            => Connection().Query<UserBalance>(
                "SELECT * FROM user_balances WHERE id = @id LIMIT 1",
                new {id}
            ).FirstOrDefault();

        public static UserBalance FindByGuid(string guid)
            => Connection().Query<UserBalance>(
                "SELECT * FROM user_balances WHERE guid = @guid LIMIT 1",
                new {guid}
            ).FirstOrDefault();
        
        public static UserBalance FindByUserId(int userId)
            => Connection().Query<UserBalance>(
                "SELECT * FROM user_balances WHERE user_id = @user_id LIMIT 1",
                new {user_id = userId}
            ).FirstOrDefault();

        public static int Create(
            UserModel user, CurrencyType currencyType, decimal setBalance = 0
        )
            => ExecuteScalarInt(
                $@"INSERT INTO user_balances(guid, user_id, currency_type, balance)
                VALUES (@guid, @user_id, '{currencyType.ToString()}', @balance);
                SELECT currval('user_balances_id_seq');"
                , new {
                    guid = Guid.NewGuid().ToString(), user_id = user.id, balance = setBalance
                }
            );

        public UserBalance UpdateBalance(decimal newBalance) {
            ExecuteSql(
                @"UPDATE user_balances 
                SET balance = @balance, updated_at = CURRENT_TIMESTAMP WHERE id = @id",
                new {
                    balance = newBalance, id
                }
            );
            return this;
        }

        public static UserBalance[] GetPositive(UserModel user, ushort limit = 10)
            => Connection().Query<UserBalance>(
                $@"SELECT * FROM user_balances 
                    WHERE user_id = @user_id AND balance > 0 
                    ORDER BY balance DESC LIMIT {limit}", new {user_id = user.id}
            ).ToArray();

        public UserBalance Refresh() => Find(id);
    }
}