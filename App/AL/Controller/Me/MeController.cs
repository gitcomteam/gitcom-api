using App.DL.Repository.User;
using App.PL.Transformer.User;
using Micron.DL.Middleware;
using Micron.DL.Middleware.Auth;
using Micron.DL.Module.Controller;
using Micron.DL.Module.Http;

namespace App.AL.Controller.Home {
    public sealed class MeController : BaseController {
        protected override IMiddleware[] Middleware() => new IMiddleware[] {
            new JwtMiddleware()
        };

        public MeController() {
            Get("/api/v1/me/get", _ => {
                var me = UserRepository.Find(CurrentRequest.UserId);
                return HttpResponse.Item("me", new UserTransformer().Transform(me));
            });
        }
    }
}