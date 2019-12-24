using System;
using System.Linq;
using App.DL.Repository.Product;
using App.DL.Repository.User;
using Dapper;

namespace App.DL.Model.Product {
    public class UserOwnedProduct : Micron.DL.Model.Model {
        public int id;

        public string guid;

        public int user_id;

        public int product_id;

        public DateTime expiry_at;

        public DateTime created_at;

        public DateTime updated_at;

        public static UserOwnedProduct Find(int id)
            => Connection().Query<UserOwnedProduct>(
                "SELECT * FROM user_owned_products WHERE id = @id LIMIT 1", new {id}
            ).FirstOrDefault();

        public static UserOwnedProduct FindBy(string col, string val)
            => Connection().Query<UserOwnedProduct>(
                $"SELECT * FROM user_owned_products  WHERE {col} = @val LIMIT 1", new {val}
            ).FirstOrDefault();

        public static UserOwnedProduct[] GetBy(string col, string val)
            => Connection().Query<UserOwnedProduct>(
                $"SELECT * FROM user_owned_products WHERE {col} = @val LIMIT 50", new {val}
            ).ToArray();

        public static UserOwnedProduct[] GetBy(string col, int val)
            => Connection().Query<UserOwnedProduct>(
                $"SELECT * FROM user_owned_products WHERE {col} = @val LIMIT 50", new {val}
            ).ToArray();

        public static int Create(User.User user, ProjectProduct product) {
            var expiryAt = DateTime.UtcNow.AddHours(product.duration_hours);
            return ExecuteScalarInt(
                @"INSERT INTO user_owned_products(guid, user_id, product_id, expiry_at, updated_at)
                                VALUES (@guid, @user_id, @product_id, @expiry_at, CURRENT_TIMESTAMP); 
                                SELECT currval('user_owned_products_id_seq');"
                , new {
                    guid = Guid.NewGuid().ToString(), user_id = user.id, product_id = product.id, expiry_at = expiryAt
                }
            );
        }

        public UserOwnedProduct Refresh() => Find(id);

        public User.User User() => UserRepository.Find(user_id);

        public ProjectProduct Product() => ProjectProductRepository.Find(product_id);
    }
}