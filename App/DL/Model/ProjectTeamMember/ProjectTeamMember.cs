using System;
using System.Linq;
using Dapper;
using UserModel = App.DL.Model.User.User;
using ProjectModel = App.DL.Model.Project.Project;

// ReSharper disable InconsistentNaming

namespace App.DL.Model.ProjectTeamMember {
    public class ProjectTeamMember : Micron.DL.Model.Model {
        public int id;

        public int project_id;

        public int user_id;

        public DateTime created_at;

        public static ProjectTeamMember Find(int id)
            => Connection().Query<ProjectTeamMember>(
                "SELECT * FROM project_team_members WHERE id = @id LIMIT 1", new {id}
            ).FirstOrDefault();

        public static ProjectTeamMember Find(ProjectModel project, UserModel user)
            => Connection().Query<ProjectTeamMember>(
                "SELECT * FROM project_team_members WHERE project_id = @project_id AND user_id = @user_id  LIMIT 1",
                new {project_id = project.id, user_id = user.id}
            ).FirstOrDefault();

        public static bool IsExist(ProjectModel project, UserModel user) {
            var res = Connection().ExecuteScalar<bool>(
                "SELECT COUNT(1) FROM project_team_members WHERE project_id = @project_id AND user_id = @user_id",
                new {
                    project_id = project.id, user_id = user.id
                });
            return res;
        }

        public static ProjectTeamMember[] Get(ProjectModel project)
            => Connection().Query<ProjectTeamMember>(
                "SELECT * FROM project_team_members WHERE project_id = @project_id",
                new {project_id = project.id}
            ).ToArray();

        public static int Create(ProjectModel project, UserModel user)
            => ExecuteScalarInt(
                @"INSERT INTO project_team_members(project_id, user_id) 
                VALUES (@project_id, @user_id); SELECT currval('project_team_members_id_seq');"
                , new {project_id = project.id, user_id = user.id}
            );

        public ProjectTeamMember Save() {
            ExecuteSql(
                @"UPDATE project_team_members 
                SET project_id = @project_id, user_id = @user_id WHERE id = @id",
                new {project_id, user_id, id}
            );
            return this;
        }

        public ProjectTeamMember Refresh() => Find(id);

        public static int Count() => ExecuteScalarInt("SELECT count(*) FROM project_team_members WHERE id = @id");

        public void Delete() => ExecuteScalarInt("DELETE FROM project_team_members WHERE id = @id", new {id});

        public ProjectModel Project() => ProjectModel.Find(project_id);

        public UserModel User() => UserModel.Find(user_id);
    }
}
