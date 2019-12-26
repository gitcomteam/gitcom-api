using System;
using System.Linq;
using App.DL.Model.Work;
using App.DL.Repository.Repo;
using App.DL.Repository.User;
using RepoModel = App.DL.Model.Repo.Repo;
using UserModel = App.DL.Model.User.User;
using Dapper;
using App.DL.Model.Alias;
using App.DL.Repository.Alias;
using App.DL.Repository.Project;
using App.DL.Repository.Product;
using App.DL.Model.Product;

// ReSharper disable InconsistentNaming

namespace App.DL.Model.Project {
    public class Project : Micron.DL.Model.Model {
        public int id;

        public string guid;

        public int creator_id;

        public string name;

        public string description;

        public int repository_id;

        public DateTime created_at;

        public DateTime updated_at;

        public static Project Find(int id)
            => Connection().Query<Project>(
                "SELECT * FROM projects WHERE id = @id LIMIT 1", new {id}
            ).FirstOrDefault();

        public static Project FindByGuid(string guid)
            => Connection().Query<Project>(
                "SELECT * FROM projects WHERE guid = @guid LIMIT 1", new {guid}
            ).FirstOrDefault();

        public static Project FindBy(string col, string val)
            => Connection().Query<Project>(
                $"SELECT * FROM projects WHERE {col} = @val LIMIT 1", new {val}
            ).FirstOrDefault();

        public static Project FindBy(string col, int val)
            => Connection().Query<Project>(
                $"SELECT * FROM projects WHERE {col} = @val LIMIT 1", new {val}
            ).FirstOrDefault();

        public static Project FindRandom()
            => Connection().Query<Project>(
                "SELECT * FROM projects WHERE id = @id ORDER BY random() LIMIT 1"
            ).FirstOrDefault();

        public static Project[] GetRandom()
            => Connection().Query<Project>(
                "SELECT * FROM projects ORDER BY random() LIMIT 10"
            ).ToArray();

        public static Project[] GetNewest()
            => Connection().Query<Project>(
                "SELECT * FROM projects ORDER BY id DESC LIMIT 10"
            ).ToArray();

        public static int Create(string name, UserModel creator = null, RepoModel repo = null) {
            return ExecuteScalarInt(
                @"INSERT INTO public.projects(guid, name, creator_id, repository_id)
                VALUES (@guid, @name, @creator_id, @repository_id); SELECT currval('projects_id_seq');"
                , new {
                    guid = Guid.NewGuid().ToString(), name, creator_id = creator?.id, repository_id = repo?.id ?? 0
                }
            );
        }

        public Project Save() {
            ExecuteSql(
                @"UPDATE projects 
                SET name = @name, guid = @guid, repository_id = @repository_id, updated_at = CURRENT_TIMESTAMP WHERE id = @id",
                new {name, guid, repository_id, id}
            );
            return this;
        }

        public Project UpdateCol(string col, string val) {
            ExecuteSql(
                $"UPDATE projects SET {col} = @val, updated_at = CURRENT_TIMESTAMP WHERE id = @id",
                new {val, id}
            );
            return this;
        }

        public Project Refresh() => ProjectRepository.Find(id);

        public Repo.Repo Repository() => RepoRepository.Find(repository_id);

        public User.User Creator() => UserRepository.Find(creator_id);

        public ProjectAlias Alias() => ProjectAliasRepository.Find(this);

        public Board.Board[] Boards(int limit = 10)
            => Connection().Query<Board.Board>(
                @"SELECT * FROM boards WHERE project_id = @project_id LIMIT @limit",
                new {project_id = id, limit}
            ).ToArray();

        public ProjectWorkType[] WorkTypes(int limit = 10)
            => Connection().Query<ProjectWorkType>(
                @"SELECT * FROM project_work_types WHERE project_id = @project_id LIMIT @limit",
                new {project_id = id, limit}
            ).ToArray();

        public ProjectProduct[] Products() => ProjectProductRepository.Get(this);

        public bool InLibrary(UserModel user)
            => Connection().ExecuteScalar<int>(
                   @"SELECT COUNT(*) FROM user_projects_library 
                    WHERE project_id = @project_id AND user_id = @user_id LIMIT 1"
                   , new {project_id = id, user_id = user.id}
               ) > 0;

        public int StarsCount() =>
            ExecuteScalarInt("SELECT COUNT(*) FROM user_projects_library WHERE project_id = @id", new {id});

        public void Delete() => ExecuteScalarInt("DELETE FROM projects WHERE id = @id", new {id});
    }
}