using System.Globalization;
using App.DL.Model.Withdraw;
using Micron.PL.Transformer;
using Newtonsoft.Json.Linq;

namespace App.PL.Transformer.Withdraw {
    public class WithdrawalRequestTransformer : BaseTransformer {
        public override JObject Transform(object obj) {
            var item = (WithdrawalRequest) obj;
            return new JObject() {
                ["guid"] = item.guid,
                ["user_guid"] = item.User().guid,
                ["amount"] = item.amount,
                ["paid"] = item.paid,
                ["currency_type"] = item.currency_type.ToString(),
                ["created_at"] = item.created_at.ToString(CultureInfo.InvariantCulture),
            };
        }
    }
}