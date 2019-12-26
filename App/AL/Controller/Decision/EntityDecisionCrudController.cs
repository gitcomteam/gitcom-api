using System;
using System.Collections.Generic;
using System.Linq;
using App.AL.Utils.Entity;
using App.AL.Utils.Permission;
using App.AL.Validation.Entity;
using App.DL.Enum;
using App.DL.Repository.Decision;
using App.DL.Repository.User;
using App.PL.Transformer.Decision;
using Micron.AL.Validation.Basic;
using Micron.AL.Validation.Db;
using Micron.AL.Validation.String;
using Micron.DL.Middleware;
using Micron.DL.Middleware.Auth;
using Micron.DL.Module.Controller;
using Micron.DL.Module.Http;
using Micron.DL.Module.Validator;
using Nancy;
using Newtonsoft.Json.Linq;

namespace App.AL.Controller.Decision {
    public class EntityDecisionCrudController : BaseController {
        protected override IMiddleware[] Middleware() => new IMiddleware[] {
            new JwtMiddleware()
        };
        
        public EntityDecisionCrudController() {
            // Options should be a valid json array ex. "['option 1', 'option2']"
            Post("/api/v1/entity/decision/create", _ => {
                var rules = new List<IValidatorRule>() {
                    new ShouldHaveParameters(new[] {
                        "entity_guid", "entity_type", "title", "content", "deadline", "options"
                    }),
                    new MinLength("title", 3),
                    new MinLength("content", 10),
                    new ShouldBeCorrectEnumValue("entity_type", typeof(EntityType)),
                    new EntityShouldExist()
                };

                var errors = ValidationProcessor.Process(Request, rules, true);
                if (errors.Count > 0) return HttpResponse.Errors(errors);

                JArray options;

                try {
                    options = JArray.Parse(GetRequestStr("options"));
                }
                catch (Exception e) {
                    return HttpResponse.Error(
                        HttpStatusCode.UnprocessableEntity,
                        "For options please provide a valid JSON array"
                    );
                }

                var entityType = (EntityType) GetRequestEnum("entity_type", typeof(EntityType));
                
                var entityId = EntityUtils.GetEntityId(GetRequestStr("entity_guid"), entityType);
                
                if (entityType != EntityType.Project || entityType != EntityType.Board) {
                    
                }
                
                var deadline = DateTime.Parse(GetRequestStr("deadline"));

                var minDeadline = DateTime.Now.AddDays(1);

                if (deadline < minDeadline) {
                    return HttpResponse.Error(
                        HttpStatusCode.UnprocessableEntity,
                        "Deadline cannot be earlier than 1 day : " + minDeadline
                    );
                }

                var me = UserRepository.Find(CurrentRequest.UserId);

                if (PermissionUtils.HasEntityPermission(me, entityId, entityType)) {
                    return HttpResponse.Error(
                        HttpStatusCode.Unauthorized,
                        "You don't have decision edit access"
                    );
                }

                var decision = EntityDecisionRepository.Create(
                    me,
                    entityId,
                    entityType,
                    GetRequestStr("title"),
                    GetRequestStr("content"),
                    deadline
                );

                int optionOrder = 1;
                foreach (var option in options) {
                    EntityDecisionOptionRepository.Create(decision, option.Value<string>(), optionOrder);
                    optionOrder++;
                    if (optionOrder > 10) {
                        break;
                    }
                }

                return HttpResponse.Item(
                    "decision", new DecisionTransformer().Transform(decision), HttpStatusCode.Created
                );
            });
            Patch("/api/v1/entity/decision/edit", _ => {
                var rules = new List<IValidatorRule>() {
                    new ShouldHaveParameters(new[] {"decision_guid"}),
                    new ExistsInTable("decision_guid", "entity_decisions", "guid")
                };

                var errors = ValidationProcessor.Process(Request, rules, true);
                if (errors.Count > 0) return HttpResponse.Errors(errors);

                var me = UserRepository.Find(CurrentRequest.UserId);

                var decision = EntityDecisionRepository.FindByGuid(GetRequestStr("decision_guid"));

                if (me.id != decision.creator_id) {
                    return HttpResponse.Error(HttpStatusCode.Unauthorized, "Only creator can update this decision");
                }

                if (GetRequestStr("new_status") != "") {
                    var newStatus = (DecisionStatus) GetRequestEnum("new_status", typeof(DecisionStatus));

                    switch (newStatus) {
                        case DecisionStatus.Canceled:
                            decision.UpdateStatus(DecisionStatus.Canceled);
                            break;
                        case DecisionStatus.Open:
                        case DecisionStatus.Closed:
                            return HttpResponse.Error(HttpStatusCode.Unauthorized, "You cannot set this status");
                    }
                }

                decision = decision.Refresh();

                return HttpResponse.Item("decision", new DecisionTransformer().Transform(decision));
            });
            Delete("/api/v1/entity/decision/delete", _ => {
                var rules = new List<IValidatorRule>() {
                    new ShouldHaveParameters(new[] {"decision_guid"}),
                    new ExistsInTable("decision_guid", "entity_decisions", "guid")
                };

                var errors = ValidationProcessor.Process(Request, rules, true);
                if (errors.Count > 0) return HttpResponse.Errors(errors);

                var me = UserRepository.Find(CurrentRequest.UserId);

                var decision = EntityDecisionRepository.FindByGuid(GetRequestStr("decision_guid"));

                if (me.id != decision.creator_id) {
                    return HttpResponse.Error(HttpStatusCode.Unauthorized, "Only creator can update this decision");
                }

                decision.Delete();

                return HttpResponse.Item("decision", new DecisionTransformer().Transform(decision));
            });
        }
    }
}