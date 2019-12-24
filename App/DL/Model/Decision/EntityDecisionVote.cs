using System;
using System.Linq;
using Dapper;
using UserModel = App.DL.Model.User.User;

namespace App.DL.Model.Decision {
    public class EntityDecisionVote : Micron.DL.Model.Model {
        public int id;

        public string guid;

        public int decision_id;

        public string title;

        public int order;

        public DateTime created_at;

        public static EntityDecisionVote Find(int id)
            => Connection().Query<EntityDecisionVote>(
                "SELECT * FROM entity_decision_votes WHERE id = @id LIMIT 1",
                new {id}
            ).FirstOrDefault();

        public static EntityDecisionVote FindByGuid(string guid)
            => Connection().Query<EntityDecisionVote>(
                "SELECT * FROM entity_decision_votes WHERE guid = @guid LIMIT 1",
                new {guid}
            ).FirstOrDefault();
        
        public static EntityDecisionVote ClearOption(UserModel user, EntityDecisionOption option)
            => Connection().Query<EntityDecisionVote>(
                "SELECT * FROM entity_decision_votes WHERE user_id = @user_id AND option_id = @option_id LIMIT 1",
                new {user_id = user.id, option_id = option.id}
            ).FirstOrDefault();

        public static EntityDecisionVote Find(UserModel user, EntityDecisionOption option)
            => Connection().Query<EntityDecisionVote>(
                "SELECT * FROM entity_decision_votes WHERE user_id = @user_id AND option_id = @option_id LIMIT 1",
                new {user_id = user.id, option_id = option.id}
            ).FirstOrDefault();

        public static int Create(UserModel user, EntityDecisionOption option, int votepower)
            => ExecuteScalarInt(
                @"INSERT INTO entity_decision_votes(guid, user_id, option_id, votepower)
                                VALUES (@guid, @user_id, @option_id, @votepower);
                                SELECT currval('entity_decision_votes_id_seq');"
                , new {guid = Guid.NewGuid().ToString(), user_id = user.id, option_id = option.id, votepower}
            );
        
        public void Delete() => ExecuteScalarInt("DELETE FROM entity_decision_votes WHERE id = @id", new {id});
    }
}