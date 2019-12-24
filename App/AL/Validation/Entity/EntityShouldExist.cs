using System;
using App.AL.Utils.Entity;
using App.DL.Enum;
using Micron.DL.Module.Http;
using Micron.DL.Module.Validator;
using Nancy;

namespace App.AL.Validation.Entity {
    public class EntityShouldExist : IValidatorRule {
        public string Parameter { get; }

        private readonly string _typeParameter;

        public EntityShouldExist(string guidParameter = "entity_guid", string entityTypeParameter = "entity_type") {
            Parameter = guidParameter;
            _typeParameter = entityTypeParameter;
        }

        public HttpError Process(Request request) {
            var entityType = (EntityType) Enum.Parse(typeof(EntityType), (string) request.Query[_typeParameter]);
            
            return EntityUtils.IsEntityExists((string) request.Query[Parameter], entityType)
                ? null
                : new HttpError(HttpStatusCode.NotFound, $"{entityType.ToString()} does not exist", Parameter);
        }
    }
}