using System;
using App.DL.Enum;
using App.DL.Model.Decision;

namespace App.DL.Repository.Decision {
    public static class EntityDecisionRepository {
        public static EntityDecision Find(int id) => EntityDecision.Find(id);

        public static EntityDecision FindByGuid(string guid) => EntityDecision.FindByGuid(guid);

        public static bool UpdateStatus(EntityDecision decision, DecisionStatus status) {
            if (decision.status == DecisionStatus.Canceled || decision.status == DecisionStatus.Closed) {
                return false;
            }

            decision.UpdateStatus(status);
            return true;
        }

        public static EntityDecision[] GetActive(int entityId, EntityType type, int offset = 0, int limit = 10) {
            return EntityDecision.GetActive(entityId, type, offset, limit);
        }
        
        public static EntityDecision[] Get(int entityId, EntityType type, int offset = 0, int limit = 10) {
            return EntityDecision.Get(entityId, type, offset, limit);
        }

        public static EntityDecision Create(
            Model.User.User creator, int entityId, EntityType entityType, string title, string content,
            DateTime deadline
        ) {
            var minDeadline = DateTime.Now.AddDays(1);
            var maxDeadline = DateTime.Now.AddDays(30);

            deadline = deadline < minDeadline ? minDeadline : deadline;
            deadline = deadline > maxDeadline ? maxDeadline : deadline;
            
            return Find(EntityDecision.Create(creator, entityId, entityType, title, content, deadline));
        }
    }
}