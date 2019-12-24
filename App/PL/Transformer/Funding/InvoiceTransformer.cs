using App.AL.Utils.Entity;
using App.DL.Model.Funding;
using Micron.PL.Transformer;
using Newtonsoft.Json.Linq;

namespace App.PL.Transformer.Funding {
    public class InvoiceTransformer : BaseTransformer {
        public override JObject Transform(object obj) {
            var item = (Invoice) obj;
            return new JObject {
                ["guid"] = item.guid,
                ["user_guid"] = item.User().guid,
                ["entity_guid"] = EntityUtils.GetEntityGuid(item.entity_id, item.entity_type),
                ["entity_type"] = item.entity_type.ToString(),
                ["amount"] = item.amount,
                ["currency_type"] = item.currency_type.ToString(),
                ["status"] = item.status.ToString(),
                ["wallet"] = new CurrencyWalletTransformer().Transform(item.Wallet()),
                ["created_at"] = item.created_at,
                ["updated_at"] = item.updated_at
            };
        }
    }
}