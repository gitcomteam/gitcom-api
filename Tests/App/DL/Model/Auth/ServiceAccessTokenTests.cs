using App.DL.Enum;
using App.DL.Model.Auth;
using App.DL.Repository.Auth;
using Micron.DL.Module.Misc;
using NUnit.Framework;
using Tests.Testing;
using Tests.Utils.Fake.User;

namespace Tests.App.DL.Model.Auth {
    [TestFixture]
    public class ServiceAccessTokenTests : BaseTestFixture {
        [Test]
        public void Create_DataCorrect_ItemCreated() {
            var user = UserFaker.Create();

            var accessToken = Rand.RandomString();

            var id = ServiceAccessToken.Create(user, accessToken, ServiceType.GitHub);

            var token = ServiceAccessToken.Find(id);
            
            Assert.NotNull(token);
        }
        
        [Test]
        public void Find_SearchByUserAndServiceType_ItemCreated() {
            var user = UserFaker.Create();
            
            var accessToken = ServiceAccessTokenRepository.FindOrUpdateAccessToken(user, Rand.RandomString(), ServiceType.GitHub);
            
            Assert.NotNull(accessToken);
        }
    }
}