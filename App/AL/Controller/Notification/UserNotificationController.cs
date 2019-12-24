using App.AL.Utils.Notification;
using App.DL.Repository.User;
using App.PL.Transformer.Notification;
using Micron.DL.Middleware;
using Micron.DL.Middleware.Auth;
using Micron.DL.Module.Controller;
using Micron.DL.Module.Http;
using Newtonsoft.Json.Linq;

namespace App.AL.Controller.Notification {
    public sealed class UserNotificationController : BaseController {
        protected override IMiddleware[] Middleware() => new IMiddleware[] {new JwtMiddleware()};

        public UserNotificationController() {
            Get("/api/v1/me/notifications/get", _ => {
                var me = UserRepository.Find(CurrentRequest.UserId);
                return HttpResponse.Item("notifications", new UserNotificationTransformer().Many(
                    UserNotificationUtils.GetActive(me)
                ));
            });

            Patch("/api/v1/me/notifications/seen_all", _ => {
                var me = UserRepository.Find(CurrentRequest.UserId);
                UserNotificationUtils.SeenAll(me);
                return HttpResponse.Data(new JObject());
            });
        }
    }
}