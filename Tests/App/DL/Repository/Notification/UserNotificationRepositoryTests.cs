using App.DL.Enum;
using App.DL.Repository.Notification;
using NUnit.Framework;
using Tests.Testing;
using Tests.Utils.Fake.User;

namespace Tests.App.DL.Repository.Notification {
    public class UserNotificationRepositoryTests : BaseTestFixture {
        [Test]
        public void Create_DataCorrect_NotificationCreated() {
            var user = UserFaker.Create();

            var title = "test title";

            var content = "content here";
            
            var notification = UserNotificationRepository.Create(user, title, content, UserNotificationType.Warning);

            Assert.NotNull(notification);
            
            Assert.AreEqual(title, notification.title);
            Assert.AreEqual(content, notification.content);
            Assert.AreEqual(UserNotificationType.Warning, notification.type);
        }

        [Test]
        public void Get_DataCorrect_GotNotifications() {
            var user = UserFaker.Create();
            
            var title = "test title";

            var content = "content here";
            
            Assert.Zero(UserNotificationRepository.Get(user).Length);
            
            UserNotificationRepository.Create(user, title, content, UserNotificationType.Warning);

            var notifications = UserNotificationRepository.Get(user);
            
            Assert.AreEqual(1, notifications.Length);
            
            Assert.AreEqual(title, notifications[0].title);
            Assert.AreEqual(content, notifications[0].content);
        }
    }
}