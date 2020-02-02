using System.Collections.Generic;
using App.AL.Validation.String;
using App.DL.Module.Email;
using App.DL.Repository.User;
using App.DL.Repository.User.Registration;
using Micron.AL.Validation.Basic;
using Micron.AL.Validation.String;
using Micron.DL.Middleware;
using Micron.DL.Module.Auth;
using Micron.DL.Module.Controller;
using Micron.DL.Module.Crypto;
using Micron.DL.Module.Http;
using Micron.DL.Module.Misc;
using Micron.DL.Module.Validator;
using Nancy;
using Newtonsoft.Json.Linq;

namespace App.AL.Controller.Auth {
    public sealed class JwtAuthController : BaseController {
        protected override IMiddleware[] Middleware() => new IMiddleware[] { };

        public JwtAuthController() {
            Get("/api/v1/login", _ => {
                var errors = ValidationProcessor.Process(Request, new IValidatorRule[] {
                    new MinLength("email", 4),
                    new MinLength("password", 4)
                }, true);
                if (errors.Count > 0) return HttpResponse.Errors(errors);

                var email = GetRequestStr("email").Replace(" ", "");

                var password = GetRequestStr("password");

                var user = UserRepository.FindByEmail(email);

                if (user == null) return HttpResponse.Error(HttpStatusCode.NotFound, "User not found");

                if (Encryptor.Encrypt(password) != user.password)
                    return HttpResponse.Error(
                        new HttpError(HttpStatusCode.Unauthorized, "Your email / password combination is incorrect")
                    );
                
                if (!user.EmailConfirmed())
                    return HttpResponse.Error(HttpStatusCode.Forbidden, "You need to confirm your email");

                return HttpResponse.Data(new JObject() {
                    ["token"] = Jwt.FromUserId(user.id)
                });
            });

            Post("/api/v1/register", _ => {
                var errors = ValidationProcessor.Process(Request, new IValidatorRule[] {
                    new ShouldHaveParameters(new[] {"login", "email", "password"}),
                    new MinLength("login", 4),
                    new MinLength("email", 4),
                    new MinLength("password", 6),
                    new ShouldBeValidEmail(),
                }, true);
                if (errors.Count > 0) return HttpResponse.Errors(errors);

                var login = GetRequestStr("login").Replace(" ", "");
                var user = UserRepository.FindByLogin(login);
                if (user != null)
                    return HttpResponse.Error(
                        HttpStatusCode.Forbidden,
                        "User with this login already exist"
                    );

                var email = GetRequestStr("email").Replace(" ", "");
                user = UserRepository.FindByEmail(email);
                if (user != null)
                    return HttpResponse.Error(
                        HttpStatusCode.Forbidden,
                        "User with this login already exist"
                    );

                var registeredUser = UserRepository.FindOrCreateByEmailAndLogin(
                    email, login, GetRequestStr("password"),
                    UserRepository.FindByGuid(GetRequestStr("referral_key"))
                );

                var registerQueueItem = RegistrationQueueItemRepository.Create(registeredUser);

                MailGunSender.QueueTemplate(
                    "confirm-your-email", registeredUser.email, "GitCom - you almost there!",
                    new[] {
                        new KeyValuePair<string, string>("confirmation_key", registerQueueItem.confirmation_key),
                    }
                );

                return HttpResponse.Data(new JObject() {
                    ["response"] = "Please confirm your email"
                });
            });

            Post("/api/v1/lazy_register", _ => {
                var errors = ValidationProcessor.Process(Request, new IValidatorRule[] {
                    new ShouldHaveParameters(new[] {"email"}),
                    new ShouldBeValidEmail(),
                }, true);
                if (errors.Count > 0) return HttpResponse.Errors(errors);

                var email = GetRequestStr("email").Replace(" ", "").ToLower();

                var existingUser = UserRepository.FindByEmail(email);

                if (existingUser != null) {
                    return HttpResponse.Error(
                        HttpStatusCode.Forbidden,
                        "User with this email already exist, you need to log in"
                    );
                }

                var login = email.Split("@")[0];

                var registeredUser = UserRepository.FindOrCreateByEmailAndLogin(
                    email, login, Rand.RandomString()
                );

                var registerQueueItem = RegistrationQueueItemRepository.Create(registeredUser);
                
                MailGunSender.QueueTemplate(
                    "confirm-your-email", registeredUser.email, "GitCom - you almost there!",
                    new[] {
                        new KeyValuePair<string, string>("confirmation_key", registerQueueItem.confirmation_key),
                    }
                );
                

                return HttpResponse.Data(new JObject() {
                    ["token"] = Jwt.FromUserId(registeredUser.id)
                });
            });

            Post("/api/v1/register/confirm_email", _ => {
                var errors = ValidationProcessor.Process(Request, new IValidatorRule[] {
                    new ShouldHaveParameters(new[] {"confirmation_key"}),
                }, true);
                if (errors.Count > 0) return HttpResponse.Errors(errors);
                
                var queuedItem = RegistrationQueueItemRepository.FindBy(
                    "confirmation_key", GetRequestStr("confirmation_key")
                );

                if (queuedItem == null)
                    return HttpResponse.Error(HttpStatusCode.NotFound, "Confirmation key is invalid");

                if (queuedItem.email_confirmed)
                    return HttpResponse.Error(HttpStatusCode.Forbidden, "Email is already confirmed");
                
                var me = UserRepository.Find(queuedItem.user_id);

                queuedItem.EmailConfirmed();
                
                MailGunSender.QueueTemplate(
                    "registration-complete", me.email, "GitCom - welcome!",
                    new[] {
                        new KeyValuePair<string, string>("login", me.login),
                    }
                );

                return HttpResponse.Data(new JObject() {
                    ["token"] = Jwt.FromUserId(me.id)
                });
            });
        }
    }
}