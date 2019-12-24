using App.DL.Model.Subscription;

namespace App.DL.Repository.Subscription {
    public static class UserSubscriptionInfoRepository {
        public static UserSubscriptionInfo Find(int id) => UserSubscriptionInfo.Find(id); 
        
        public static UserSubscriptionInfo Find(Model.User.User user) => UserSubscriptionInfo.Find(user); 

        public static UserSubscriptionInfo FindOrCreate(Model.User.User user) => UserSubscriptionInfo.FindOrCreate(user);
    }
}