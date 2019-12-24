using App.DL.Repository.User;
using NUnit.Framework;
using Tests.Testing;
using Tests.Utils.Fake.User;

namespace Tests.App.DL.Module.Cache {
    public class ModelCache : BaseTestFixture {
        [Test]
        public void Store_UserId_WithoutCrashing() {
            var user = UserFaker.Create();
            
            for (int i = 0; i <= 100; i++) {
                UserRepository.Find(user.id);
            }
        }
        
        [Test]
        public void Store_UserGuid_WithoutCrashing() {
            var user = UserFaker.Create();
            
            for (int i = 0; i <= 100; i++) {
                UserRepository.FindByGuid(user.guid);
            }
        }
    }
}