using App.AL.Validation.Permission;
using App.DL.Enum;
using App.DL.Model.Project.Post;
using App.DL.Repository.Project;
using App.DL.Repository.User;
using App.PL.Transformer.Project.Post;
using Micron.AL.Validation.Basic;
using Micron.AL.Validation.Db;
using Micron.DL.Middleware;
using Micron.DL.Middleware.Auth;
using Micron.DL.Module.Controller;
using Micron.DL.Module.Http;
using Micron.DL.Module.Validator;
using Nancy;

namespace App.AL.Controller.Project.Post {
    public class ProjectCrudController : BaseController {
        protected override IMiddleware[] Middleware() => new IMiddleware[] {
            new JwtMiddleware(),
        };

        public ProjectCrudController() {
            Post("/api/v1/project/post/new", _ => {
                var me = UserRepository.Find(CurrentRequest.UserId);
                var project = ProjectRepository.FindByGuid(GetRequestStr("project_guid"));
                if (project == null) return HttpResponse.Error(HttpStatusCode.NotFound, "Project not found");

                var errors = ValidationProcessor.Process(Request, new IValidatorRule[] {
                    new ShouldHaveParameters(new[] {"project_guid", "title", "content"}),
                    new ExistsInTable("project_guid", "projects", "guid"),
                    new HasPermission(me, project.id, EntityType.Project),
                }, true);
                if (errors.Count > 0) return HttpResponse.Errors(errors);

                var post = ProjectPost.Create(
                    project, GetRequestStr("title"), GetRequestStr("content")
                );

                return HttpResponse.Item(
                    "post", new ProjectPostTransformer().Transform(post), HttpStatusCode.Created
                );
            });
            
            Delete("/api/v1/project/post/delete", _ => {
                var post = ProjectPost.FindBy("guid", GetRequestStr("post_guid"));
                if (post == null) return HttpResponse.Error(HttpStatusCode.NotFound, "Post not found");
                
                var me = UserRepository.Find(CurrentRequest.UserId);
                
                var errors = ValidationProcessor.Process(Request, new IValidatorRule[] {
                    new ShouldHaveParameters(new[] {"post_guid"}),
                    new HasPermission(me, post.project_id, EntityType.Project),
                }, true);
                if (errors.Count > 0) return HttpResponse.Errors(errors);

                post.Delete();
                
                return HttpResponse.Item("post", new ProjectPostTransformer().Transform(post));
            });
        }
    }
}