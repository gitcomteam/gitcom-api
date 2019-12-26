using System;
using System.Linq;
using App.DL.Repository.User;
using Dapper;

namespace App.DL.Model.User.Referral {
    public class UserReferral : Micron.DL.Model.Model {
        public int id;

        public string guid;

        public int user_id;

        public int referral_id;

        public DateTime created_at;

        public static UserReferral Find(int id)
            => Connection().Query<UserReferral>(
                "SELECT * FROM user_referrals WHERE id = @id LIMIT 1", new {id}
            ).FirstOrDefault();

        public static UserReferral[] GetInvited(User user)
            => Connection().Query<UserReferral>(
                "SELECT * FROM user_referrals WHERE referral_id = @referral_id LIMIT 50", new {
                    referral_id = user.id
                }
            ).ToArray();

        public static int Create(User user, User referral)
            => ExecuteScalarInt(
                @"INSERT INTO user_referrals(guid, user_id, referral_id)
                        VALUES (@guid, @user_id, @referral_id);
                        SELECT currval('user_referrals_id_seq');"
                , new {
                    guid = Guid.NewGuid().ToString(), user_id = user.id, referral_id = referral.id
                }
            );

        public User User() => UserRepository.Find(user_id);

        public User Referral() => UserRepository.Find(referral_id);
    }
}