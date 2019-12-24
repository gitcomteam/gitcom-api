using App.AL.Validation.String;
using App.DL.Model.MailingList;
using Micron.AL.Validation.Basic;
using Micron.DL.Middleware;
using Micron.DL.Module.Controller;
using Micron.DL.Module.Http;
using Micron.DL.Module.Validator;
using Newtonsoft.Json.Linq;

namespace App.AL.Controller.MailingList {
    public class MailingListController : BaseController {
        protected override IMiddleware[] Middleware() => new IMiddleware[] {};

        public MailingListController() {
            Post("/api/v1/mailing_list/subscribe", _ => {
                var errors = ValidationProcessor.Process(Request, new IValidatorRule[] {
                    new ShouldHaveParameters(new[] {"email"}),
                    new ShouldBeValidEmail(), 
                }, true);
                if (errors.Count > 0) return HttpResponse.Errors(errors);

                var email = GetRequestStr("email");
                
                var existingSubscriber = MailingListSubscriber.FindBy("email", email);

                if (existingSubscriber == null) MailingListSubscriber.Create(email);
                
                return HttpResponse.Data(new JObject() {
                    ["status"] = "subscribed"
                });
            });

            Delete("/api/v1/mailing_list/unsubscribe", _ => {
                var errors = ValidationProcessor.Process(Request, new IValidatorRule[] {
                    new ShouldHaveParameters(new[] {"unsubscribe_key"})
                });
                if (errors.Count > 0) return HttpResponse.Errors(errors);

                var subscriber = MailingListSubscriber.FindBy(
                    "unsubscribe_key", GetRequestStr("unsubscribe_key")
                );
                subscriber?.Delete();

                return HttpResponse.Data(new JObject() {
                    ["status"] = "unsubscribed"
                });
            });
        }
    }
}