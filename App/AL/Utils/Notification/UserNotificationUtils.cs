using App.DL.Enum;
using App.DL.Model.Noficication;
using App.DL.Model.User;
using App.DL.Repository.Notification;
using Micron.DL.Module.Db;
using Dapper;

namespace App.AL.Utils.Notification {
    public static class UserNotificationUtils {
        public static UserNotification NewNotification(
            User user, string title, string content, UserNotificationType type = UserNotificationType.Info
        ) {
            return UserNotificationRepository.Create(user, title, content, type);
        }

        public static UserNotification[] GetActive(User user) {
            return UserNotificationRepository.Get(user);
        }

        public static void SeenAll(User user) {
            DbConnection.Connection().ExecuteScalar<bool>(
                "UPDATE user_notifications SET seen = true WHERE user_id = @user_id",
                new { user_id = user.id }
            );
        }
    }
}