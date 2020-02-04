using App.DL.Repository.Project;
using App.PL.Transformer.Project;
using NUnit.Framework;
using Tests.Testing;
using Tests.Utils.Fake.Repo;

namespace Tests.App.PL.Transformer.Project {
    public class ProjectTransformerTests : BaseTestFixture {
        [Test]
        public void Transform_ProjectWithoutCreator_WithoutExceptions() {
            var project = ProjectRepository.FindOrCreate("some proj", null, RepoFaker.Create());
            new ProjectTransformer().Transform(project);
        }
    }
}