using App.AL.Utils.External.GitHub;
using App.AL.Utils.External.GitLab;
using App.DL.Enum;
using App.DL.External.GitLab;
using App.DL.Repository.Repo;
using App.DL.Repository.User;
using App.PL.Transformer.Project;
using App.PL.Transformer.Repo;
using ProjectModel = App.DL.Model.Project.Project;
using Micron.AL.Validation.Basic;
using Micron.AL.Validation.String;
using Micron.DL.Middleware;
using Micron.DL.Middleware.Auth;
using Micron.DL.Module.Controller;
using Micron.DL.Module.Http;
using Micron.DL.Module.Validator;
using Nancy;
using Newtonsoft.Json.Linq;

namespace App.AL.Controller.Repo.Import {
    public sealed class ImportRepoController : BaseController {
        protected override IMiddleware[] Middleware() => new IMiddleware[] {new JwtMiddleware()};

        public ImportRepoController() {
            Post("/api/v1/repository/import", _ => {
                var errors = ValidationProcessor.Process(Request, new IValidatorRule[] {
                    new ShouldHaveParameters(new[] {"service_type", "origin_id"}),
                    new ShouldBeCorrectEnumValue("service_type", typeof(RepoServiceType))
                }, true);
                if (errors.Count > 0) {
                    return HttpResponse.Errors(errors);
                }

                var originId = GetRequestStr("origin_id");

                RepoServiceType serviceType =
                    (RepoServiceType) GetRequestEnum("service_type", typeof(RepoServiceType));

                var existingRepo = RepoRepository.Find(originId, serviceType);
                
                if (existingRepo != null) {
                    return HttpResponse.Error(HttpStatusCode.UnprocessableEntity, "Project is already imported",
                        new JObject() {
                            ["project_guid"] = existingRepo.Project().guid
                        });
                }

                if (serviceType == RepoServiceType.GitHub && !GitHubRepositoriesUtils.IfRepoExists(originId)) {
                    return HttpResponse.Error(
                        HttpStatusCode.NotFound, "GitHub repository with this id does not exist"
                    );
                }

                if (serviceType == RepoServiceType.GitLab && !GitlabApi.IfRepoExists(originId)) {
                    return HttpResponse.Error(
                        HttpStatusCode.NotFound, "GitLab repository with this id does not exist"
                    );
                }

                var me = UserRepository.Find(CurrentRequest.UserId);

                (ProjectModel project, DL.Model.Repo.Repo repo) result = (null, null);

                switch (serviceType) {
                    case RepoServiceType.GitHub:
                        result = GitHubRepositoriesUtils.ImportProject(me, originId);
                        break;
                    case RepoServiceType.GitLab:
                        result = GitLabRepositoriesUtils.ImportProject(me, originId);
                        break;
                }

                return HttpResponse.Data(new JObject() {
                    ["project"] = new ProjectTransformer().Transform(result.project),
                    ["repository"] = new RepoTransformer().Transform(result.repo)
                });
            });
        }
    }
}