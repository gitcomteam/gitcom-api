using App.DL.Repository.User;
using App.PL.Transformer.User.Badge;
using Micron.AL.Validation.Basic;
using Micron.AL.Validation.Db;
using Micron.DL.Middleware;
using Micron.DL.Module.Controller;
using Micron.DL.Module.Http;
using Micron.DL.Module.Validator;

namespace App.AL.Controller.User.Badge {
    public class BadgeController : BaseController {
        protected override IMiddleware[] Middleware() => new IMiddleware[] { };

        public BadgeController() {
            Get("/api/v1/user/badges/get", _ => {
                var errors = ValidationProcessor.Process(Request, new IValidatorRule[] {
                    new ShouldHaveParameters(new[] {"user_guid"}),
                    new ExistsInTable("user_guid", "users", "guid"),
                }, true);
                if (errors.Count > 0) return HttpResponse.Errors(errors);

                var user = UserRepository.FindByGuid(GetRequestStr("user_guid"));

                return HttpResponse.Item("badges", new UserBadgeTransformer().Many(user.Badges()));
            });
        }
    }
}