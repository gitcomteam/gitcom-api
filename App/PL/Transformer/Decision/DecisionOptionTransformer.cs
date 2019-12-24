using App.DL.Model.Decision;
using Micron.PL.Transformer;
using Newtonsoft.Json.Linq;

namespace App.PL.Transformer.Decision {
    public class DecisionOptionTransformer : BaseTransformer {
        public override JObject Transform(object obj) {
            var item = (EntityDecisionOption) obj;
            return new JObject {
                ["guid"] = item.guid,
                ["decision_guid"] = item.Decision().guid,
                ["title"] = item.title,
                ["order"] = item.order,
                ["created_at"] = item.created_at
            };
        }
    }
}