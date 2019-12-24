using App.DL.Model.Decision;
using UserModel = App.DL.Model.User.User;

namespace App.DL.Repository.Decision {
    public static class EntityDecisionVoteRepository {
        public static EntityDecisionVote Find(int id) {
            return EntityDecisionVote.Find(id);
        }

        public static EntityDecisionVote FindByGuid(string guid) {
            return EntityDecisionVote.FindByGuid(guid);
        }

        public static EntityDecisionVote Find(UserModel user, EntityDecisionOption option) {
            return EntityDecisionVote.Find(user, option);
        }

        public static EntityDecisionVote Create(UserModel user, EntityDecisionOption option, int votepower) {
            return Find(EntityDecisionVote.Create(user, option, votepower));
        }
    }
}