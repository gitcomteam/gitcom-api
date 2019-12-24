using App.AL.Utils.Entity;
using App.DL.Enum;
using App.DL.Model.Setting;
using Micron.PL.Transformer;
using Newtonsoft.Json.Linq;

namespace App.PL.Transformer.Setting {
    public class UserSettingTransformer : BaseTransformer {
        public override JObject Transform(object obj) {
            var item = (UserSetting) obj;
            return new JObject {
                ["guid"] = item.guid,
                ["user_guid"] = EntityUtils.GetEntityGuid(item.user_id, EntityType.User),
                ["key"] = item.key,
                ["value"] = item.value,
            };
        }
    }
}