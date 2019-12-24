using System;
using Micron.DL.Module.Db;
using NUnit.Framework;
using Tests.Utils.DB;

namespace Tests.Testing {
    [SetUpFixture]
    public class SetUpFixture {
        [OneTimeSetUp]
        public void BeforeAllTests() {
            DbCleaner.TruncateAll();
        }
        
        [OneTimeTearDown]
        public void AfterAllTests() {
            try {
                DbConnection.RollbackTransaction();
            }
            catch (Exception e) {
                // ignored
            }
            DbCleaner.TruncateAll();
        }
    }
}