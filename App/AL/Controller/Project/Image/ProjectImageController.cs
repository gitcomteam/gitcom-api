using App.DL.Repository.Project;
using App.PL.Transformer.Image;
using Micron.AL.Validation.Db;
using Micron.DL.Middleware;
using Micron.DL.Module.Controller;
using Micron.DL.Module.Http;
using Micron.DL.Module.Validator;

namespace App.AL.Controller.Project.Image {
    public class ProjectImageController : BaseController {
        protected override IMiddleware[] Middleware() => new IMiddleware[] { };

        public ProjectImageController() {
            Get("/api/v1/project/images/get", _ => {
                var errors = ValidationProcessor.Process(Request, new IValidatorRule[] {
                    new ExistsInTable("project_guid", "projects", "guid"),
                });
                if (errors.Count > 0) return HttpResponse.Errors(errors);

                var project = ProjectRepository.FindByGuid(GetRequestStr("project_guid"));

                return HttpResponse.Item("images", new ImageTransformer().Many(project.Images()));
            });
        }
    }
}