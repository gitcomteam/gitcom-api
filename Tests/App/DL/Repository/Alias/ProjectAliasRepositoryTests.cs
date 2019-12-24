using App.DL.Repository.Alias;
using NUnit.Framework;
using Tests.Testing;
using Tests.Utils.Fake.Project;

namespace Tests.App.DL.Repository.Alias {
    public class ProjectAliasRepositoryTests : BaseTestFixture {
        [Test]
        public void Create_DataCorrect_GotAlias() {
            var project = ProjectFaker.Create();

            var alias = ProjectAliasRepository.Create(project, "test owner", "test alias");
            
            Assert.NotNull(alias);
            
            Assert.AreEqual(project.id, alias.project_id);
        }
    }
}