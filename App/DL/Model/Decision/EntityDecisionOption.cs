using System;
using System.Linq;
using App.DL.Repository.Decision;
using Dapper;
using UserModel = App.DL.Model.User.User;

namespace App.DL.Model.Decision {
    public class EntityDecisionOption : Micron.DL.Model.Model {
        public int id;

        public string guid;

        public int decision_id;

        public string title;

        public int order;

        public DateTime created_at;

        public static EntityDecisionOption Find(int id)
            => Connection().Query<EntityDecisionOption>(
                "SELECT * FROM entity_decision_options WHERE id = @id LIMIT 1",
                new {id}
            ).FirstOrDefault();

        public static EntityDecisionOption FindByGuid(string guid)
            => Connection().Query<EntityDecisionOption>(
                "SELECT * FROM entity_decision_options WHERE guid = @guid LIMIT 1",
                new {guid}
            ).FirstOrDefault();

        public static EntityDecisionOption[] Find(EntityDecision decision)
            => Connection().Query<EntityDecisionOption>(
                "SELECT * FROM entity_decision_options WHERE decision_id = @decision_id LIMIT 10",
                new {decision_id = decision.id}
            ).ToArray();

        public static int Create(EntityDecision decision, string title, int order = 0)
            => ExecuteScalarInt(
                @"INSERT INTO entity_decision_options(guid, decision_id, title, ""order"")
                                VALUES (@guid, @decision_id, @title, @order);
                                SELECT currval('entity_decision_options_id_seq');"
                , new {guid = Guid.NewGuid().ToString(), decision_id = decision.id, title, order}
            );

        public EntityDecision Decision() => EntityDecisionRepository.Find(decision_id);
        
        public EntityDecisionVote UserVote(UserModel user) {
            return Connection().Query<EntityDecisionVote>(
                "SELECT * FROM entity_decision_votes WHERE user_id = @user_id AND option_id = @option_id LIMIT 1",
                new {user_id = user.id, option_id = id}
            ).FirstOrDefault();
        }
    }
}