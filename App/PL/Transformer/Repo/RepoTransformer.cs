using App.PL.Transformer.User;
using Micron.PL.Transformer;
using Newtonsoft.Json.Linq;
using RepoModel = App.DL.Model.Repo.Repo;

namespace App.PL.Transformer.Repo {
    public class RepoTransformer : BaseTransformer {
        public override JObject Transform(object obj) {
            var item = (RepoModel) obj;
            return new JObject {
                ["guid"] = item.guid,
                ["creator"] = new UserTransformer().Transform(item.Creator()),
                ["title"] = item.title,
                ["repo_url"] = item.repo_url,
                ["origin_id"] = item.origin_id,
                ["created_at"] = item.created_at.ToString("d")
            };
        }
    }
}