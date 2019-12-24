using App.AL.Utils.Entity;
using App.AL.Utils.Permission;
using App.AL.Validation.Entity;
using App.DL.Enum;
using App.DL.Repository.User;
using Micron.AL.Validation.Basic;
using Micron.AL.Validation.String;
using Micron.DL.Middleware;
using Micron.DL.Middleware.Auth;
using Micron.DL.Module.Controller;
using Micron.DL.Module.Http;
using Micron.DL.Module.Validator;
using Newtonsoft.Json.Linq;

namespace App.AL.Controller.Permission {
    public class EntityPermissionsController : BaseController {
        protected override IMiddleware[] Middleware() => new IMiddleware[] { new JwtMiddleware() };

        public EntityPermissionsController() {
            Get("/api/v1/entity/permissions/get", _ => {
                var errors = ValidationProcessor.Process(Request, new IValidatorRule[] {
                    new ShouldHaveParameters(new[] {"entity_guid", "entity_type"}),
                    new ShouldBeCorrectEnumValue("entity_type", typeof(EntityType)),
                    new EntityShouldExist(),
                }, true);
                if (errors.Count > 0) return HttpResponse.Errors(errors);

                var me = UserRepository.Find(CurrentRequest.UserId);

                var entityType = (EntityType) GetRequestEnum("entity_type", typeof(EntityType));
                
                var entityId = EntityUtils.GetEntityId(GetRequestStr("entity_guid"), entityType);

                if (!PermissionUtils.HasEntityPermission(me, entityId, entityType)) {
                    return HttpResponse.Item("permissions", new JArray() {});
                }

                return HttpResponse.Item("permissions", new JArray() {"read", "write"});
            });
        }
    }
}