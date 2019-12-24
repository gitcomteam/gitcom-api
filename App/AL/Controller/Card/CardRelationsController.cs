using App.DL.Repository.Card;
using App.PL.Transformer.Project;
using Micron.AL.Validation.Basic;
using Micron.AL.Validation.Db;
using Micron.DL.Middleware;
using Micron.DL.Module.Controller;
using Micron.DL.Module.Http;
using Micron.DL.Module.Validator;

namespace App.AL.Controller.Card {
    public class CardRelationsController : BaseController {
        protected override IMiddleware[] Middleware() => new IMiddleware[] {};

        public CardRelationsController() {
            Get("/api/v1/card/project/get", _ => {
                var errors = ValidationProcessor.Process(Request, new IValidatorRule[] {
                    new ShouldHaveParameter("card_guid"),
                    new ExistsInTable("card_guid", "cards", "guid")
                });
                if (errors.Count > 0) return HttpResponse.Errors(errors);
                
                var card = CardRepository.FindByGuid(GetRequestStr("card_guid"));

                return HttpResponse.Item("project", new ProjectTransformer().Transform(card.Project()));
            });
        }
    }
}