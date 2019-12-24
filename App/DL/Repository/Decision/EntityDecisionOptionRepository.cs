using App.DL.Model.Decision;

namespace App.DL.Repository.Decision {
    public static class EntityDecisionOptionRepository {
        public static EntityDecisionOption Find(int id) {
            return EntityDecisionOption.Find(id);
        }

        public static EntityDecisionOption FindByGuid(string guid) {
            return EntityDecisionOption.FindByGuid(guid);
        }

        public static EntityDecisionOption[] Find(EntityDecision decision) {
            return EntityDecisionOption.Find(decision);
        }

        public static EntityDecisionOption Create(EntityDecision decision, string title, int order = 0) {
            return Find(EntityDecisionOption.Create(decision, title, order));
        }
    }
}