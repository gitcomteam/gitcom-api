using Micron.PL.Transformer;
using Newtonsoft.Json.Linq;
using BoardColumnModel = App.DL.Model.BoardColumn.BoardColumn;

namespace App.PL.Transformer.BoardColumn
{
    public class BoardColumnTransformer : BaseTransformer
    {
        public override JObject Transform(object obj)
        {
            var item = (BoardColumnModel) obj;
            return new JObject
            {
                ["guid"] = item.guid,
                ["name"] = item.name,
                ["board_guid"] = item.Board().guid,
                ["board_order"] = item.board_order
            };
        }
    }
}