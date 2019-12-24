using Micron.PL.Transformer;
using Newtonsoft.Json.Linq;

namespace App.PL.Transformer.Project {
    public class ProjectTransformer : BaseTransformer {
        public override JObject Transform(object obj) {
            var item = (DL.Model.Project.Project) obj;
            var result = new JObject {
                ["guid"] = item.guid,
                ["name"] = item.name,
                ["description"] = item.description,
                ["repository_guid"] = item.Repository()?.guid,
                ["creator_guid"] = item.Creator()?.guid,
                ["base_uri"] = null,
                ["created_at"] = item.created_at.ToString("d"),
                ["updated_at"] = item.created_at.ToString("d")
            };

            var alias = item.Alias();
            if (alias != null) {
                result["base_uri"] = $"{alias.owner}/{alias.alias}";
            }

            return result;
        }
    }
}