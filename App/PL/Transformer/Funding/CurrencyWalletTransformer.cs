using App.DL.Model.Funding;
using Micron.PL.Transformer;
using Newtonsoft.Json.Linq;

namespace App.PL.Transformer.Funding {
    public class CurrencyWalletTransformer : BaseTransformer {
        public override JObject Transform(object obj) {
            var item = (CurrencyWallet) obj;
            return new JObject {
                ["guid"] = item.guid,
                ["address"] = item.address,
                ["currency_type"] = item.currency_type.ToString()
            };
        }
    }
}