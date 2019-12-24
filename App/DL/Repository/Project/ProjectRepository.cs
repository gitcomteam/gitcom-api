using App.AL.Utils.Work;
using App.DL.Repository.ProjectTeamMember;
using Newtonsoft.Json.Linq;
using UserModel = App.DL.Model.User.User;
using ProjectModel = App.DL.Model.Project.Project;
using RepoModel = App.DL.Model.Repo.Repo;

namespace App.DL.Repository.Project {
    public static class ProjectRepository {
        public static ProjectModel Find(int id) {
            return ProjectModel.Find(id);
        }

        public static ProjectModel FindByGuid(string guid) {
            return ProjectModel.FindByGuid(guid);
        }
        
        public static ProjectModel FindRandom() => ProjectModel.FindRandom();

        public static ProjectModel[] GetRandom() => ProjectModel.GetRandom();

        public static ProjectModel[] GetNewest() => ProjectModel.GetNewest();

        public static ProjectModel CreateAndGet(string name, UserModel creator, RepoModel repository) {
            var project = ProjectModel.Find(ProjectModel.Create(name, creator, repository));
            ProjectTeamMemberRepository.CreateAndGet(project, creator);
            ProjectWorkUtils.SetUp(project);
            return project;
        }

        public static ProjectModel UpdateAndRefresh(ProjectModel model, JObject data) {
            model.name = data.Value<string>("name") ?? model.name;
            return model.Save();
        }

        public static void Delete(ProjectModel project) {
            var teamMembers = ProjectTeamMemberRepository.Get(project);
            foreach (var member in teamMembers) {
                member.Delete();
            }
            project.Delete();
        }
    }
}