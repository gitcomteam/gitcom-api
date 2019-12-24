using App.DL.Enum;
using App.DL.Model.Noficication;

namespace App.DL.Repository.Notification {
    public static class UserNotificationRepository {
        public static UserNotification Find(int id) => UserNotification.Find(id);

        public static UserNotification FindByGuid(string guid) => UserNotification.FindByGuid(guid);

        public static UserNotification[] Get(
            Model.User.User user, bool seen = false, int limit = 10
        ) {
            return UserNotification.Get(user, seen, limit);
        }

        public static UserNotification Create(
            Model.User.User user, string title, string content, UserNotificationType type = UserNotificationType.Info
        ) {
            return Find(UserNotification.Create(user, title, content, type));
        }
    }
}