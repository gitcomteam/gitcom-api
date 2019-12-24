using App.AL.Utils.Permission;
using App.DL.Enum;
using App.DL.Model.User;
using Micron.DL.Module.Http;
using Micron.DL.Module.Validator;
using Nancy;

namespace App.AL.Validation.Permission {
    public class HasPermission : IValidatorRule {

        private User _user;

        private int _entityId;

        private EntityType _entityType;
        
        public HasPermission(User user, int entityId, EntityType entityType) {
            _user = user;
            _entityId = entityId;
            _entityType = entityType;
        }

        public HttpError Process(Request request) {
            if (!PermissionUtils.HasEntityPermission(_user, _entityId, _entityType)) {
                return new HttpError(HttpStatusCode.Forbidden, "You don't have edit permissions for this entity");
            }
            return null;
        }
    }
}