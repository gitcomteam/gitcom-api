using App.DL.Repository.Repo;
using App.PL.Transformer.Repo;
using Micron.AL.Validation.Db;
using Micron.DL.Middleware;
using Micron.DL.Module.Controller;
using Micron.DL.Module.Http;
using Micron.DL.Module.Validator;

namespace App.AL.Controller.Repo {
    public sealed class RepoController : BaseController {
        protected override IMiddleware[] Middleware() => new IMiddleware[] { };

        public RepoController() {
            Get("/api/v1/repository/get", _ => {
                var errors = ValidationProcessor.Process(Request, new IValidatorRule[] {
                    new ExistsInTable("repo_guid", "repositories", "guid"),
                });
                if (errors.Count > 0) {
                    return HttpResponse.Errors(errors);
                }

                return HttpResponse.Item("repository", new RepoTransformer().Transform(
                    RepoRepository.FindByGuid((string) Request.Query["repo_guid"])
                ));
            });
        }
    }
}