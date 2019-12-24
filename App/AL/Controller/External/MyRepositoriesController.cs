using System.Linq;
using App.AL.Utils.External.GitHub;
using App.AL.Utils.External.GitLab;
using App.DL.Repository.User;
using App.PL.Transformer.External.GitHub;
using Micron.DL.Middleware;
using Micron.DL.Middleware.Auth;
using Micron.DL.Module.Controller;
using Micron.DL.Module.Http;

namespace App.AL.Controller.External {
    public class MyRepositoriesController : BaseController {
        protected override IMiddleware[] Middleware() => new IMiddleware[] { new JwtMiddleware() };

        public MyRepositoriesController() {
            Get("/api/v1/me/external/repositories/get", _ => {
                var me = UserRepository.Find(CurrentRequest.UserId);

                var repos = GitHubRepositoriesUtils.GetUserRepositories(me);

                repos = repos.Concat(
                    GitLabRepositoriesUtils.GetUserRepositories(me)
                ).ToArray();
                
                return HttpResponse.Item("repositories", new ExternalRepoTransformer().Many(repos));
            });
        }
    }
}