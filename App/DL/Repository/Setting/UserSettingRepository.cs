using App.DL.Model.Setting;
using App.DL.Module.Cache;

namespace App.DL.Repository.Setting {
    public static class UserSettingRepository {
        public static UserSetting Find(int id) {
            var cached = ModelCache.Get("User", id);
            if (cached != null) {
                return (UserSetting) cached;
            }

            var item = UserSetting.Find(id);
            ModelCache.Store("User", item.id, item);
            return UserSetting.Find(id);
        }

        public static UserSetting Find(Model.User.User user, string key) {
            return UserSetting.Find(user, key);
        }

        public static UserSetting FindBy(Model.User.User user, string col, string val) {
            return UserSetting.FindBy(user, col, val);
        }

        public static UserSetting[] Get(Model.User.User user, int limit = 50) {
            return UserSetting.Get(user, limit);
        }

        public static UserSetting SetSetting(Model.User.User user, string key, string value) {
            return UserSetting.SetSetting(user, key, value);
        }
    }
}