using App.DL.Repository.Board;
using Micron.DL.Module.Misc;
using Tests.Utils.Fake.Project;
using Tests.Utils.Fake.User;
using UserModel = App.DL.Model.User.User;
using BoardModel = App.DL.Model.Board.Board;

namespace Tests.Utils.Fake.Board {
    public static class BoardFaker {
        public static BoardModel Create(UserModel user = null) {
            user = user ?? UserFaker.Create();
            return BoardRepository.CreateAndGet(
                "boardName_" + Rand.SmallInt(),
                "description" + Rand.SmallInt(),
                ProjectFaker.Create(user),
                user
            );
        }
    }
}