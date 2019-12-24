using System;
using System.Linq;
using App.DL.Repository.Project;
using Dapper;

namespace App.DL.Model.Work {
    public class ProjectWorkType : Micron.DL.Model.Model {
        public int id;

        public string guid;

        public int project_id;

        public string title;

        public int budget_percent;

        public DateTime created_at;

        public DateTime updated_at;

        public static ProjectWorkType Find(int id)
            => Connection().Query<ProjectWorkType>(
                "SELECT * FROM project_work_types WHERE id = @id LIMIT 1", new {id}
            ).FirstOrDefault();

        public static ProjectWorkType FindBy(string col, string val)
            => Connection().Query<ProjectWorkType>(
                $"SELECT * FROM project_work_types WHERE {col} = @val LIMIT 1", new {val}
            ).FirstOrDefault();

        public static ProjectWorkType FindBy(string col, int val)
            => Connection().Query<ProjectWorkType>(
                $"SELECT * FROM project_work_types WHERE {col} = @val LIMIT 1", new {val}
            ).FirstOrDefault();

        public static int Create(Project.Project project, string title, int budgetPercent)
            => ExecuteScalarInt(
                @"INSERT INTO project_work_types(guid, project_id, title, budget_percent)
                        VALUES (@guid, @project_id, @title, @budget_percent);
                        SELECT currval('project_work_types_id_seq');"
                , new {guid = Guid.NewGuid().ToString(), project_id = project.id, title, budget_percent = budgetPercent}
            );

        public void Update(string col, string val) {
            ExecuteSql(
                $"UPDATE project_work_types SET {col} = @val, updated_at = CURRENT_TIMESTAMP WHERE id = @id",
                new {val, id}
            );
        }

        public Project.Project Project() => ProjectRepository.Find(project_id);
    }
}