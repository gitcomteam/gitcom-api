using App.DL.Model.Product;
using Micron.PL.Transformer;
using Newtonsoft.Json.Linq;

namespace App.PL.Transformer.Product {
    public class ProjectProductTransformer : BaseTransformer {
        public override JObject Transform(object obj) {
            var item = (ProjectProduct) obj;
            return new JObject {
                ["guid"] = item.guid,
                ["name"] = item.name,
                ["description"] = item.description,
                ["url"] = item.url,
                ["project_guid"] = item.Project().guid,
                ["usd_price"] = (decimal) item.usd_price_pennies / 100,
                ["duration_hours"] = item.duration_hours,
                ["created_at"] = item.created_at.ToString("d"),
                ["updated_at"] = item.created_at.ToString("d")
            };
        }
    }
}