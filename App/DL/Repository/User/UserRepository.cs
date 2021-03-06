using App.AL.Utils.External.Discord;
using App.DL.Enum;
using App.DL.Model.Funding;
using App.DL.Model.User;
using App.DL.Model.User.Badge;
using App.DL.Module.Cache;
using App.DL.Repository.User.Referral;
using Micron.DL.Module.Config;
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
        
        public static UserModel FindByLogin(string login) {
            return UserModel.FindByLogin(login);
        }

        public static UserModel FindByGuid(string guid) {
            var cached = ModelCache.Get("User", guid);
            if (cached != null) return (UserModel) cached;
            
            var item= UserModel.FindByGuid(guid);
            if (item != null) ModelCache.Store("User", item.guid, item);
            return item;
        }

        public static UserModel FindOrCreateByEmailAndLogin(
            string email, string login, string password = null, UserModel referral = null
        ) {
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
            
            UserBadge.Create(user, "Early adopter");

            int tokenRegisterBonus = System.Convert.ToInt32(
                AppConfig.GetConfiguration("user:registration:token_bonus")
            );

            if (tokenRegisterBonus > 0) {
                UserBalance.Create(user, CurrencyType.GitComToken, tokenRegisterBonus);
                FundingTransaction.Create(
                    user, user.id, EntityType.User, null, tokenRegisterBonus, CurrencyType.GitComToken
                );
            }

            if (referral != null) UserReferralRepository.Create(user, referral);
            
            return user;
        }

        public static UserModel Create(string email, string login, string password) {
            var newUserId = UserModel.Create(email, login, password);
            DiscordWebhooks.SendEvent("system-events", $"New user #{newUserId} just signed up! Email: {email}");
            return Find(newUserId);
        }
    }
}