using App.DL.Repository.Card;
using App.PL.Transformer.Work;
using Micron.AL.Validation.Basic;
using Micron.AL.Validation.Db;
using Micron.DL.Middleware;
using Micron.DL.Module.Controller;
using Micron.DL.Module.Http;
using Micron.DL.Module.Validator;

namespace App.AL.Controller.Work.Card {
    public class CardWorkController : BaseController {
        protected override IMiddleware[] Middleware() => new IMiddleware[] {};

        public CardWorkController() {
            Get("/api/v1/card/work/get", _ => {
                var errors = ValidationProcessor.Process(Request, new IValidatorRule[] {
                    new ShouldHaveParameters(new[] {"card_guid"}),
                    new ExistsInTable("card_guid", "cards", "guid")
                });
                if (errors.Count > 0) {
                    return HttpResponse.Errors(errors);
                }

                var card = CardRepository.FindByGuid(GetRequestStr("card_guid"));

                return HttpResponse.Item(
                    "work_items", new CardWorkTransformer().Many(card.SubmittedWork())
                );
            });
        }
    }
}