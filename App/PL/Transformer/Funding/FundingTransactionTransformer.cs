using App.AL.Utils.Entity;
using App.DL.Model.Funding;
using Micron.PL.Transformer;
using Newtonsoft.Json.Linq;

namespace App.PL.Transformer.Funding {
    public class FundingTransactionTransformer : BaseTransformer {
        public override JObject Transform(object obj) {
            var item = (FundingTransaction) obj;
            return new JObject {
                ["guid"] = item.guid,
                ["from_user_guid"] = item.FromUser()?.guid,
                ["entity_guid"] = EntityUtils.GetEntityGuid(item.entity_id, item.entity_type),
                ["entity_type"] = item.entity_type.ToString(),
                ["amount"] = item.amount,
                ["currency_type"] = item.currency_type.ToString(),
                ["created_at"] = item.created_at
            };
        }
    }
}