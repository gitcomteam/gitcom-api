using System.Net.Http;
using App.DL.Enum;
using App.DL.Repository.Auth;
using App.DL.Repository.User;
using Micron.AL.Validation.Basic;
using Micron.DL.Middleware;
using Micron.DL.Module.Auth;
using Micron.DL.Module.Controller;
using Micron.DL.Module.Http;
using Micron.DL.Module.Validator;
using Nancy;
using Newtonsoft.Json.Linq;

namespace App.AL.Controller.Auth.External.Google {
    public class GoogleAuthController : BaseController {
        private const string ApiUrl = "https://www.googleapis.com/oauth2/v1/";

        protected override IMiddleware[] Middleware() => new IMiddleware[] { };

        public GoogleAuthController() {
            Get("/api/v1/auth/google/my_token/get", _ => {
                var errors = ValidationProcessor.Process(Request, new IValidatorRule[] {
                    new ShouldHaveParameters(new[] {"google_token"}),
                });
                if (errors.Count > 0) return HttpResponse.Errors(errors);

                var googleToken = GetRequestStr("google_token");

                var response = new HttpClient().GetAsync(
                    ApiUrl + $"userinfo?access_token={googleToken}"
                ).Result;
                if (!response.IsSuccessStatusCode) {
                    return HttpResponse.Error(HttpStatusCode.BadRequest, "Invalid google token");
                }

                var json = JObject.Parse(response.Content.ReadAsStringAsync().Result);

                var email = json.Value<string>("email");
                var login = email.Split("@")[0];

                var user = UserRepository.FindByEmail(email) ?? UserRepository.FindOrCreateByEmailAndLogin(
                               email, login, null,
                               UserRepository.FindByGuid(GetRequestStr("referral_key"))
                           );

                var accessToken =
                    ServiceAccessTokenRepository.FindOrUpdateAccessToken(user, googleToken, ServiceType.Google);
                accessToken.UpdateCol("origin_user_id", json.Value<string>("id"));

                return HttpResponse.Data(new JObject() {
                    ["token"] = Jwt.FromUserId(user.id)
                });
            });
        }
    }
}