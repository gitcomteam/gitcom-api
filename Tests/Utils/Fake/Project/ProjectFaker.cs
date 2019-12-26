using App.DL.Repository.Project;
using Micron.DL.Module.Misc;
using Tests.Utils.Fake.User;
using Tests.Utils.Fake.Repo;
using UserModel = App.DL.Model.User.User;
using ProjectModel = App.DL.Model.Project.Project;
using RepoModel = App.DL.Model.Repo.Repo;

namespace Tests.Utils.Fake.Project {
    public static class ProjectFaker {
        public static ProjectModel Create(UserModel user = null, RepoModel repository = null)
        {
            user = user ?? UserFaker.Create();
            repository = repository ?? RepoFaker.Create();
            return ProjectRepository.FindOrCreate(
                "repoName_" + Rand.SmallInt(),
                user,
                repository
            );
        }
    }
}