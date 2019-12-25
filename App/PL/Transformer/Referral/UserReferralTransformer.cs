using App.DL.Model.User.Referral;
using Micron.PL.Transformer;
using Newtonsoft.Json.Linq;

namespace App.PL.Transformer.Referral {
    public class UserReferralTransformer : BaseTransformer {
        public override JObject Transform(object obj) {
            var item = (UserReferral) obj;
            return new JObject {
                ["guid"] = item.guid,
                ["login"] = item.User().login,
                ["created_at"] = item.created_at
            };
        }
    }
}