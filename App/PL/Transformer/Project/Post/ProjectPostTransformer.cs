using App.DL.Model.Project.Post;
using Micron.PL.Transformer;
using Newtonsoft.Json.Linq;

namespace App.PL.Transformer.Project.Post {
    public class ProjectPostTransformer : BaseTransformer {
        public override JObject Transform(object obj) {
            var item = (ProjectPost) obj;
            var result = new JObject {
                ["guid"] = item.guid,
                ["title"] = item.title,
                ["project_guid"] = item.Project().guid,
                ["content"] = item.content,
                ["created_at"] = item.created_at,
                ["updated_at"] = item.updated_at
            };
            return result;
        }
    }
}