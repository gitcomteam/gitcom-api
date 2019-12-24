using App.DL.CustomObj.Repo;
using Micron.PL.Transformer;
using Newtonsoft.Json.Linq;

namespace App.PL.Transformer.External.GitHub {
    public class ExternalRepoTransformer : BaseTransformer {
        public override JObject Transform(object obj) {
            var item = (ExternalRepo) obj;
            return new JObject() {
                ["origin_id"] = item.Id,
                ["full_name"] = item.Name,
                ["description"] = item.Description,
                ["service_type"] = item.ServiceType.ToString(),
                ["owner"] = new JObject() {
                    ["login"] = item.Owner.login
                }
            };
        }
    }
}