using App.DL.Repository.User;
using App.PL.Transformer.Project;
using Micron.AL.Validation.Db;
using Micron.DL.Middleware;
using Micron.DL.Module.Controller;
using Micron.DL.Module.Http;
using Micron.DL.Module.Validator;

namespace App.AL.Controller.Project {
    public class UserProjectsController : BaseController {
        protected override IMiddleware[] Middleware() => new IMiddleware[] {};

        public UserProjectsController() {
            Get("/api/v1/user/projects/get", _ => {
                var errors = ValidationProcessor.Process(Request, new IValidatorRule[] {
                    new ExistsInTable("user_guid", "users", "guid")
                });
                if (errors.Count > 0) return HttpResponse.Errors(errors);

                var user = UserRepository.FindByGuid(GetRequestStr("user_guid"));
                
                return HttpResponse.Item("projects", new ProjectTransformer().Many(user.Projects()));
            });
        }
    }
}