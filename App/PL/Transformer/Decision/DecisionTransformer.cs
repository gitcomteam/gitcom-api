using App.AL.Utils.Entity;
using App.DL.Model.Decision;
using Micron.PL.Transformer;
using Newtonsoft.Json.Linq;

namespace App.PL.Transformer.Decision {
    public class DecisionTransformer : BaseTransformer {
        public override JObject Transform(object obj) {
            var item = (EntityDecision) obj;
            return new JObject {
                ["guid"] = item.guid,
                ["creator_guid"] = item.Creator().guid,
                ["entity_guid"] = EntityUtils.GetEntityGuid(item.entity_id, item.entity_type),
                ["entity_type"] = item.entity_type.ToString(),
                ["options"] = new DecisionOptionTransformer().Many(item.Options()),
                ["title"] = item.title,
                ["content"] = item.content,
                ["status"] = item.status.ToString(),
                ["deadline"] = item.deadline,
                ["updated_at"] = item.updated_at,
                ["created_at"] = item.created_at
            };
        }
    }
}