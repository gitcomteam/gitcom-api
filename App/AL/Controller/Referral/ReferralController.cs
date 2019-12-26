using App.DL.Repository.User;
using App.DL.Repository.User.Referral;
using App.PL.Transformer.Referral;
using Micron.DL.Middleware;
using Micron.DL.Middleware.Auth;
using Micron.DL.Module.Controller;
using Micron.DL.Module.Http;
using Newtonsoft.Json.Linq;

namespace App.AL.Controller.Referral {
    public class ReferralController : BaseController {
        protected override IMiddleware[] Middleware() => new IMiddleware[] { new JwtMiddleware() };
        
        public ReferralController() {
            Get("/api/v1/me/referral_key/get", _ => {
                var me = UserRepository.Find(CurrentRequest.UserId);
                return HttpResponse.Data(new JObject() {
                    ["referral_key"] = me.guid
                });
            });

            Get("/api/v1/me/referred/get", _ => {
                var me = UserRepository.Find(CurrentRequest.UserId);
                return HttpResponse.Data(new JObject() {
                    ["referred_users"] = new UserReferralTransformer().Many(UserReferralRepository.GetInvited(me))
                });
            });
        }
    }
}