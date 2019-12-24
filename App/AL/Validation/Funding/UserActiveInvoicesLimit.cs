using App.DL.Model.Funding;
using App.DL.Model.User;
using Micron.DL.Module.Http;
using Micron.DL.Module.Validator;
using Nancy;

namespace App.AL.Validation.Funding {
    public class UserActiveInvoicesLimit : IValidatorRule {
        private readonly User _user;

        private readonly ushort _limit;

        public UserActiveInvoicesLimit(User user, ushort limit = 5) {
            _user = user;
            _limit = limit;
        }

        public HttpError Process(Request request)
            => Invoice.ActiveCount(_user) > _limit
                ? new HttpError(HttpStatusCode.Forbidden, "User has too many active invoices")
                : null;
    }
}