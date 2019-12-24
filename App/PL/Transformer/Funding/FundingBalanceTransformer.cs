using App.AL.Utils.Entity;
using App.DL.Model.Funding;
using Micron.PL.Transformer;
using Newtonsoft.Json.Linq;

namespace App.PL.Transformer.Funding {
    public class FundingBalanceTransformer : BaseTransformer {
        public override JObject Transform(object obj) {
            var item = (FundingBalance) obj;
            return new JObject {
                ["guid"] = item.guid,
                ["entity_guid"] = EntityUtils.GetEntityGuid(item.entity_id, item.entity_type),
                ["entity_type"] = item.entity_type.ToString(),
                ["currency_type"] = item.currency_type.ToString(),
                ["amount"] = item.balance,
                ["created_at"] = item.created_at,
                ["updated_at"] = item.updated_at
            };
        }
    }
}