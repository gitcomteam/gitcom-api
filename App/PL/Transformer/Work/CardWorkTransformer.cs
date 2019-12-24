using System.Globalization;
using App.DL.Model.Work;
using Micron.PL.Transformer;
using Newtonsoft.Json.Linq;

namespace App.PL.Transformer.Work {
    public class CardWorkTransformer : BaseTransformer {
        public override JObject Transform(object obj) {
            var item = (CardWork) obj;
            return new JObject() {
                ["guid"] = item.guid,
                ["user_guid"] = item.User().guid,
                ["card_guid"] = item.Card().guid,
                ["work_type"] = new ProjectWorkTypeTransformer().Transform(item.WorkType()),
                ["proof"] = item.proof,
                ["created_at"] = item.created_at.ToString(CultureInfo.InvariantCulture),
                ["updated_at"] = item.updated_at.ToString(CultureInfo.InvariantCulture)
            };
        }
    }
}