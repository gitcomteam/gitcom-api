using App.DL.Repository.User;
using App.PL.Transformer.User;
using Micron.AL.Validation.Basic;
using Micron.AL.Validation.Db;
using Micron.DL.Middleware;
using Micron.DL.Module.Controller;
using Micron.DL.Module.Http;
using Micron.DL.Module.Validator;

namespace App.AL.Controller.Home {
    public sealed class UserController : BaseController {
        protected override IMiddleware[] Middleware() => new IMiddleware[] {};

        public UserController() {
            Get("/api/v1/user/get", _ => {
                var errors = ValidationProcessor.Process(Request, new IValidatorRule[] {
                    new ShouldHaveParameters(new[] {"user_guid"}),
                    new ExistsInTable("user_guid", "users", "guid")
                });
                if (errors.Count > 0) {
                    return HttpResponse.Errors(errors);
                }
                
                var user = UserRepository.FindByGuid(GetRequestStr("user_guid"));
                return HttpResponse.Item("me", new UserTransformer().Transform(user));
            });
        }
    }
}