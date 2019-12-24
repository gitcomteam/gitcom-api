using App.DL.Model.Work;

namespace App.DL.Repository.Work {
    public static class CardWorkRepository {
        public static CardWork Find(int id) {
            return CardWork.Find(id);
        }
        
        public static CardWork FindByGuid(string guid) {
            return CardWork.FindBy("guid", guid);
        }

        public static CardWork FindBy(string col, string val) {
            return CardWork.FindBy(col, val);
        }
        
        public static CardWork FindBy(string col, int val) {
            return CardWork.FindBy(col, val);
        }

        public static CardWork CreateAndGet(
            Model.User.User user, Model.Card.Card card, ProjectWorkType workType, string proof
        ) {
            return Find(CardWork.Create(user, card, workType, proof));
        }
    }
}