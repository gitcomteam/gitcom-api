using System;
using Micron.DL.Module.Http;
using Micron.DL.Module.Validator;
using Nancy;

namespace App.AL.Validation.String {
    public class ShouldBeValidEmail : IValidatorRule {
        public string Parameter { get; }
        
        public ShouldBeValidEmail(string emailParameter = "email") {
            Parameter = emailParameter;
        }

        public HttpError Process(Request request) {
            try {
                new System.Net.Mail.MailAddress(request.Query[Parameter]);
            }
            catch (Exception e) {
                return new HttpError(HttpStatusCode.UnprocessableEntity, "Provided email is invalid");
            }
            return null;
        }
    }
}