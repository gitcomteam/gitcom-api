using App.DL.Repository.Product;
using App.DL.Repository.User;
using App.PL.Transformer.Product;
using Micron.DL.Middleware;
using Micron.DL.Middleware.Auth;
using Micron.DL.Module.Controller;
using Micron.DL.Module.Http;

namespace App.AL.Controller.Product {
    public class UserOwnedProductCrudController : BaseController {
        protected override IMiddleware[] Middleware() => new IMiddleware[] {
            new JwtMiddleware()
        };

        public UserOwnedProductCrudController() {
            Get("/api/v1/me/products/owned", _ => {
                var me = UserRepository.Find(CurrentRequest.UserId);

                var products = UserOwnedProductRepository.Get(me);

                return HttpResponse.Item("products", new UserOwnedProductTransformer().Many(products));
            });
        }
    }
}