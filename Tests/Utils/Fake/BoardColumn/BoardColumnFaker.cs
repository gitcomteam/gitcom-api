using System;
using App.DL.Repository.BoardColumn;
using Micron.DL.Module.Misc;
using Tests.Utils.Fake.Board;
using Tests.Utils.Fake.User;
using UserModel = App.DL.Model.User.User;
using BoardColumnModel = App.DL.Model.BoardColumn.BoardColumn;

namespace Tests.Utils.Fake.BoardColumn {
    public static class BoardColumnFaker {
        public static BoardColumnModel Create(UserModel user = null) {
            user = user ?? UserFaker.Create();
            var board = BoardFaker.Create(user);
            return BoardColumnRepository.CreateAndGet(
                "boardColumnName_" + Rand.SmallInt(),
                board,
                Convert.ToInt16(Rand.IntRange(0, 20))
            );
        }
    }
}