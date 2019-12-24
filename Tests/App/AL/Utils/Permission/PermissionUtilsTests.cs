using App.AL.Utils.Permission;
using App.DL.Enum;
using NUnit.Framework;
using Tests.Testing;
using Tests.Utils.Fake.Board;
using Tests.Utils.Fake.Project;
using Tests.Utils.Fake.ProjectTeamMember;
using Tests.Utils.Fake.User;

namespace Tests.App.AL.Utils.Permission {
    [TestFixture]
    public class PermissionUtilsTests : BaseTestFixture {
        [Test]
        public void HasEntityPermission_Project_HasPermission() {
            var user = UserFaker.Create();
            var project = ProjectFaker.Create();

            Assert.False(PermissionUtils.HasEntityPermission(user, project.id, EntityType.Project));

            ProjectTeamMemberFaker.Create(project, user);

            Assert.True(PermissionUtils.HasEntityPermission(user, project.id, EntityType.Project));
        }
        
        [Test]
        public void HasEntityPermission_Board_HasPermission() {
            var user = UserFaker.Create();
            var board = BoardFaker.Create();

            Assert.False(PermissionUtils.HasEntityPermission(user, board.id, EntityType.Board));

            ProjectTeamMemberFaker.Create(board.Project(), user);

            Assert.True(PermissionUtils.HasEntityPermission(user, board.id, EntityType.Board));
        }
    }
}