using App.DL.Repository.ProjectTeamMember;

using ProjectModel = App.DL.Model.Project.Project;
using UserModel = App.DL.Model.User.User;
using Tests.Utils.Fake.Project;
using Tests.Utils.Fake.User;
using ProjectTeamMemberModel = App.DL.Model.ProjectTeamMember.ProjectTeamMember;

namespace Tests.Utils.Fake.ProjectTeamMember {
    public static class ProjectTeamMemberFaker {
        public static ProjectTeamMemberModel Create(ProjectModel project = null, UserModel user = null)
        {
            user = user ?? UserFaker.Create();
            project = project ?? ProjectFaker.Create();
            return ProjectTeamMemberRepository.CreateAndGet(
                project,
                user
            );
        }
    }
}