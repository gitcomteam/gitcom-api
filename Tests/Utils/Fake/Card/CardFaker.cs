using App.DL.Repository.Card;
using Micron.DL.Module.Misc;
using Tests.Utils.Fake.BoardColumn;
using Tests.Utils.Fake.User;
using UserModel = App.DL.Model.User.User;
using CardModel = App.DL.Model.Card.Card;
using BoardColumnModel = App.DL.Model.BoardColumn.BoardColumn;

namespace Tests.Utils.Fake.Card {
    public static class CardFaker {
        public static CardModel Create(UserModel user = null) {
            user ??= UserFaker.Create();
            return CardRepository.CreateAndGet(
                "cardName_" + Rand.SmallInt(),
                "description" + Rand.SmallInt(),
                Rand.IntRange(0, 100),
                BoardColumnFaker.Create(user),
                user,
                user
            );
        }
    }
}