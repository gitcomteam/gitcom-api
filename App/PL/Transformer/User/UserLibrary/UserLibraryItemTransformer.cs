using App.DL.Model.UserLibrary;
using Micron.PL.Transformer;
using Newtonsoft.Json.Linq;

namespace App.PL.Transformer.UserLibrary {
    public class UserLibraryItemTransformer : BaseTransformer {
        public override JObject Transform(object obj) {
            var item = (UserLibraryItem) obj;
            return new JObject {
                ["guid"] = item.guid,
                ["user_guid"] = item.guid,
                ["project_guid"] = item.Project().guid,
                ["created_at"] = item.created_at
            };
        }
    }
}