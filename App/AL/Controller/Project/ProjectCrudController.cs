using App.DL.Repository.Project;
using App.PL.Transformer.Project;
using Micron.AL.Validation.Db;
using Micron.DL.Middleware;
using Micron.DL.Middleware.Auth;
using Micron.DL.Module.Controller;
using Micron.DL.Module.Http;
using Micron.DL.Module.Validator;

namespace App.AL.Controller.Project {
    public sealed class ProjectCrudController : BaseController {
        protected override IMiddleware[] Middleware() => new IMiddleware[] {
            new JwtMiddleware()
        };

        public ProjectCrudController() {
            Patch("/api/v1/project/edit", _ => {
                var errors = ValidationProcessor.Process(Request, new IValidatorRule[] {
                    new ExistsInTable("project_guid", "projects", "guid")
                });
                if (errors.Count > 0) return HttpResponse.Errors(errors);

                var project = ProjectRepository.FindByGuid(GetRequestStr("project_guid"));

                var description = GetRequestStr("description");

                if (!string.IsNullOrEmpty(description)) {
                    project.UpdateCol("description", description);
                }

                return HttpResponse.Item("project", new ProjectTransformer().Transform(project.Refresh()));
            });
            
            Delete("/api/v1/project/delete", _ => {
                var errors = ValidationProcessor.Process(Request, new IValidatorRule[] {
                    new ExistsInTable("project_guid", "projects", "guid"),
                });
                if (errors.Count > 0) return HttpResponse.Errors(errors);

                var project = ProjectRepository.FindByGuid((string) Request.Query["project_guid"]);
                ProjectRepository.Delete(project);

                return HttpResponse.Item("project", new ProjectTransformer().Transform(project));
            });
        }
    }
}