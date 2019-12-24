using App.DL.Repository.Project;
using App.PL.Transformer.Work;
using Micron.AL.Validation.Basic;
using Micron.AL.Validation.Db;
using Micron.DL.Middleware;
using Micron.DL.Module.Controller;
using Micron.DL.Module.Http;
using Micron.DL.Module.Validator;

namespace App.AL.Controller.Work.Project {
    public class ProjectWorkTypeController : BaseController {
        protected override IMiddleware[] Middleware() => new IMiddleware[] { };

        public ProjectWorkTypeController() {
            Get("/api/v1/project/work_types/get", _ => {
                var errors = ValidationProcessor.Process(Request, new IValidatorRule[] {
                    new ShouldHaveParameters(new[] {"project_guid"}),
                    new ExistsInTable("project_guid", "projects", "guid")
                });
                if (errors.Count > 0) {
                    return HttpResponse.Errors(errors);
                }

                var project = ProjectRepository.FindByGuid(GetRequestStr("project_guid"));

                return HttpResponse.Item(
                    "work_types", new ProjectWorkTypeTransformer().Many(project.WorkTypes())
                );
            });
        }
    }
}