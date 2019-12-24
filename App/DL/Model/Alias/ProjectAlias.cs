using System;
using System.Linq;
using App.DL.Repository.Project;
using Dapper;

namespace App.DL.Model.Alias {
    public class ProjectAlias : Micron.DL.Model.Model {
        public int id;

        public string guid;

        public string owner;

        public string alias;

        public int project_id;

        public DateTime created_at;

        public DateTime updated_at;

        public static ProjectAlias Find(int id)
            => Connection().Query<ProjectAlias>(
                "SELECT * FROM project_aliases WHERE id = @id LIMIT 1", new {id}
            ).FirstOrDefault();

        public static ProjectAlias FindBy(string col, string val)
            => Connection().Query<ProjectAlias>(
                $"SELECT * FROM project_aliases WHERE {col} = @val LIMIT 1", new {val}
            ).FirstOrDefault();

        public static ProjectAlias FindBy(string col, int val)
            => Connection().Query<ProjectAlias>(
                $"SELECT * FROM project_aliases WHERE {col} = @val LIMIT 1", new {val}
            ).FirstOrDefault();

        public static ProjectAlias FindByAlias(string owner, string alias)
            => Connection().Query<ProjectAlias>(
                "SELECT * FROM project_aliases WHERE owner = @owner AND alias = @alias LIMIT 1", new {
                    owner, alias
                }
            ).FirstOrDefault();

        public static int Create(Project.Project project, string owner, string alias)
            => ExecuteScalarInt(
                @"INSERT INTO project_aliases(guid, project_id, owner, alias)
                               VALUES (@guid, @project_id, @owner, @alias);
                               SELECT currval('project_aliases_id_seq');"
                , new {
                    guid = Guid.NewGuid().ToString(), project_id = project.id, owner, alias
                }
            );

        public void UpdateCol(string col, string val) {
            ExecuteSql(
                $"UPDATE project_aliases SET {col} = @val, updated_at = CURRENT_TIMESTAMP WHERE id = @id",
                new {val, id}
            );
        }

        public Project.Project Project() => ProjectRepository.Find(project_id);
    }
}