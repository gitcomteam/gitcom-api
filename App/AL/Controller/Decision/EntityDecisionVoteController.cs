using System.Collections.Generic;
using App.AL.Utils.Decision;
using App.DL.Enum;
using App.DL.Repository.Decision;
using App.DL.Repository.User;
using Micron.AL.Validation.Basic;
using Micron.AL.Validation.Db;
using Micron.DL.Middleware;
using Micron.DL.Middleware.Auth;
using Micron.DL.Module.Controller;
using Micron.DL.Module.Http;
using Micron.DL.Module.Validator;
using Nancy;
using Newtonsoft.Json.Linq;

namespace App.AL.Controller.Decision {
    public class EntityDecisionVoteController : BaseController {
        protected override IMiddleware[] Middleware() => new IMiddleware[] {new JwtMiddleware()};

        public EntityDecisionVoteController() {
            Post("/api/v1/entity/decision/vote", _ => {
                var rules = new List<IValidatorRule>() {
                    new ShouldHaveParameters(new[] {"option_guid"}),
                    new ExistsInTable("option_guid", "entity_decision_options", "guid")
                };

                var errors = ValidationProcessor.Process(Request, rules, true);
                if (errors.Count > 0) return HttpResponse.Errors(errors);
                
                var option = EntityDecisionOptionRepository.FindByGuid(GetRequestStr("option_guid"));

                var decision = option.Decision();

                if (decision.status != DecisionStatus.Open) {
                    return HttpResponse.Error(
                        HttpStatusCode.Unauthorized, "You cannot vote for decision with status: " + decision.status
                    );
                }

                var me = UserRepository.Find(CurrentRequest.UserId);

                EntityDecisionUtils.RemoveUserVote(decision, me);
                
                EntityDecisionVoteRepository.Create(me, option, 1); // TODO: get votepower
                
                return HttpResponse.Data(new JObject() {
                    ["message"] = "Your vote was updated"
                });
            });
        }
    }
}