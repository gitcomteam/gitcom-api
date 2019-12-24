using App.DL.Model.Project;
using App.DL.Repository.Work;

namespace App.AL.Utils.Work {
    public static class ProjectWorkUtils {
        public static (string name, int percent)[] GetDefaultWorkTypes() => new (string name, int percent)[] {
            ("Development", 60),
            ("Testing", 10),
            ("Planning", 8),
            ("Code review", 6),
            ("Documentation", 6),
            ("Configuration", 6),
            ("Translation", 4),
            ("Other", 0)
        };

        public static void SetUp(Project project) {
            var defaultWorkTypes = GetDefaultWorkTypes();

            foreach (var workType in defaultWorkTypes) {
                ProjectWorkTypeRepository.CreateAndGet(project, workType.name, workType.percent);
            }
        }
    }
}