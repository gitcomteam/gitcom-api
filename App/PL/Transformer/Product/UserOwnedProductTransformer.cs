using App.DL.Model.Product;
using Micron.PL.Transformer;
using Newtonsoft.Json.Linq;

namespace App.PL.Transformer.Product {
    public class UserOwnedProductTransformer : BaseTransformer {
        public override JObject Transform(object obj) {
            var item = (UserOwnedProduct) obj;
            return new JObject {
                ["guid"] = item.guid,
                ["user_id"] = item.User().guid,
                ["product"] = new ProjectProductTransformer().Transform(item.Product()),
                ["expiry_at"] = item.expiry_at,
                ["created_at"] = item.created_at.ToString("d"),
                ["updated_at"] = item.created_at.ToString("d")
            };
        }
    }
}