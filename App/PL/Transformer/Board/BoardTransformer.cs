using Micron.PL.Transformer;
using Newtonsoft.Json.Linq;
using BoardModel = App.DL.Model.Board.Board;

namespace App.PL.Transformer.Board {
    public class BoardTransformer : BaseTransformer {
        public override JObject Transform(object obj) {
            var item = (BoardModel) obj;
            return new JObject {
                ["guid"] = item.guid,
                ["name"] = item.name,
                ["description"] = item.description,
                ["project_guid"] = item.Project().guid,
                ["user_guid"] = item.User().guid,
                ["created_at"] = item.created_at.ToString("d"),
                ["updated_at"] = item.updated_at.ToString("d")
            };
        }
    }
}