using System;
using System.Threading.Tasks;
using App.AL.Middleware.Schedule;
using App.DL.Enum;
using App.DL.Repository.Funding;
using App.DL.Repository.User;
using Micron.DL.Middleware;
using Micron.DL.Module.Config;
using Micron.DL.Module.Controller;
using Micron.DL.Module.Http;
using Newtonsoft.Json.Linq;
using Sentry;

namespace App.AL.Schedule.User.Register {
    public class RegisterBonus : BaseController {
        protected override IMiddleware[] Middleware() => new IMiddleware[] {
            new ScheduleAuth(),
        };

        public RegisterBonus() {
            Post("/api/v1/schedule/user/register_bonus/start", _ => {
                Task.Run(() => {
                    int tokenBonus = Convert.ToInt32(
                        AppConfig.GetConfiguration("user:registration:token_bonus")
                    );
                    if (tokenBonus <= 0) return;

                    int pageIndex = 1;
                    var users = DL.Model.User.User.Paginate(pageIndex, 100);
                    while (users.Length > 0) {
                        foreach (var user in users) {
                            try {
                                if (
                                    !user.EmailConfirmed() || FundingTransactionRepository.Get(user).Length > 0
                                ) continue;
                                var balance = UserBalanceRepository.FindOrCreate(user, CurrencyType.GitComToken);
                                balance.UpdateBalance(balance.balance + tokenBonus);
                                FundingTransactionRepository.Create(
                                    user, user.id, EntityType.UserBalance, 2M, CurrencyType.GitComToken,
                                    "Registration bonus"
                                );
                            }
                            catch (Exception e) {
                                SentrySdk.CaptureException(e);
                            }
                        }

                        ++pageIndex;
                        users = DL.Model.User.User.Paginate(pageIndex, 100);
                    }
                });
                return HttpResponse.Data(new JObject());
            });
        }
    }
}