using System;
using System.Linq;
using Dapper;
using UserModel = App.DL.Model.User.User;
using ProjectModel = App.DL.Model.Project.Project;

// ReSharper disable InconsistentNaming

namespace App.DL.Model.Board
{
    public class Board : Micron.DL.Model.Model {
        public int id;

        public string guid;

        public string name;

        public string description;

        public int project_id;

        public int creator_id;

        public DateTime created_at;

        public DateTime updated_at;

        public static Board Find(int id)
            => Connection().Query<Board>(
                "SELECT * FROM boards WHERE id = @id LIMIT 1", new {id}
            ).FirstOrDefault();

        public static Board FindByGuid(string guid)
            => Connection().Query<Board>(
                "SELECT * FROM boards WHERE guid = @guid LIMIT 1", new {guid}
            ).FirstOrDefault();

        public static Board Find(ProjectModel project, UserModel creator) {
            return Connection().Query<Board>(
                "SELECT * FROM boards WHERE project_id = @project_id AND creator_id = @creator_id  LIMIT 1",
                new {
                    project_id = project.id, creator_id = creator.id
                }
            ).FirstOrDefault();
        }

        public static int Create(string name, string description, ProjectModel project, UserModel creator) {

            return ExecuteScalarInt(
                @"INSERT INTO boards(guid, name, description, project_id, creator_id) 
                VALUES (@guid, @name, @description, @project_id, @creator_id); SELECT currval('boards_id_seq');"
                , new {guid = Guid.NewGuid().ToString(), name, description, project_id = project.id, creator_id = creator?.id}
            );
        }

        public Board Save() {
            ExecuteSql(
                @"UPDATE boards 
                SET name = @name, description = @description, project_id = @project_id WHERE id = @id",
                new {name, description, project_id, creator_id, id}
            );
            return this;
        }

        public Board Refresh() => Find(id);

        public static int Count() => ExecuteScalarInt("SELECT count(*) FROM boards WHERE id = @id");

        public void Delete() => ExecuteScalarInt("DELETE FROM boards WHERE id = @id", new {id});

        public ProjectModel Project() => ProjectModel.Find(project_id);

        public UserModel User() => UserModel.Find(creator_id);

        public BoardColumn.BoardColumn[] Columns(int limit = 10)
            => Connection().Query<BoardColumn.BoardColumn>(
                @"SELECT * FROM board_columns WHERE board_id = @board_id LIMIT @limit",
                new {board_id = id, limit}
            ).ToArray();
    }
}
