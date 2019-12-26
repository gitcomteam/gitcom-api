using App.DL.Model.User.Referral;

namespace App.DL.Repository.User.Referral {
    public static class UserReferralRepository {
        public static UserReferral Find(int id) => UserReferral.Find(id);
        
        public static UserReferral[] GetInvited(Model.User.User user) => UserReferral.GetInvited(user);

        public static UserReferral Create(Model.User.User user, Model.User.User referral)
            => Find(UserReferral.Create(user, referral));
    }
}