using App.DL.Enum;
using App.DL.Model.Auth;
using UserModel = App.DL.Model.User.User;

namespace App.DL.Repository.Auth {
    public static class ServiceAccessTokenRepository {
        public static ServiceAccessToken Find(int id) {
            return ServiceAccessToken.Find(id);
        }

        public static ServiceAccessToken FindByGuid(string guid) {
            return ServiceAccessToken.FindByGuid(guid);
        }

        public static ServiceAccessToken FindBy(string col, string val) {
            return ServiceAccessToken.FindBy(col, val);
        }

        public static ServiceAccessToken Find(UserModel user, ServiceType serviceType) {
            return ServiceAccessToken.Find(user, serviceType);
        }

        public static ServiceAccessToken FindOrUpdateAccessToken(
            UserModel user, string accessToken, ServiceType serviceType
        ) {
            var token = Find(user, serviceType);

            if (token == null) {
                token = Create(user, accessToken, serviceType);
            }

            token.UpdateToken(accessToken);

            return token;
        }

        public static ServiceAccessToken Create(UserModel user, string accessToken, ServiceType serviceType) {
            return Find(ServiceAccessToken.Create(user, accessToken, serviceType));
        }
    }
}