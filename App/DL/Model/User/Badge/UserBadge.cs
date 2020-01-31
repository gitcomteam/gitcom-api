using System;
using System.Linq;
using App.DL.Repository.User;
using Dapper;

namespace App.DL.Model.User.Badge {
    public class UserBadge : Micron.DL.Model.Model {
        public int id;

        public string guid;

        public int user_id;

        public string badge;

        public DateTime created_at;

        public static UserBadge Find(int id)
            => Connection().Query<UserBadge>(
                "SELECT * FROM user_badges WHERE id = @id LIMIT 1", new {id}
            ).FirstOrDefault();

        public static UserBadge FindBy(string col, string val)
            => Connection().Query<UserBadge>(
                $"SELECT * FROM user_badges WHERE {col} = @val LIMIT 1", new {val}
            ).FirstOrDefault();

        public static UserBadge[] Get(User user)
            => Connection().Query<UserBadge>(
                "SELECT * FROM user_badges WHERE user_id = @user_id", new {user_id = user.id}
            ).ToArray();

        public static UserBadge Create(User user, string badge) {
            return Find(ExecuteScalarInt(
                @"INSERT INTO user_badges(guid, user_id, badge)
                        VALUES (@guid, @user_id, @badge);
                        SELECT currval('user_badges_id_seq');"
                , new {
                    guid = Guid.NewGuid().ToString(), user_id = user.id, badge
                }
            ));
        }

        public UserBadge UpdateCol(string col, string val) {
            ExecuteSql(
                $"UPDATE user_badges SET {col} = @val WHERE id = @id",
                new {val, id}
            );
            return this;
        }

        public void Delete() => ExecuteScalarInt("DELETE FROM user_badges WHERE id = @id", new {id});

        public User User() => UserRepository.Find(user_id);
    }
}