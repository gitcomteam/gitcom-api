using App.DL.Model.Decision;
using App.DL.Model.User;

namespace App.AL.Utils.Decision {
    public static class EntityDecisionUtils {
        public static void RemoveUserVote(EntityDecision decision, User user) {
            foreach (var option in decision.Options()) {
                var vote = option.UserVote(user);
                vote?.Delete();
            }
        }
    }
}