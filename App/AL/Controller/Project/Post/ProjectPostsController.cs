using App.DL.Repository.Project;
using App.PL.Transformer.Project.Post;
using Micron.AL.Validation.Db;
using Micron.DL.Middleware;
using Micron.DL.Module.Controller;
using Micron.DL.Module.Http;
using Micron.DL.Module.Validator;

namespace App.AL.Controller.Project.Post {
    public class ProjectPostsController : BaseController {
        protected override IMiddleware[] Middleware() => new IMiddleware[] {};

        public ProjectPostsController() {
            Get("/api/v1/project/posts/get", _ => {
                var errors = ValidationProcessor.Process(Request, new IValidatorRule[] {
                    new ExistsInTable("project_guid", "projects", "guid"),
                });
                if (errors.Count > 0) return HttpResponse.Errors(errors);

                var project = ProjectRepository.FindByGuid(GetRequestStr("project_guid"));
                
                return HttpResponse.Item("posts", new ProjectPostTransformer().Many(project.Posts()));
            });
        }
    }
}