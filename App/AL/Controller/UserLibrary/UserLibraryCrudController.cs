using App.DL.Repository.Project;
using App.DL.Repository.User;
using App.DL.Repository.UserLibrary;
using App.PL.Transformer.UserLibrary;
using Micron.AL.Validation.Basic;
using Micron.AL.Validation.Db;
using Micron.DL.Middleware;
using Micron.DL.Middleware.Auth;
using Micron.DL.Module.Controller;
using Micron.DL.Module.Http;
using Micron.DL.Module.Validator;
using Nancy;
using Newtonsoft.Json.Linq;

namespace App.AL.Controller.UserLibrary {
    public class UserLibraryCrudController : BaseController {
        protected override IMiddleware[] Middleware() => new IMiddleware[] {
            new JwtMiddleware()
        };

        public UserLibraryCrudController() {
            Get("/api/v1/me/library/projects/get", _ => {
                var me = UserRepository.Find(CurrentRequest.UserId);

                var items = UserLibraryItemRepository.Get(me);

                return HttpResponse.Item("library_projects", new UserLibraryItemTransformer().Many(items));
            });
            
            Get("/api/v1/me/library/project/status/get", _ => {
                var errors = ValidationProcessor.Process(Request, new IValidatorRule[] {
                    new ExistsInTable("project_guid", "projects", "guid")
                });
                if (errors.Count > 0) {
                    return HttpResponse.Errors(errors);
                }

                var me = UserRepository.Find(CurrentRequest.UserId);
                var project = ProjectRepository.FindByGuid(GetRequestStr("project_guid"));

                return HttpResponse.Data(new JObject() {
                    ["status"] = new JObject() {
                        ["in_library"] = project.InLibrary(me)
                    }
                });
            });

            Get("/api/v1/me/library/project/get", _ => {
                var errors = ValidationProcessor.Process(Request, new IValidatorRule[] {
                    new ShouldHaveParameters(new[] {"project_guid"}),
                    new ExistsInTable("project_guid", "projects", "guid")
                }, true);
                if (errors.Count > 0) return HttpResponse.Errors(errors);

                var me = UserRepository.Find(CurrentRequest.UserId);
                var project = ProjectRepository.FindByGuid(GetRequestStr("project_guid"));
                var item = UserLibraryItemRepository.Find(me, project);

                if (item == null) {
                    return HttpResponse.Error(HttpStatusCode.NotFound, "Project does not exist in your library");
                }

                return HttpResponse.Item("library_project", new UserLibraryItemTransformer().Transform(item));
            });

            Post("/api/v1/me/library/project/add", _ => {
                var errors = ValidationProcessor.Process(Request, new IValidatorRule[] {
                    new ShouldHaveParameters(new[] {"project_guid"}),
                    new ExistsInTable("project_guid", "projects", "guid"),
                }, true);
                if (errors.Count > 0) return HttpResponse.Errors(errors);

                var me = UserRepository.Find(CurrentRequest.UserId);
                var project = ProjectRepository.FindByGuid(GetRequestStr("project_guid"));

                var item = UserLibraryItemRepository.FindOrCreate(me, project);

                return HttpResponse.Item(
                    "library_project", new UserLibraryItemTransformer().Transform(item), HttpStatusCode.Created
                );
            });

            Delete("/api/v1/me/library/project/remove", _ => {
                var errors = ValidationProcessor.Process(Request, new IValidatorRule[] {
                    new ShouldHaveParameters(new[] {"project_guid"}),
                    new ExistsInTable("project_guid", "projects", "guid")
                }, true);
                if (errors.Count > 0) return HttpResponse.Errors(errors);

                var me = UserRepository.Find(CurrentRequest.UserId);

                var project = ProjectRepository.FindByGuid(GetRequestStr("project_guid"));

                var item = UserLibraryItemRepository.Find(me, project);
                if (item == null) {
                    return HttpResponse.Error(HttpStatusCode.NotFound, "Project does not exist in your library");
                }
                item.Delete();

                return HttpResponse.Item("library_project", new UserLibraryItemTransformer().Transform(item));
            });
        }
    }
}