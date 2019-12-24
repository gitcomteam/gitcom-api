using App.DL.Repository.User;
using App.PL.Transformer.User;
using Micron.DL.Middleware;
using Micron.DL.Middleware.Auth;
using Micron.DL.Module.Controller;
using Micron.DL.Module.Http;

namespace App.AL.Controller.Funding.Balance {
    public class MyBalanceController : BaseController {
        protected override IMiddleware[] Middleware() => new IMiddleware[] {
            new JwtMiddleware(),
        };

        public MyBalanceController() {
            Get("/api/v1/me/balances/get", _ => {
                var me = UserRepository.Find(CurrentRequest.UserId);
                
                var balances = UserBalanceRepository.GetPositive(me);

                return HttpResponse.Item("balances", new UserBalanceTransformer().Many(balances));
            });
        }
    }
}