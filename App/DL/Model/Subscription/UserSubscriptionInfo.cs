using System;
using System.Linq;
using App.DL.Enum;
using Dapper;

namespace App.DL.Model.Subscription {
    public class UserSubscriptionInfo : Micron.DL.Model.Model {
        public int id;

        public int user_id;

        public decimal selected_amount;
        
        public CurrencyType selected_currency;

        public DateTime last_paid;

        public DateTime created_at;

        public DateTime updated_at;

        public static UserSubscriptionInfo Find(int id)
            => Connection().Query<UserSubscriptionInfo>(
                "SELECT * FROM user_subscription_info WHERE id = @id LIMIT 1",
                new {id}
            ).FirstOrDefault();

        public static UserSubscriptionInfo Find(User.User user)
            => Connection().Query<UserSubscriptionInfo>(
                "SELECT * FROM user_subscription_info WHERE user_id = @user_id LIMIT 1",
                new {user_id = user.id}
            ).FirstOrDefault();

        public static UserSubscriptionInfo FindOrCreate(User.User user) => Find(user) ?? Find(Create(user));

        public static int Create(User.User user) {
            return ExecuteScalarInt(
                @"INSERT INTO public.user_subscription_info(user_id) VALUES (@user_id);
                       SELECT currval('user_subscription_info_id_seq');"
                , new {user_id = user.id}
            );
        }

        public UserSubscriptionInfo Refresh() => Find(id);

        public UserSubscriptionInfo UpdateSelectedAmount(decimal amount) {
            ExecuteSql(
                @"UPDATE user_subscription_info SET selected_amount = @selected_amount, updated_at = CURRENT_TIMESTAMP " +
                "WHERE id = @id", new {selected_amount = amount, id}
            );
            return this;
        }
        
        public UserSubscriptionInfo UpdateSelectedCurrency(CurrencyType currency) {
            ExecuteSql(
                $@"UPDATE user_subscription_info SET selected_currency = '{currency.ToString()}', updated_at = CURRENT_TIMESTAMP " +
                "WHERE id = @id", new {id}
            );
            return this;
        }
    }
}