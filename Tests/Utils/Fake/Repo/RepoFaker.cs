using App.DL.Enum;
using App.DL.Repository.Repo;
using Micron.DL.Module.Misc;
using Tests.Utils.Fake.User;
using RepoModel = App.DL.Model.Repo.Repo;

namespace Tests.Utils.Fake.Repo {
    public static class RepoFaker {
        public static RepoModel Create() {
            return RepoRepository.CreateAndGet(
                UserFaker.Create(),
                "randomTitle_" + Rand.SmallInt(),
                "repoUrl_" + Rand.SmallInt(),
                RepoServiceType.GitHub
            );
        }
    }
}