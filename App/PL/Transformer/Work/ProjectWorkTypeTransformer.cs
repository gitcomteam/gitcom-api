using System.Globalization;
using App.DL.Model.Work;
using Micron.PL.Transformer;
using Newtonsoft.Json.Linq;

namespace App.PL.Transformer.Work {
    public class ProjectWorkTypeTransformer : BaseTransformer {
        public override JObject Transform(object obj) {
            var item = (ProjectWorkType) obj;
            return new JObject() {
                ["guid"] = item.guid,
                ["project_guid"] = item.Project().guid,
                ["title"] = item.title,
                ["budget_percent"] = item.budget_percent,
                ["created_at"] = item.created_at.ToString(CultureInfo.InvariantCulture),
                ["updated_at"] = item.updated_at.ToString(CultureInfo.InvariantCulture),
            };
        }
    }
}