using Micron.PL.Transformer;
using Newtonsoft.Json.Linq;
using CardModel = App.DL.Model.Card.Card;
using UserModel = App.DL.Model.User.User;

namespace App.PL.Transformer.Card
{
    public class CardTransformer : BaseTransformer
    {
        public override JObject Transform(object obj)
        {
            var item = (CardModel) obj;
            return new JObject
            {
                ["guid"] = item.guid,
                ["name"] = item.name,
                ["description"] = item.description,
                ["creator_guid"] = item.Creator()?.guid,
                ["column_order"] = item.column_order,
                ["column_guid"] = item.Column().guid,
                ["created_at"] = item.created_at,
                ["updated_at"] = item.updated_at,
            };
        }
    }
}