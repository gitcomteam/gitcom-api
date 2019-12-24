using App.DL.Model.Work;
using App.DL.Repository.Work;
using Tests.Utils.Fake.Card;
using Tests.Utils.Fake.User;
using UserModel = App.DL.Model.User.User;
using CardModel = App.DL.Model.Card.Card;

namespace Tests.Utils.Fake.Work {
    public static class CardWorkFaker {
        public static CardWork Create(ProjectWorkType workType, CardModel card = null, UserModel user = null) {
            user ??= UserFaker.Create();
            card ??= CardFaker.Create();
            return CardWorkRepository.CreateAndGet(user, card, workType, "some proof like link or something");
        }
    }
}