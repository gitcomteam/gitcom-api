using System.Threading.Tasks;
using App.DL.Module.Schedule;
using NUnit.Framework;
using Tests.Testing;

namespace Tests.App.DL.Module.Schedule {
    public class JobsPoolTests : BaseTestFixture {
        [Test]
        public void SimplePush_Ok() {
            var pool = JobsPool.Get();
            pool.SimplePush(Task.Run(() => {}));
            pool.SimplePush(Task.Run(() => {}));
            Assert.AreEqual(2, JobsPool.PoolSize());
            pool.WaitAll();
            pool.CleanUp();
        }

        [Test]
        public void CleanUp_Ok() {
            var pool = JobsPool.Get();
            pool.SimplePush(Task.Run(() => {}));
            pool.SimplePush(Task.Run(() => {}));
            Assert.True(JobsPool.PoolSize() == 2);
            pool.WaitAll();
            pool.CleanUp();
            Assert.Zero(JobsPool.PoolSize());
        }
    }
}