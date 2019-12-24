using System;
using App.DL.Module.Cache;
using Micron.DL.Module.Db;
using NUnit.Framework;

namespace Tests.Testing {
    [TestFixture]
    public class BaseTestFixture {
        [SetUp]
        public void BeforeEachTest() {
            try {
                DbConnection.RollbackTransaction();
                ModelCache.Reset();
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
            DbConnection.BeginTransaction();
        }
    }
}