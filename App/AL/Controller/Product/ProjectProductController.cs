using App.DL.Repository.Project;
using App.PL.Transformer.Product;
using Micron.AL.Validation.Basic;
using Micron.AL.Validation.Db;
using Micron.DL.Middleware;
using Micron.DL.Module.Controller;
using Micron.DL.Module.Http;
using Micron.DL.Module.Validator;

namespace App.AL.Controller.Product {
    public class ProjectProductController : BaseController {
        protected override IMiddleware[] Middleware() => new IMiddleware[] { };

        public ProjectProductController() {
            Get("/api/v1/project/products/get", _ => {
                var errors = ValidationProcessor.Process(Request, new IValidatorRule[] {
                    new ShouldHaveParameters(new[] {"project_guid"}),
                    new ExistsInTable("project_guid", "projects", "guid"),
                });
                if (errors.Count > 0) return HttpResponse.Errors(errors);

                var projectGuid = GetRequestStr("project_guid");

                var project = ProjectRepository.FindByGuid(projectGuid);

                return HttpResponse.Item(
                    "products", new ProjectProductTransformer().Many(project.Products())
                );
            });
        }
    }
}