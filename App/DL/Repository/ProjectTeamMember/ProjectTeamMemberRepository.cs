using App.AL.Utils.Entity;
using App.DL.Enum;
using App.DL.Repository.User;
using App.DL.Repository.Project;
using Newtonsoft.Json.Linq;
using ProjectModel = App.DL.Model.Project.Project;
using UserModel = App.DL.Model.User.User;
using ProjectTeamMemberModel = App.DL.Model.ProjectTeamMember.ProjectTeamMember;

namespace App.DL.Repository.ProjectTeamMember {
    public static class ProjectTeamMemberRepository {
        public static ProjectTeamMemberModel Find(int id) {
            return ProjectTeamMemberModel.Find(id);
        }

        public static ProjectTeamMemberModel FindByProjectAndUser(string projectGuid, string userGuid) {
            var user = UserRepository.FindByGuid(userGuid);
            return ProjectTeamMemberModel.Find(
                ProjectRepository.Find(EntityUtils.GetEntityId(projectGuid, EntityType.Project)), user
            );
        }

        public static ProjectTeamMemberModel Find(ProjectModel project, UserModel user) {
            return ProjectTeamMemberModel.Find(project, user);
        }

        public static ProjectTeamMemberModel[] Get(ProjectModel project) {
            return ProjectTeamMemberModel.Get(project);
        }

        public static bool IsExists(ProjectModel project, UserModel user) {
            return ProjectTeamMemberModel.IsExist(project, user);
        }

        public static ProjectTeamMemberModel CreateAndGet(ProjectModel project, UserModel user) {
            return ProjectTeamMemberModel.Find(ProjectTeamMemberModel.Create(project, user));
        }

        public static ProjectTeamMemberModel UpdateAndRefresh(ProjectTeamMemberModel model, JObject data) {
            model.project_id = data.Value<int?>("project_id") ?? model.project_id;
            model.user_id = data.Value<int?>("user_id") ?? model.user_id;
            return model.Save();
        }
    }
}