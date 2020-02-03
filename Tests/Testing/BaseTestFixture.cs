using App.DL.Module.Schedule;
using NUnit.Framework;
using Tests.Utils.DB;

namespace Tests.Testing {
    [TestFixture]
    public class BaseTestFixture {
        [SetUp]
        public void BeforeEachTest() {
            JobsPool.Get().CleanUp();
            DbCleaner.TruncateAll();
        }
    }
}