using System.Collections.Generic;
using App.AL.Utils.Entity;
using App.AL.Validation.Entity;
using App.DL.Enum;
using App.DL.Repository.Decision;
using App.PL.Transformer.Decision;
using Micron.AL.Validation.Basic;
using Micron.AL.Validation.String;
using Micron.DL.Middleware;
using Micron.DL.Module.Controller;
using Micron.DL.Module.Http;
using Micron.DL.Module.Validator;

namespace App.AL.Controller.Decision {
    public class EntityDecisionController : BaseController {
        protected override IMiddleware[] Middleware() => new IMiddleware[] {};

        public EntityDecisionController() {
            Get("/api/v1/entity/decisions/active/get", _ => {
                var rules = new List<IValidatorRule>() {
                    new ShouldHaveParameters(new[] {"entity_guid", "entity_type"}),
                    new EntityShouldExist("entity_guid", "entity_type"),
                    new ShouldBeCorrectEnumValue("entity_type", typeof(EntityType)),
                };

                var errors = ValidationProcessor.Process(Request, rules, true);
                if (errors.Count > 0) return HttpResponse.Errors(errors);

                var entityType = (EntityType) GetRequestEnum("entity_type", typeof(EntityType));

                var entityId = EntityUtils.GetEntityId(GetRequestStr("entity_guid"), entityType);

                var decisions = EntityDecisionRepository.GetActive(entityId, entityType);

                return HttpResponse.Item("decisions", new DecisionTransformer().Many(decisions));
            });

        }
    }
}