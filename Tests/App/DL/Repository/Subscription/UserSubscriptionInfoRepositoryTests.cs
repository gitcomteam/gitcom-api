using App.DL.Repository.Subscription;
using NUnit.Framework;
using Tests.Testing;
using Tests.Utils.Fake.User;

namespace Tests.App.DL.Repository.Subscription {
    public class UserSubscriptionInfoRepositoryTests : BaseTestFixture {
        [Test]
        public void FindOrCreate_DataCorrect_InfoCreated() {
            var user = UserFaker.Create();
            
            Assert.IsNull(UserSubscriptionInfoRepository.Find(user));

            UserSubscriptionInfoRepository.FindOrCreate(user);
            
            Assert.NotNull(UserSubscriptionInfoRepository.Find(user));
        }
    }
}