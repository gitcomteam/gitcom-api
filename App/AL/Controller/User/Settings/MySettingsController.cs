using System.Linq;
using App.AL.Config.UserSetting;
using App.DL.Model.Setting;
using App.DL.Repository.Setting;
using App.DL.Repository.User;
using App.PL.Transformer.Setting;
using Micron.AL.Validation.Basic;
using Micron.DL.Middleware;
using Micron.DL.Middleware.Auth;
using Micron.DL.Module.Controller;
using Micron.DL.Module.Http;
using Micron.DL.Module.Validator;
using Nancy;

namespace App.AL.Controller.User.Settings {
    public class MySettingsController : BaseController {
        protected override IMiddleware[] Middleware() => new IMiddleware[] {
            new JwtMiddleware(),
        };

        public MySettingsController() {
            Get("/api/v1/me/setting/get", _ => {
                var errors = ValidationProcessor.Process(Request, new IValidatorRule[] {
                    new ShouldHaveParameters(new[] {"key"}),
                }, true);
                if (errors.Count > 0) return HttpResponse.Errors(errors);

                var settingKey = GetRequestStr("key");

                if (!AllowedValues.GetAllowed().ContainsKey(settingKey)) {
                    new HttpError(HttpStatusCode.UnprocessableEntity, "This setting key is not allowed");
                }

                var me = UserRepository.Find(CurrentRequest.UserId);
                var setting = UserSetting.Find(me, settingKey);

                setting ??= UserSettingRepository.SetSetting(me, settingKey, "");

                return HttpResponse.Item("setting", new UserSettingTransformer().Transform(setting));
            });

            Get("/api/v1/me/settings/get", _ => {
                var me = UserRepository.Find(CurrentRequest.UserId);

                var settings = UserSettingRepository.Get(me);

                return HttpResponse.Item("settings", new UserSettingTransformer().Many(settings));
            });

            Patch("/api/v1/me/setting/set", _ => {
                var errors = ValidationProcessor.Process(Request, new IValidatorRule[] {
                    new ShouldHaveParameters(new[] {"key", "value"}),
                }, true);
                if (errors.Count > 0) return HttpResponse.Errors(errors);

                var settingKey = GetRequestStr("key");

                if (!AllowedValues.GetAllowed().ContainsKey(settingKey)) {
                    return HttpResponse.Error(
                        new HttpError(HttpStatusCode.UnprocessableEntity, "This setting key is not allowed")
                    );
                }

                var settingValue = GetRequestStr("value");

                if (
                    AllowedValues.GetAllowed()[settingKey] != null &&
                    !AllowedValues.GetAllowed()[settingKey].Contains(settingValue)
                ) {
                    return HttpResponse.Error(new HttpError(HttpStatusCode.Forbidden,
                        "This setting value is not allowed"));
                }

                var me = UserRepository.Find(CurrentRequest.UserId);

                var setting = UserSettingRepository.SetSetting(
                    me, settingKey, GetRequestStr("value")
                );

                return HttpResponse.Item("setting", new UserSettingTransformer().Transform(setting));
            });
        }
    }
}