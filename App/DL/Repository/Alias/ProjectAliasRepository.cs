using App.DL.Model.Alias;

namespace App.DL.Repository.Alias {
    public static class ProjectAliasRepository {
        public static ProjectAlias Find(int id) {
            return ProjectAlias.Find(id);
        }
        
        public static ProjectAlias FindBy(string col, string val) {
            return ProjectAlias.FindBy(col, val);
        }

        public static ProjectAlias FindBy(string col, int val) {
            return ProjectAlias.FindBy(col, val);
        }

        public static ProjectAlias FindByAlias(string owner, string alias) {
            return ProjectAlias.FindByAlias(owner, alias);
        }

        public static ProjectAlias Find(Model.Project.Project project) {
            return ProjectAlias.FindBy("project_id", project.id);
        }

        public static ProjectAlias Create(Model.Project.Project project, string owner, string alias) {
            return Find(ProjectAlias.Create(project, owner, alias));
        }
    }
}