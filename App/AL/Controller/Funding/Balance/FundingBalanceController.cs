using App.AL.Utils.Entity;
using App.AL.Validation.Entity;
using App.DL.Enum;
using App.DL.Repository.Funding;
using App.DL.Repository.User;
using App.PL.Transformer.Funding;
using Micron.AL.Validation.Basic;
using Micron.AL.Validation.String;
using Micron.DL.Middleware;
using Micron.DL.Module.Controller;
using Micron.DL.Module.Http;
using Micron.DL.Module.Validator;

namespace App.AL.Controller.Funding.Balance {
    public class FundingBalanceController : BaseController {
        protected override IMiddleware[] Middleware() => new IMiddleware[] {};

        // TODO: add auth for getting user balance?
        public FundingBalanceController() {
            Get("/api/v1/entity/funding/balances/get", _ => {
                var errors = ValidationProcessor.Process(Request, new IValidatorRule[] {
                    new ShouldHaveParameters(new[] {"entity_guid", "entity_type"}),
                    new ShouldBeCorrectEnumValue("entity_type", typeof(EntityType)),
                    new EntityShouldExist(),
                }, true);
                if (errors.Count > 0) {
                    return HttpResponse.Errors(errors);
                }

                var entityType = (EntityType) GetRequestEnum("entity_type", typeof(EntityType));
                
                var entityId = EntityUtils.GetEntityId(GetRequestStr("entity_guid"), entityType);

                var balances = FundingBalanceRepository.Get(entityId, entityType);
                
                return HttpResponse.Item("balances", new FundingBalanceTransformer().Many(balances));
            });
        }
    }
}