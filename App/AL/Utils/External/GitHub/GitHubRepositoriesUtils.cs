using System;
using App.DL.CustomObj.Repo;
using App.DL.Enum;
using App.DL.Model.Repo;
using App.DL.Repository.Project;
using App.DL.Repository.Repo;
using App.PL.CustomTransformer.External;
using Octokit;
using Project = App.DL.Model.Project.Project;
using User = App.DL.Model.User.User;

namespace App.AL.Utils.External.GitHub {
    public static class GitHubRepositoriesUtils {
        private static GitHubClient GetClient() {
            return new GitHubClient(new ProductHeaderValue("SupportHub"));
        }
        
        public static ExternalRepo[] GetUserRepositories(User user) {
            var token = user.ServiceAccessToken(ServiceType.GitHub);

            if (token == null) {
                return new ExternalRepo[] { };
            }

            var gitHubClient = GetClient();
            gitHubClient.Credentials = new Credentials(token.access_token);

            var gitHubUser = gitHubClient.User.Current().Result;

            var repos = gitHubClient.Repository.GetAllForUser(gitHubUser.Login).Result;

            return ExternalRepoTransformer.CreateFromMany(user, repos);
        }

        public static bool IfRepoExists(string originId) {
            Repository result;
            try {
                result = GetClient().Repository.Get(Convert.ToInt64(originId)).Result;
            }
            catch (Exception e) {
                return false;
            }

            return result.Id > 0;
        }

        public static (Project project, Repo repo) ImportProject(User me, string originId) {
            var externalRepo = GetClient().Repository.Get(Convert.ToInt64(originId)).Result;

            var token = me.ServiceAccessToken(ServiceType.GitHub);

            if (token.origin_user_id == "") {
                GitHubUserUtils.UpdateOriginUserId(me);
            }

            Repo repository = RepoRepository.CreateAndGet(
                me, externalRepo.Name, externalRepo.HtmlUrl, RepoServiceType.GitHub, externalRepo.Id.ToString()
            );

            var originUserId = token.origin_user_id;

            User creator = externalRepo.Owner.Id == Convert.ToInt64(originUserId) ? me : null;

            var project = ProjectRepository.FindOrCreate(repository.title, creator, repository);

            project.UpdateCol("description", externalRepo.Description);
            
            return (project, repository);
        }
    }
}