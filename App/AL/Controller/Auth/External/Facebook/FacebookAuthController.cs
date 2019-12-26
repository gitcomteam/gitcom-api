using System;
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

namespace App.AL.Controller.Auth.External.Facebook {
    public class FacebookAuthController : BaseController {
        private const string ApiUrl = "https://graph.facebook.com/v5.0/";

        protected override IMiddleware[] Middleware() => new IMiddleware[] { };

        public FacebookAuthController() {
            Get("/api/v1/auth/facebook/my_token/get", _ => {
                var errors = ValidationProcessor.Process(Request, new IValidatorRule[] {
                    new ShouldHaveParameters(new[] {"facebook_token"}),
                });
                if (errors.Count > 0) return HttpResponse.Errors(errors);

                var facebookToken = GetRequestStr("facebook_token");

                var response = new HttpClient().GetAsync(
                    ApiUrl + $"me?access_token={facebookToken}&fields=name,email"
                ).Result;
                if (!response.IsSuccessStatusCode) {
                    return HttpResponse.Error(HttpStatusCode.BadRequest, "Invalid facebook token");
                }

                var json = JObject.Parse(response.Content.ReadAsStringAsync().Result);

                var email = json.Value<string>("email");
                var login = email.Split("@")[0];

                var user = UserRepository.FindByEmail(email) ??
                           UserRepository.FindOrCreateByEmailAndLogin(
                               email, login, null,
                               UserRepository.FindByGuid(GetRequestStr("referral_key"))
                           );

                var accessToken =
                    ServiceAccessTokenRepository.FindOrUpdateAccessToken(user, facebookToken, ServiceType.Facebook);
                accessToken.UpdateCol("origin_user_id", json.Value<string>("id"));

                return HttpResponse.Data(new JObject() {
                    ["token"] = Jwt.FromUserId(user.id)
                });
            });
        }
    }
}