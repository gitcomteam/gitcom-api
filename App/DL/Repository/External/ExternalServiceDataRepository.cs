using App.DL.Enum;
using App.DL.Model.External;
using UserModel = App.DL.Model.User.User;

namespace App.DL.Repository.External {
    public static class ExternalServiceDataRepository {
        public static ExternalServiceData Find(int id) {
            return ExternalServiceData.Find(id);
        }

        public static ExternalServiceData FindByGuid(string guid) {
            return ExternalServiceData.FindByGuid(guid);
        }

        public static ExternalServiceData Find(UserModel user, ServiceType serviceType) {
            return ExternalServiceData.Find(user, serviceType);
        }

        public static int Create(UserModel user, ServiceType serviceType, string originId, string login) {
            return ExternalServiceData.Create(user, serviceType, originId, login);
        }
    }
}