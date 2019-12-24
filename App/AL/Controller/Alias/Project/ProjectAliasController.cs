using App.DL.Repository.Alias;
using App.PL.Transformer.Project;
using Micron.AL.Validation.Basic;
using Micron.DL.Middleware;
using Micron.DL.Module.Controller;
using Micron.DL.Module.Http;
using Micron.DL.Module.Validator;
using Nancy;

namespace App.AL.Controller.Alias.Project {
    public class ProjectAliasController : BaseController {
        protected override IMiddleware[] Middleware() => new IMiddleware[] { };

        public ProjectAliasController() {
            Get("/api/v1/alias/project/get", _ => {
                var errors = ValidationProcessor.Process(Request, new IValidatorRule[] {
                    new ShouldHaveParameters(new[] {"owner", "alias"})
                });
                if (errors.Count > 0) return HttpResponse.Errors(errors);

                var alias = ProjectAliasRepository.FindByAlias(
                    GetRequestStr("owner"), GetRequestStr("alias")
                );

                if (alias == null) {
                    return HttpResponse.Error(HttpStatusCode.NotFound, "Project not found");
                }
                
                return HttpResponse.Item("project", new ProjectTransformer().Transform(alias.Project()));
            });
        }
    }
}