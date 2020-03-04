using App.DL.Enum;
using App.DL.Module.Schedule;
using App.DL.Repository.Funding;
using App.DL.Repository.User;
using Micron.DL.Module.Config;
using Nancy;
using Nancy.Testing;
using NUnit.Framework;
using Tests.Testing;
using Tests.Utils.Fake.User;

namespace Tests.App.AL.Schedule.User.Register {
    public class RegisterBonusTests : BaseTestFixture {
        [Test]
        public void RegisterBonus_Ok() {
            var user = UserFaker.Create();

            var browser = new Browser(new DefaultNancyBootstrapper());
            var result = browser
                .Post("/api/v1/schedule/user/register_bonus/start", with => {
                    with.HttpRequest();
                    with.Query("schedule_token", AppConfig.GetConfiguration("auth:schedule:token"));
                }).Result;

            Assert.Zero(FundingTransactionRepository.Get(user).Length);

            JobsPool.Get().WaitAll();
            
            Assert.True(FundingTransactionRepository.Get(user).Length == 1);
            var balance = UserBalanceRepository.Find(user, CurrencyType.GitComToken);
            var tokenBonus = System.Convert.ToInt32(AppConfig.GetConfiguration("user:registration:token_bonus"));
            
            Assert.AreEqual(tokenBonus, balance.balance);
        }
    }
}