using App.DL.Model.User.Registration;

namespace App.DL.Repository.User.Registration {
    public static class RegistrationQueueItemRepository {
        public static RegistrationQueueItem Find(int id) => RegistrationQueueItem.Find(id);

        public static RegistrationQueueItem Find(Model.User.User user) => RegistrationQueueItem.Find(user);

        public static RegistrationQueueItem FindBy(string col, string val) => RegistrationQueueItem.FindBy(col, val);

        public static RegistrationQueueItem Create(Model.User.User user)
            => Find(RegistrationQueueItem.Create(user));
    }
}