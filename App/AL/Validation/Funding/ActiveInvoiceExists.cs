using System;
using App.AL.Utils.Entity;
using App.AL.Utils.Funding;
using App.DL.Enum;
using App.DL.Model.User;
using Micron.DL.Module.Http;
using Micron.DL.Module.Validator;
using Nancy;

namespace App.AL.Validation.Funding {
    public class ActiveInvoiceExists : IValidatorRule {
        public string Parameter { get; }

        private readonly User _user;
        
        private readonly string _typeParameter;
        
        public ActiveInvoiceExists(User user, string guidParameter, string entityTypeParameter) {
            Parameter = guidParameter;
            _user = user;
            _typeParameter = entityTypeParameter;
        }

        public HttpError Process(Request request) {
            var entityType = (EntityType) Enum.Parse(typeof(EntityType), (string) request.Query[_typeParameter]);
            var entityId = EntityUtils.GetEntityId((string) request.Query[Parameter], entityType);
            
            return InvoiceUtils.ActiveInvoiceExists(_user, entityId, entityType)
                ? new HttpError(HttpStatusCode.Forbidden, $"Invoice for same {entityType.ToString()} already exists")
                : null;
        }
    }
}