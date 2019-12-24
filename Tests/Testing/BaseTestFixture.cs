using NUnit.Framework;
using Tests.Utils.DB;

// TODO: use transactions to speed up performance

namespace Tests.Testing {
    [TestFixture]
    public class BaseTestFixture {
        [SetUp]
        public void BeforeEachTest() {
            DbCleaner.TruncateAll();
        }
    }
}