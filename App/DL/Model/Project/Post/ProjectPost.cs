using System;
using System.Linq;
using App.DL.Repository.Project;
using Dapper;

namespace App.DL.Model.Project.Post {
    public class ProjectPost : Micron.DL.Model.Model {
        public int id;

        public string guid;

        public int project_id;

        public string title;

        public string content;

        public DateTime created_at;

        public DateTime updated_at;

        public static ProjectPost Find(int id)
            => Connection().Query<ProjectPost>(
                "SELECT * FROM project_posts WHERE id = @id LIMIT 1", new {id}
            ).FirstOrDefault();

        public static ProjectPost FindBy(string col, string val)
            => Connection().Query<ProjectPost>(
                $"SELECT * FROM project_posts WHERE {col} = @val LIMIT 1", new {val}
            ).FirstOrDefault();

        public static ProjectPost[] Get(Project project)
            => Connection().Query<ProjectPost>(
                "SELECT * FROM project_posts WHERE project_id = @project_id LIMIT 10", new {project_id = project.id}
            ).ToArray();

        public static ProjectPost[] Latest() => Connection()
            .Query<ProjectPost>("SELECT * FROM project_posts ORDER BY id DESC LIMIT 10")
            .ToArray();

        public static ProjectPost Create(Project project, string title, string content) {
            return Find(ExecuteScalarInt(
                @"INSERT INTO project_posts(guid, project_id, title, content, updated_at)
                        VALUES (@guid, @project_id, @title, @content, CURRENT_TIMESTAMP);
                        SELECT currval('project_posts_id_seq');"
                , new {
                    guid = Guid.NewGuid().ToString(), project_id = project.id, title, content
                }
            ));
        }

        public ProjectPost UpdateCol(string col, string val) {
            ExecuteSql(
                $"UPDATE project_posts SET {col} = @val, updated_at = CURRENT_TIMESTAMP WHERE id = @id",
                new {val, id}
            );
            return this;
        }

        public void Delete() => ExecuteScalarInt("DELETE FROM project_posts WHERE id = @id", new {id});

        public Project Project() => ProjectRepository.Find(project_id);
    }
}