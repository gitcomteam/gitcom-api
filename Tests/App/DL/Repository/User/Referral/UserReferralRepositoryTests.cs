using App.DL.Repository.User.Referral;
using NUnit.Framework;
using Tests.Testing;
using Tests.Utils.Fake.User;

namespace Tests.App.DL.Repository.User.Referral {
    public class UserReferralRepositoryTests : BaseTestFixture {
        [Test]
        public void Create_DataCorrect_ReferralCreated() {
            var user = UserFaker.Create();
            var referral = UserFaker.Create();
            UserReferralRepository.Create(user, referral);

            var invitedUsers = UserReferralRepository.GetInvited(referral);
            
            Assert.AreEqual(invitedUsers[0].referral_id, referral.id);
        }
    }
}