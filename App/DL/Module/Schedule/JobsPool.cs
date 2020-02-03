using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.DL.Module.Schedule {
    public class JobsPool {
        private static JobsPool _instance;

        private List<Task> tasks = new List<Task>();

        public static JobsPool Get() => _instance ??= new JobsPool();

        public void Push(Task task) {
            CleanUp();
            tasks.Add(task);
        }

        public void SimplePush(Task task) => tasks.Add(task);

        public void CleanUp() {
            tasks = tasks.Where(t => !t.IsCompleted).ToList();
        }

        public static int PoolSize() => Get().tasks.Count;

        public void WaitAll() => Task.WhenAll(tasks.ToArray());
    }
}