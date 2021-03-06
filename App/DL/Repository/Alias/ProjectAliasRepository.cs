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

        public static ProjectAlias Create(Model.Project.Project project) {
            var alias = PrepareAlias(project.name);

            var creator = project.Creator();

            var owner = creator != null ? PrepareAlias(creator.login) : "_";
            #if !DEBUG
            if (creator == null) owner = PrepareAlias(project.Repository().GithubRepo().Owner.Login);
            #endif

            var newAlias = alias;
            var postfix = 0;
            while (FindByAlias(owner, newAlias) != null) {
                newAlias = $"{alias}_{postfix}";
                ++postfix;
            }
            return Find(ProjectAlias.Create(project, owner, newAlias));
        }
        
        private static string PrepareAlias(string input) => input.Replace(" ", "").ToLower();
    }
}