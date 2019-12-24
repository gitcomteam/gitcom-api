using App.DL.Repository.ProjectTeamMember;
using NUnit.Framework;
using Tests.Testing;
using Tests.Utils.Fake.User;
using Tests.Utils.Fake.Project;

namespace Tests.App.DL.Model.ProjectTeamMember {
    [TestFixture]
    public class ProjectTeamMemberTests : BaseTestFixture {
        [Test]
        public void Find_DataCorrect()
        {
            var user = UserFaker.Create();
            var project = ProjectFaker.Create();
            ProjectTeamMemberRepository.CreateAndGet(project, user);
            var teamMember = ProjectTeamMemberRepository.FindByProjectAndUser(project.guid, user.guid);
            Assert.AreEqual(teamMember.project_id, project.id);
            Assert.AreEqual(teamMember.user_id, user.id);
        }
    }
}