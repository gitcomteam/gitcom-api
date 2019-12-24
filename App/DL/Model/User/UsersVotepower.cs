using System;
using System.Linq;
using Dapper;
using UserModel = App.DL.Model.User.User;

namespace App.DL.Model.User {
    public class UsersVotepower : Micron.DL.Model.Model {
        public int id;

        public string guid;

        public int user_id;

        public int project_id;

        public int votepower;

        public DateTime created_at;
        
        public static UsersVotepower Find(int id)
            => Connection().Query<UsersVotepower>(
                "SELECT * FROM users_votepower WHERE id = @id LIMIT 1",
                new {id}
            ).FirstOrDefault();

        public static UsersVotepower FindByGuid(string guid)
            => Connection().Query<UsersVotepower>(
                "SELECT * FROM users_votepower WHERE guid = @guid LIMIT 1",
                new {guid}
            ).FirstOrDefault();

        public static UsersVotepower Find(UserModel user, Project.Project project)
            => Connection().Query<UsersVotepower>(
                "SELECT * FROM users_votepower WHERE user_id = @user_id AND project_id = @project_id LIMIT 1",
                new {user_id = user.id, project_id = project.id}
            ).FirstOrDefault();

        public static int Create(UserModel user, Project.Project project)
            => ExecuteScalarInt(
                @"INSERT INTO users_votepower(guid, user_id, project_id)
                        VALUES (@guid, @user_id, @project_id);
                        SELECT currval('users_votepower_id_seq');"
                , new {guid = Guid.NewGuid().ToString(), user_id = user.id, project_id = project.id}
            );
    }
}