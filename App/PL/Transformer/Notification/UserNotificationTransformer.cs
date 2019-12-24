using App.DL.Model.Noficication;
using Micron.PL.Transformer;
using Newtonsoft.Json.Linq;

namespace App.PL.Transformer.Notification {
    public class UserNotificationTransformer : BaseTransformer {
        public override JObject Transform(object obj) {
            var item = (UserNotification) obj;
            return new JObject {
                ["guid"] = item.guid,
                ["user_guid"] = item.User().guid,
                ["title"] = item.title,
                ["content"] = item.content,
                ["seen"] = item.seen,
                ["type"] = item.type.ToString(),
                ["created_at"] = item.created_at,
                ["updated_at"] = item.updated_at
            };
        }
    }
}