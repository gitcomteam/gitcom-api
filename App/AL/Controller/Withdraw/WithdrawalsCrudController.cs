using App.DL.Enum;
using App.DL.Repository.User;
using App.DL.Repository.Withdrawal;
using App.PL.Transformer.Withdraw;
using Micron.AL.Validation.Basic;
using Micron.AL.Validation.String;
using Micron.DL.Middleware;
using Micron.DL.Middleware.Auth;
using Micron.DL.Module.Controller;
using Micron.DL.Module.Http;
using Micron.DL.Module.Validator;
using Nancy;

namespace App.AL.Controller.Withdraw {
    public class WithdrawalsCrudController : BaseController {
        protected override IMiddleware[] Middleware() => new IMiddleware[] { new JwtMiddleware() };

        public WithdrawalsCrudController() {
            Post("/api/v1/me/withdrawals/new", _ => {
                var errors = ValidationProcessor.Process(Request, new IValidatorRule[] {
                    new ShouldHaveParameters(new[] {"amount", "currency_type"}),
                    new ShouldBeCorrectEnumValue("currency_type", typeof(CurrencyType)),
                }, true);
                if (errors.Count > 0) return HttpResponse.Errors(errors);

                var me = UserRepository.Find(CurrentRequest.UserId);

                var currencyType = (CurrencyType) GetRequestEnum("currency_type", typeof(CurrencyType));

                var amount = System.Convert.ToDecimal(GetRequestStr("amount"));

                var withdrawalRequest = WithdrawalRequestRepository.Create(me, currencyType, amount);

                return HttpResponse.Item(
                    "withdraw_request", new WithdrawalRequestTransformer().Transform(withdrawalRequest),
                    HttpStatusCode.Created
                );
            });

            Get("/api/v1/me/withdrawals/get", _ => {
                var me = UserRepository.Find(CurrentRequest.UserId);
                var withdrawalRequest = WithdrawalRequestRepository.Get(me);

                return HttpResponse.Item(
                    "withdraw_requests", new WithdrawalRequestTransformer().Many(withdrawalRequest)
                );
            });
        }
    }
}