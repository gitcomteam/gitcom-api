using App.DL.Model.User.Badge;
using Micron.PL.Transformer;
using Newtonsoft.Json.Linq;

namespace App.PL.Transformer.User.Badge {
    public class UserBadgeTransformer : BaseTransformer {
        public override JObject Transform(object obj) {
            var item = (UserBadge) obj;
            return new JObject() {
                ["guid"] = item.guid,
                ["user_guid"] = item.User().id,
                ["badge"] = item.badge,
                ["created_at"] = item.created_at.ToString("d"),
            };
        }
    }
}