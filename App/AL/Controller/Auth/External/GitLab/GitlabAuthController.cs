using App.DL.Enum;
using App.DL.External.GitLab;
using App.DL.Repository.Auth;
using App.DL.Repository.User;
using Micron.AL.Validation.Basic;
using Micron.DL.Middleware;
using Micron.DL.Module.Auth;
using Micron.DL.Module.Config;
using Micron.DL.Module.Controller;
using Micron.DL.Module.Http;
using Micron.DL.Module.Validator;
using Nancy;
using Newtonsoft.Json.Linq;

namespace App.AL.Controller.Auth.External.GitLab {
    public sealed class GitlabAuthController : BaseController {
        protected override IMiddleware[] Middleware() => new IMiddleware[] { };

        private const string Scopes = "read_user";

        public GitlabAuthController() {
            var clientId = AppConfig.GetConfiguration("auth:external:gitlab:client_id");

            var redirectUri = AppConfig.GetConfiguration("auth:external:gitlab:redirect_url");

            Get("/api/v1/auth/gitlab/login_link/get", _ => {
                var loginLink =
                    $"https://gitlab.com/oauth/authorize?client_id={clientId}&redirect_uri={redirectUri}" +
                    $"&response_type=token&scope={Scopes}";
                return HttpResponse.Data(new JObject() {
                    ["login_link"] = loginLink
                });
            });

            Get("/api/v1/auth/gitlab/get_auth_token", av => {
                var errors = ValidationProcessor.Process(Request, new IValidatorRule[] {
                    new ShouldHaveParameters(new[] {"access_token"}),
                });
                if (errors.Count > 0) return HttpResponse.Errors(errors);
                
                var accessToken = GetRequestStr("access_token");

                var client = new GitLabClient(accessToken);
                client.SetAuthorizedUser();

                if (client.User == null) {
                    return HttpResponse.Error(HttpStatusCode.Unauthorized,
                        "We're unable to get your access token, please try again");
                }
                
                var user = UserRepository.FindByEmail(client.User.Email) ?? UserRepository.FindOrCreateByEmailAndLogin(client.User.Email, client.User.Login);
                
                ServiceAccessTokenRepository.FindOrUpdateAccessToken(user, accessToken, ServiceType.GitLab);

                return HttpResponse.Data(new JObject() {
                    ["token"] = Jwt.FromUserId(user.id)
                });
            });
        }
    }
}