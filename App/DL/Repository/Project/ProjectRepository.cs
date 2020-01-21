using App.AL.Tasks.Project;
using App.DL.Repository.ProjectTeamMember;
using UserModel = App.DL.Model.User.User;
using ProjectModel = App.DL.Model.Project.Project;
using RepoModel = App.DL.Model.Repo.Repo;

namespace App.DL.Repository.Project {
    public static class ProjectRepository {
        public static ProjectModel Find(int id) {
            return ProjectModel.Find(id);
        }

        public static ProjectModel FindByGuid(string guid) => ProjectModel.FindByGuid(guid);
        
        public static ProjectModel FindBy(string col, string val) => ProjectModel.FindBy(col, val);
        
        public static ProjectModel FindBy(string col, int val) => ProjectModel.FindBy(col, val);
        
        public static ProjectModel[] GetBy(string col, string val) => ProjectModel.GetBy(col, val);
        
        public static ProjectModel[] GetBy(string col, int val) => ProjectModel.GetBy(col, val);

        public static ProjectModel FindRandom() => ProjectModel.FindRandom();

        public static ProjectModel[] GetRandom() => ProjectModel.GetRandom();

        public static ProjectModel[] GetNewest() => ProjectModel.GetNewest();

        public static ProjectModel FindOrCreate(string name, UserModel creator, RepoModel repository) {
            var project = ProjectModel.FindBy("repository_id", repository.id) ??
                          ProjectModel.Find(ProjectModel.Create(name, creator, repository));
            ProjectSetUp.Run(project, creator);
            return project;
        }

        public static void Delete(ProjectModel project) {
            var teamMembers = ProjectTeamMemberRepository.Get(project);
            foreach (var member in teamMembers) {
                member.Delete();
            }

            project.Alias()?.Delete();

            project.Delete();
        }
    }
}