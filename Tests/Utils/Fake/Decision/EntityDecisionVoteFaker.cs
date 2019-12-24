using App.DL.Model.Decision;
using App.DL.Repository.Decision;
using Tests.Utils.Fake.User;
using UserModel = App.DL.Model.User.User;

namespace Tests.Utils.Fake.Decision {
    public class EntityDecisionVoteFaker {
        public static EntityDecisionVote Create(EntityDecisionOption option = null, UserModel user = null) {
            option ??= EntityDecisionOptionFaker.Create();
            user ??= UserFaker.Create();
            return EntityDecisionVoteRepository.Create(user, option, 1); // TODO: get votepower
        }
    }
}