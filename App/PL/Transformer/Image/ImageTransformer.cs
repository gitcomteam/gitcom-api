using Micron.PL.Transformer;
using Newtonsoft.Json.Linq;

namespace App.PL.Transformer.Image {
    public class ImageTransformer : BaseTransformer {
        public override JObject Transform(object obj) {
            var item = (DL.Model.Image.Image) obj;
            return new JObject {
                ["guid"] = item.guid,
                ["url"] = item.url,
                ["created_at"] = item.created_at,
            };
        }
    }
}