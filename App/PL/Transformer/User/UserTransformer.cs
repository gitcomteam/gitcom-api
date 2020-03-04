using Micron.PL.Transformer;
using Newtonsoft.Json.Linq;
using UserModel = App.DL.Model.User.User;

namespace App.PL.Transformer.User {
    public class UserTransformer : BaseTransformer {
        public override JObject Transform(object obj) {
            var item = (UserModel) obj;
            if (item == null) return null;
            return new JObject {
                ["guid"] = item.guid,
                ["login"] = item.login,
                ["email"] = item.email,
                ["register_date"] = item.register_date
            };
        }
    }
}