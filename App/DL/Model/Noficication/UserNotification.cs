using System;
using System.Linq;
using App.DL.Enum;
using App.DL.Repository.User;
using Dapper;

namespace App.DL.Model.Noficication {
    public class UserNotification : Micron.DL.Model.Model {
        public int id;

        public string guid;

        public int user_id;

        public string title;

        public string content;

        public bool seen;

        public UserNotificationType type;

        public DateTime created_at;

        public DateTime updated_at;

        public static UserNotification Find(int id)
            => Connection().Query<UserNotification>(
                "SELECT * FROM user_notifications WHERE id = @id LIMIT 1",
                new {id}
            ).FirstOrDefault();

        public static UserNotification FindByGuid(string guid)
            => Connection().Query<UserNotification>(
                "SELECT * FROM user_notifications WHERE guid = @guid LIMIT 1",
                new {guid}
            ).FirstOrDefault();

        public static UserNotification[] Get(
            User.User user, bool seen = false, int limit = 10
        )
            => Connection().Query<UserNotification>(
                @"SELECT * FROM user_notifications
                        WHERE user_id = @user_id AND seen = @seen
                        ORDER BY id DESC
                        LIMIT @limit",
                new {user_id = user.id, seen, limit}
            ).ToArray();

        public static int Create(
            User.User user, string title, string content,
            UserNotificationType type = UserNotificationType.Info
        )
            => ExecuteScalarInt(
                $@"INSERT INTO public.user_notifications(guid, user_id, title, content, seen, type, updated_at) 
                VALUES (@guid, @user_id, @title, @content, @seen, '{type.ToString()}', CURRENT_TIMESTAMP);
                SELECT currval('user_notifications_id_seq');"
                , new {
                    guid = Guid.NewGuid().ToString(), user_id = user.id, title, content, seen = false, type
                }
            );

        public UserNotification Save() {
            ExecuteSql(
                @"UPDATE user_notifications
                SET seen = @seen AND updated_at = CURRENT_TIMESTAMP WHERE id = @id",
                new {seen, id}
            );
            return this;
        }

        public UserNotification Refresh() => Find(id);

        public User.User User() => UserRepository.Find(user_id);
    }
}