using App.DL.Model.User;
using UserModel = App.DL.Model.User.User;

namespace App.DL.Repository.User {
    public static class UsersVotepowerRepository {
        public static UsersVotepower Find(int id) {
            return UsersVotepower.Find(id);
        }

        public static UsersVotepower FindByGuid(string guid) {
            return UsersVotepower.FindByGuid(guid);
        }

        public static UsersVotepower Find(UserModel user, Model.Project.Project project) {
            return UsersVotepower.Find(user, project);
        }
        
        public static UsersVotepower FindOrCreate(UserModel user, Model.Project.Project project) {
            var votepower = UsersVotepower.Find(user, project);
            if (votepower == null) {
                votepower = Create(user, project);
            }
            return votepower;
        }

        public static UsersVotepower Create(UserModel user, Model.Project.Project project) {
            return Find(UsersVotepower.Create(user, project));
        }
    }
}
