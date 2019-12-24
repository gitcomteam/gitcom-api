using Micron.PL.Transformer;
using Newtonsoft.Json.Linq;
using projectTeamMemberModel = App.DL.Model.ProjectTeamMember.ProjectTeamMember;

namespace App.PL.Transformer.ProjectTeamMember {
    public class ProjectTeamMemberTransformer : BaseTransformer {
        public override JObject Transform(object obj) {
            var item = (projectTeamMemberModel) obj;
            return new JObject {
                ["project_guid"] = item.Project().guid,
                ["user_guid"] = item.User().guid,
                ["created_at"] = item.created_at
            };
        }
    }
}