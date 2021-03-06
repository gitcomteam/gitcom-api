using App.DL.Repository.Project;
using App.PL.Transformer.Project;
using Micron.AL.Validation.Db;
using Micron.DL.Middleware;
using Micron.DL.Module.Controller;
using Micron.DL.Module.Http;
using Micron.DL.Module.Validator;
using Newtonsoft.Json.Linq;

namespace App.AL.Controller.Project {
    public sealed class ProjectController : BaseController {
        protected override IMiddleware[] Middleware() => new IMiddleware[] { };

        public ProjectController() {
            Get("/api/v1/project/get", _ => {
                var errors = ValidationProcessor.Process(Request, new IValidatorRule[] {
                    new ExistsInTable("project_guid", "projects", "guid"),
                });
                if (errors.Count > 0) return HttpResponse.Errors(errors);

                return HttpResponse.Item("project", new ProjectTransformer().Transform(
                    ProjectRepository.FindByGuid((string) Request.Query["project_guid"])
                ));
            });

            Get("/api/v1/projects/random/get", _ => {
                return HttpResponse.Item("projects", new ProjectTransformer().Many(
                    ProjectRepository.GetRandom()
                ));
            });

            Get("/api/v1/projects/newest/get", _ => {
                var page = GetRequestInt("page");
                page = page > 0 ? page : 1;

                var pageSize = 25;
                return HttpResponse.Data(new JObject() {
                    ["projects"] = new ProjectTransformer().Many(
                        ProjectRepository.GetNewest(page, pageSize)
                    ),
                    ["meta"] = new JObject() {
                        ["pages_count"] = (DL.Model.Project.Project.Count() / pageSize)+1,
                        ["current_page"] = page
                    }
                });
            });
        }
    }
}