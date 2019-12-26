using System;
using App.DL.CustomObj.Repo;
using App.DL.Enum;
using App.DL.External.GitLab;
using App.DL.Model.Project;
using App.DL.Model.Repo;
using App.DL.Model.User;
using App.DL.Repository.Project;
using App.DL.Repository.Repo;
using Newtonsoft.Json.Linq;

namespace App.AL.Utils.External.GitLab {
    public static class GitLabRepositoriesUtils {
        public static ExternalRepo[] GetUserRepositories(User user) {
            var result = new ExternalRepo[] { };

            var token = user.ServiceAccessToken(ServiceType.GitLab);

            if (token == null) {
                return result;
            }

            var client = new GitLabClient(token.access_token);
            client.SetAuthorizedUser();

            return client.GetMyPublicRepositories(user);
        }

        public static (Project project, Repo repo) ImportProject(User me, string originId) {
            var tokenModel = me.ServiceAccessToken(ServiceType.GitLab);
            var client = new GitLabClient(tokenModel.access_token);
            client.SetAuthorizedUser();

            if (tokenModel.origin_user_id == "") {
                GitLabUserUtils.UpdateOriginUserId(me);
            }

            var gitLabProject = GitlabApi.GetPublicProject(originId);

            if (gitLabProject == null) {
                return (null, null);
            }

            Repo repository = RepoRepository.CreateAndGet(
                me,
                gitLabProject.Value<string>("name"),
                gitLabProject.Value<string>("web_url"),
                RepoServiceType.GitLab,
                gitLabProject.Value<string>("id")
            );

            var originUserId = tokenModel.origin_user_id;

            User creator = null;

            var projectUsers = GitlabApi.GetProjectUsers(originId);
            
            foreach (var projectUser in projectUsers.Children()) {
                if (originUserId == projectUser.Value<string>("id")) {
                    creator = me;
                    break;
                }
            }
            
            var project = ProjectRepository.FindOrCreate(repository.title, creator, repository);

            project.UpdateCol("description", gitLabProject.Value<string>("description"));
            
            return (project, repository);
        }
    }
}