using System;
using App.AL.Utils.Entity;
using App.DL.Enum;
using Micron.DL.Module.Misc;
using NUnit.Framework;
using Tests.Testing;
using Tests.Utils.Fake.Project;

namespace Tests.App.AL.Utils.Entity {
    [TestFixture]
    public class EntityUtilsTests : BaseTestFixture {
        [Test]
        public void IsEntityExists_ProjectExists_Ok() {
            var project = ProjectFaker.Create();
            
            Assert.True(EntityUtils.IsEntityExists(project.id, EntityType.Project));
            Assert.True(EntityUtils.IsEntityExists(project.guid, EntityType.Project));
        }

        [Test]
        public void IsEntityExists_NoEntities_ShouldFail() {
            Assert.False(EntityUtils.IsEntityExists(Guid.NewGuid().ToString(), EntityType.Project));
            Assert.False(EntityUtils.IsEntityExists("randomGuid", EntityType.Project));
            Assert.False(EntityUtils.IsEntityExists(Rand.Int(), EntityType.Project));
        }
    }
}