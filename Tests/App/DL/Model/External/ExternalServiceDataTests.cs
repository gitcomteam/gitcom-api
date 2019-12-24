using App.DL.Enum;
using App.DL.Model.External;
using Micron.DL.Module.Misc;
using NUnit.Framework;
using Tests.Testing;
using Tests.Utils.Fake.User;

namespace Tests.App.DL.Model.External {
    public class ExternalServiceDataTests : BaseTestFixture {
        [Test]
        public void Create_DataCorrect_GotCorrectId() {
            var serviceType = ServiceType.GitLab;
            
            var user = UserFaker.Create();

            var randomId = Rand.RandomString();
            
            var id = ExternalServiceData.Create(user, serviceType, randomId, user.login);

            Assert.True(id > 0);
            
            var data = ExternalServiceData.Find(id);
            
            Assert.NotNull(data);
            
            Assert.AreEqual(user.id, data.user_id);
            Assert.AreEqual(randomId, data.origin_id);
            Assert.AreEqual(user.login, data.login);
            Assert.AreEqual(serviceType, data.service_type);
        }
    }
}