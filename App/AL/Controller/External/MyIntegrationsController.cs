using App.DL.Enum;
using App.DL.External.GitLab;
using App.DL.Repository.User;
using Micron.DL.Middleware;
using Micron.DL.Middleware.Auth;
using Micron.DL.Module.Controller;
using Micron.DL.Module.Http;
using Newtonsoft.Json.Linq;
using Octokit;

namespace App.AL.Controller.External {
    public class MyIntegrationsController : BaseController {
        protected override IMiddleware[] Middleware() => new IMiddleware[] { new JwtMiddleware() };

        public MyIntegrationsController() {
            Get("/api/v1/me/integrations/status/get", _ => {
                var me = UserRepository.Find(CurrentRequest.UserId);

                var githubToken = me.ServiceAccessToken(ServiceType.GitHub);
                
                JObject githubUser = null;

                if (githubToken != null) {
                    var githubClient = new GitHubClient(new ProductHeaderValue("SupportHub"));
                
                    githubClient.Credentials = new Credentials(githubToken.access_token);

                    var rawGitHubUser = githubClient.User.Current().Result;

                    githubUser = new JObject();
                    githubUser["login"] = rawGitHubUser.Login;
                    githubUser["email"] = rawGitHubUser.Email;
                }
                
                var gitlabToken = me.ServiceAccessToken(ServiceType.GitLab);
                JObject gitlabUser = null;
                
                if (gitlabToken != null) {
                    var client = new GitLabClient(gitlabToken.access_token);
                    client.SetAuthorizedUser();

                    if (client.User != null) {
                        gitlabUser = new JObject();
                        gitlabUser["login"] = client.User.Login;
                        gitlabUser["email"] = client.User.Email;
                    }
                }
                
                return HttpResponse.Item("integrations", new JObject() {
                    ["github"] = new JObject() {
                        ["connected"] = githubUser != null,
                        ["user"] = githubUser
                    },
                    ["gitlab"] = new JObject() {
                        ["connected"] = gitlabUser != null,
                        ["user"] = gitlabUser
                    }
                });
            });
        }
    }
}