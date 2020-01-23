using System.Collections.Generic;
using System.Net.Http;
using App.DL.Enum;
using App.DL.Repository.Auth;
using App.DL.Repository.User;
using Micron.DL.Middleware;
using Micron.DL.Module.Auth;
using Micron.DL.Module.Config;
using Micron.DL.Module.Controller;
using Micron.DL.Module.Http;
using Micron.DL.Module.Misc;
using Nancy;
using Newtonsoft.Json.Linq;
using Octokit;

namespace App.AL.Controller.Auth.external.github {
    public sealed class GithubAuthController : BaseController {
        protected override IMiddleware[] Middleware() => new IMiddleware[] {};
        
        private const string Scopes = "user:email,read:user";

        public GithubAuthController() {
            var clientId = AppConfig.GetConfiguration("auth:external:github:client_id");
            var clientSecret = AppConfig.GetConfiguration("auth:external:github:client_secret");

            Get("/api/v1/auth/github/login_link/get", _ => {
                var loginLink = $"https://github.com/login/oauth/authorize?scope={Scopes}&client_id={clientId}";
                return HttpResponse.Data(new JObject() {
                    ["login_link"] = loginLink
                });
            });

            Get("/api/v1/auth/github/get_auth_token", _ => {
                var responseBody = "";
                var code = GetRequestStr("code");

                using (var client = new HttpClient()) {
                    client.DefaultRequestHeaders.Add("Accept", "application/json");

                    var response = client.PostAsync(
                        "https://github.com/login/oauth/access_token",
                        new FormUrlEncodedContent(new[] {
                            new KeyValuePair<string, string>("client_id", clientId),
                            new KeyValuePair<string, string>("client_secret", clientSecret),
                            new KeyValuePair<string, string>("code", code),
                        })
                    ).Result;

                    if (response.IsSuccessStatusCode) {
                        responseBody = response.Content.ReadAsStringAsync().Result;
                    }
                }

                var json = JObject.Parse(responseBody);
                var accessToken = json.Value<string>("access_token");

                if (accessToken == null) {
                    return HttpResponse.Error(HttpStatusCode.Unauthorized,
                        "We're unable to get your access token, please try again");
                }

                var githubClient = new GitHubClient(new ProductHeaderValue("SupportHub"));

                githubClient.Credentials = new Credentials(accessToken);

                var githubUser = githubClient.User.Current().Result;

                var userEmail = githubUser.Email ?? $"{Rand.RandomString()}-needs-update@gitcom.org";

                var user = UserRepository.FindByEmail(userEmail) ??
                           UserRepository.FindOrCreateByEmailAndLogin(
                               userEmail, githubUser.Login, null,
                               UserRepository.FindByGuid(GetRequestStr("referral_key"))
                           );

                var tokenModel =
                    ServiceAccessTokenRepository.FindOrUpdateAccessToken(user, accessToken, ServiceType.GitHub);
                tokenModel.UpdateCol("origin_user_id", githubUser.Id.ToString());

                return HttpResponse.Data(new JObject() {
                    ["token"] = Jwt.FromUserId(user.id)
                });
            });
        }
    }
}