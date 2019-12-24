using App.DL.Enum;
using App.DL.Repository.Repo;
using Micron.DL.Module.Misc;
using NUnit.Framework;
using Tests.Testing;
using Tests.Utils.Fake.User;

namespace Tests.App.DL.Model.Repo {
    [TestFixture]
    public class RepoTests : BaseTestFixture {
        [Test]
        public void Find_DataCorrect_GotServiceTypeCorrect() {
            var user = UserFaker.Create();
            var repo = RepoRepository.CreateAndGet(
                user, "some_title_" + Rand.SmallInt(), "random_url_" + Rand.SmallInt(), RepoServiceType.GitLab
            );
            repo = repo.Refresh();
            Assert.AreEqual(RepoServiceType.GitLab, repo.service_type);
        }
    }
}