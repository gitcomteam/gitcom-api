using App.AL.Utils.Work;
using App.DL.Repository.Project;
using NUnit.Framework;
using Tests.Testing;
using Tests.Utils.Fake.Repo;
using Tests.Utils.Fake.User;

namespace Tests.App.AL.Utils.Work {
    public class ProjectWorkUtilsTests : BaseTestFixture {
        [Test]
        public void SetUp_DataCorrect_WorkTypesCreated() {
            var project = ProjectRepository.FindOrCreate(
                "tst project", UserFaker.Create(), RepoFaker.Create()
            );
            
            Assert.AreEqual(ProjectWorkUtils.GetDefaultWorkTypes().Length, project.WorkTypes().Length);
        }
    }
}