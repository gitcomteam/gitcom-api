using App.AL.Utils.Entity;
using App.AL.Utils.Permission;
using App.DL.Enum;
using App.DL.Repository.User;
using Micron.DL.Middleware;
using Micron.DL.Module.Http;
using Nancy;

namespace App.AL.Middleware.Cruds {
    public class HasEntityWritePermissions : IMiddleware {
        private string _entityGuidParam;

        private EntityType _entityType;

        private string _forcedGuid;
        
        public HasEntityWritePermissions(string guidParam, EntityType type, string forcedGuid = null) {
            _entityGuidParam = guidParam;
            _entityType = type;
            _forcedGuid = forcedGuid;
        }
        
        public ProcessedRequest Process(ProcessedRequest request) {
            var me = UserRepository.Find(request.UserId);

            var entityGuid = _forcedGuid ?? request.GetRequestStr(_entityGuidParam);

            if (!EntityUtils.IsEntityExists(entityGuid, _entityType)) {
                request.AddError(new HttpError(HttpStatusCode.NotFound, $"Target {_entityType} doesn't exist"));
            }

            if (!PermissionUtils.HasEntityPermission(me, entityGuid, _entityType)) {
                request.AddError(
                    new HttpError(HttpStatusCode.Forbidden, "You don't have write permissions for this " + _entityType)
                );
            }

            return request;
        }
    }
}