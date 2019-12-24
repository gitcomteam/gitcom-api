using App.DL.Module.Cache;
using Micron.DL.Module.Misc;
using UserModel = App.DL.Model.User.User;

namespace App.DL.Repository.User {
    public static class UserRepository {
        public static UserModel Find(int id) {
            var cached = ModelCache.Get("User", id);
            if (cached != null) {
                return (UserModel) cached;
            }
            var item = UserModel.Find(id);
            if (item != null) {
                ModelCache.Store("User", item.id, item);
            }
            return item;
        }

        public static UserModel FindByEmail(string email) {
            return UserModel.FindByEmail(email);
        }

        public static UserModel FindByGuid(string guid) {
            var cached = ModelCache.Get("User", guid);
            if (cached != null) {
                return (UserModel) cached;
            }
            var item= UserModel.FindByGuid(guid);
            ModelCache.Store("User", item.guid, item);
            return item;
        }

        public static UserModel FindOrCreateByEmailAndLogin(string email, string login, string password = null) {
            password ??= Rand.RandomString();
            
            var user = UserModel.FindByEmail(email);

            var loginUser = UserModel.FindByLogin(login);

            var baseLogin = login;
            int postfixNum = 0;
            
            while (loginUser != null) {
                postfixNum++;
                login = $"{baseLogin}_{postfixNum}";
                loginUser = UserModel.FindByLogin(login);
            }
            
            user ??= Create(email, login, password);
            
            return user;
        }

        public static UserModel Create(string email, string login, string password) {
            return Find(UserModel.Create(email, login, password));
        }
    }
}