using App.DL.Repository.User;
using Micron.AL.Validation.String;
using Micron.DL.Middleware;
using Micron.DL.Module.Auth;
using Micron.DL.Module.Controller;
using Micron.DL.Module.Crypto;
using Micron.DL.Module.Http;
using Micron.DL.Module.Validator;
using Nancy;
using Newtonsoft.Json.Linq;

namespace App.AL.Controller.Auth {
    public sealed class JwtAuthController : BaseController {
        protected override IMiddleware[] Middleware() => new IMiddleware[] { };

        public JwtAuthController() {
            Get("/api/v1/login", _ => {
                var email = (string) Request.Query["email"];
                
                var errors = ValidationProcessor.Process(Request, new IValidatorRule[] {
                    new MinLength("email", 4),
                    new MinLength("password", 4)
                });
                if (errors.Count > 0) return HttpResponse.Errors(errors);
                
                var password = (string) Request.Query["password"];

                var user = UserRepository.FindByEmail(email);

                if (user == null) {
                    return HttpResponse.Error(HttpStatusCode.NotFound, "User not found");
                }

                if (Encryptor.Encrypt(password) != user.password) {
                    return HttpResponse.Error(
                        new HttpError(HttpStatusCode.Unauthorized, "Your email / password combination is incorrect")
                    );
                }

                return HttpResponse.Data(new JObject() {
                    ["token"] = Jwt.FromUserId(user.id)
                });
            });
        }
    }
}