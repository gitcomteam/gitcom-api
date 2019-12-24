using App.DL.Model.Work;

namespace App.DL.Repository.Work {
    public static class ProjectWorkTypeRepository {
        public static ProjectWorkType Find(int id) {
            return ProjectWorkType.Find(id);
        }

        public static ProjectWorkType FindBy(string col, string val) {
            return ProjectWorkType.FindBy(col, val);
        }

        public static ProjectWorkType FindBy(string col, int val) {
            return ProjectWorkType.FindBy(col, val);
        }
        
        public static ProjectWorkType CreateAndGet(Model.Project.Project project, string title, int budgetPercent) {
            return Find(ProjectWorkType.Create(project, title, budgetPercent));
        }
    }
}