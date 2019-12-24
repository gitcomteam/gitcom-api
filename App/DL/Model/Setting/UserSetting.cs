using System;
using System.Linq;
using App.DL.Module.Cache;
using Dapper;

namespace App.DL.Model.Setting {
    public class UserSetting : Micron.DL.Model.Model {
        public int id;

        public string guid;

        public int user_id;

        public string key;

        public string value;

        public DateTime created_at;

        public DateTime updated_at;

        public static UserSetting Find(int id)
            => Connection().Query<UserSetting>(
                "SELECT * FROM user_settings WHERE id = @id LIMIT 1",
                new {id}
            ).FirstOrDefault();

        public static UserSetting Find(User.User user, string setting)
            => Connection().Query<UserSetting>(
                "SELECT * FROM user_settings WHERE user_id = @user_id AND key = @key LIMIT 1",
                new {user_id = user.id, key = setting}
            ).FirstOrDefault();

        public static UserSetting FindBy(User.User user, string col, string val)
            => Connection().Query<UserSetting>(
                $"SELECT * FROM user_settings WHERE {col} = @val LIMIT 1",
                new {val}
            ).FirstOrDefault();

        public static UserSetting[] Get(User.User user, int limit = 50)
            => Connection().Query<UserSetting>(
                "SELECT * FROM user_settings WHERE user_id = @user_id LIMIT 50",
                new {user_id = user.id}
            ).ToArray();

        public static UserSetting SetSetting(User.User user, string key, string value) {
            var existingSetting = Find(user, key);
            if (existingSetting == null) {
                existingSetting = Find(ExecuteScalarInt(
                    @"INSERT INTO user_settings(guid, user_id, key, value, updated_at)
                            VALUES (@guid, @user_id, @key, @value, CURRENT_TIMESTAMP);
                            SELECT currval('user_settings_id_seq');"
                    , new {guid = Guid.NewGuid(), user_id = user.id, key, value}
                ));
            }

            ExecuteSql(
                "UPDATE user_settings SET value = @value, updated_at = CURRENT_TIMESTAMP WHERE id = @id",
                new {@value, existingSetting.id}
            );

            ModelCache.CleanUp("UserSetting", existingSetting.id);
            ModelCache.CleanUp("UserSetting", existingSetting.guid);
            
            return existingSetting.Refresh();
        }

        public UserSetting Refresh() => Find(id);
    }
}