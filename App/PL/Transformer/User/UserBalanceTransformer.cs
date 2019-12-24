using App.AL.Utils.Entity;
using App.DL.Enum;
using App.DL.Model.User;
using Micron.PL.Transformer;
using Newtonsoft.Json.Linq;
using UserModel = App.DL.Model.User.User;

namespace App.PL.Transformer.User {
    public class UserBalanceTransformer : BaseTransformer {
        public override JObject Transform(object obj) {
            var item = (UserBalance) obj;
            return new JObject {
                ["guid"] = item.guid,
                ["user_guid"] = EntityUtils.GetEntityGuid(item.user_id, EntityType.User),
                ["balance"] = item.balance,
                ["currency_type"] = item.currency_type.ToString(),
                ["created_at"] = item.created_at,
                ["updated_at"] = item.updated_at,
            };
        }
    }
}