using System;
using System.Linq;
using App.DL.Repository.Project;
using App.DL.Repository.User;
using Dapper;

namespace App.DL.Model.UserLibrary {
    public class UserLibraryItem : Micron.DL.Model.Model {
        public int id;

        public string guid;

        public int user_id;

        public int project_id;

        public DateTime created_at;

        public DateTime updated_at;

        public static UserLibraryItem Find(int id)
            => Connection().Query<UserLibraryItem>(
                "SELECT * FROM user_projects_library WHERE id = @id LIMIT 1", new {id}
            ).FirstOrDefault();

        public static UserLibraryItem FindBy(string col, string val)
            => Connection().Query<UserLibraryItem>(
                $"SELECT * FROM user_projects_library WHERE {col} = @val LIMIT 1", new {val}
            ).FirstOrDefault();

        public static UserLibraryItem Find(User.User user, Project.Project project)
            => Connection().Query<UserLibraryItem>(
                "SELECT * FROM user_projects_library WHERE user_id = @user_id AND project_id = @project_id LIMIT 1",
                new {
                    user_id = user.id, project_id = project.id
                }
            ).FirstOrDefault();

        public static UserLibraryItem[] Get(User.User user)
            => Connection().Query<UserLibraryItem>(
                "SELECT * FROM user_projects_library WHERE user_id = @user_id LIMIT 100",
                new {user_id = user.id}
            ).ToArray();

        public static int Create(User.User user, Project.Project project) {
            return ExecuteScalarInt(
                @"INSERT INTO user_projects_library(guid, user_id, project_id, updated_at)
                        VALUES (@guid, @user_id, @project_id, CURRENT_TIMESTAMP);
                        SELECT currval('user_projects_library_id_seq');"
                , new {
                    guid = Guid.NewGuid().ToString(), user_id = user.id, project_id = project.id
                }
            );
        }
        
        public void Delete() => ExecuteScalarInt("DELETE FROM user_projects_library WHERE id = @id", new {id});

        public User.User User() => UserRepository.Find(user_id);

        public Project.Project Project() => ProjectRepository.Find(project_id);
    }
}