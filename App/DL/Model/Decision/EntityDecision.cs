using System;
using System.Linq;
using App.DL.Enum;
using App.DL.Repository.User;
using Dapper;

namespace App.DL.Model.Decision {
    public class EntityDecision : Micron.DL.Model.Model {
        public int id;

        public string guid;

        public int creator_id;

        public int entity_id;

        public EntityType entity_type;

        public string title;

        public string content;

        public DecisionStatus status;

        public DateTime deadline;

        public DateTime created_at;

        public DateTime updated_at;

        public static EntityDecision Find(int id)
            => Connection().Query<EntityDecision>(
                "SELECT * FROM entity_decisions WHERE id = @id LIMIT 1", new {id}
            ).FirstOrDefault();

        public static EntityDecision FindByGuid(string guid)
            => Connection().Query<EntityDecision>(
                "SELECT * FROM entity_decisions WHERE guid = @guid LIMIT 1", new {guid}
            ).FirstOrDefault();

        public static EntityDecision[] GetActive(
            int entityId, EntityType type, int offset = 0, int limit = 10
        )
            => Connection().Query<EntityDecision>(
                $@"SELECT * FROM entity_decisions
                WHERE entity_id = @entity_id AND entity_type = '{type.ToString()}' AND CURRENT_TIMESTAMP < deadline
                ORDER BY id DESC
                OFFSET @offset
                LIMIT @limit",
                new {entity_id = entityId, offset, limit}
            ).ToArray();

        public static EntityDecision[] Get(
            int entityId, EntityType type, int offset = 0, int limit = 10
        )
            => Connection().Query<EntityDecision>(
                $@"SELECT * FROM entity_decisions
                WHERE entity_id = @entity_id AND entity_type = '{type.ToString()}'
                ORDER BY id DESC
                OFFSET @offset
                LIMIT @limit",
                new {entity_id = entityId, offset, limit}
            ).ToArray();

        public EntityDecision UpdateStatus(DecisionStatus newStatus) {
            ExecuteSql(
                $@"UPDATE entity_decisions SET status = '{newStatus.ToString()}', updated_at = CURRENT_TIMESTAMP WHERE id = @id"
                , new {id}
            );
            return this;
        }

        public static int Create(
            User.User creator, int entityId, EntityType entityType, string title, string content, DateTime deadline
        ) {
            return ExecuteScalarInt(
                $@"INSERT INTO entity_decisions(
                    guid, creator_id, entity_id, entity_type, title, content, deadline, updated_at
                ) 
                VALUES (
                    @guid, @creator_id, @entity_id, '{entityType.ToString()}', @title, @content, @deadline, 
                    CURRENT_TIMESTAMP
                );
                SELECT currval('entity_decisions_id_seq');"
                , new {
                    guid = Guid.NewGuid().ToString(), creator_id = creator.id, entity_id = entityId, title, content,
                    deadline
                }
            );
        }
        
        public void Delete() => ExecuteScalarInt("DELETE FROM entity_decisions WHERE id = @id", new {id});

        public User.User Creator() {
            return UserRepository.Find(creator_id);
        }

        public EntityDecision Refresh() => Find(id);

        public EntityDecisionOption[] Options()
            => Connection().Query<EntityDecisionOption>(
                @"SELECT * FROM entity_decision_options
                    WHERE decision_id = @decision_id
                    ORDER BY id DESC
                    LIMIT 50",
                new {decision_id = id}
            ).ToArray();
    }
}