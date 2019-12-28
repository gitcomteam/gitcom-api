using App.DL.Model.Product;

namespace App.DL.Repository.Product {
    public static class UserOwnedProductRepository {
        public static UserOwnedProduct Find(int id) => UserOwnedProduct.Find(id);

        public static UserOwnedProduct FindByGuid(string guid) => UserOwnedProduct.FindBy("guid", guid);

        public static UserOwnedProduct FindBy(string col, string val) => UserOwnedProduct.FindBy(col, val);

        public static UserOwnedProduct[] GetBy(string col, string val) => UserOwnedProduct.GetBy(col, val);
        
        public static UserOwnedProduct[] Get(Model.User.User user) => UserOwnedProduct.GetBy("user_id", user.id);

        public static UserOwnedProduct Create(Model.User.User user, ProjectProduct product) {
            return Find(UserOwnedProduct.Create(user, product));
        }

        public static int UsersCount(ProjectProduct product) => UserOwnedProduct.UsersCount(product);
    }
}