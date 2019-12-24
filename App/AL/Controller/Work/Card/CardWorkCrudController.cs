using App.DL.Repository.Card;
using App.DL.Repository.User;
using App.DL.Repository.Work;
using App.PL.Transformer.Work;
using Micron.AL.Validation.Basic;
using Micron.AL.Validation.Db;
using Micron.DL.Middleware;
using Micron.DL.Middleware.Auth;
using Micron.DL.Module.Controller;
using Micron.DL.Module.Http;
using Micron.DL.Module.Validator;

namespace App.AL.Controller.Work.Card {
    public class CardWorkCrudController : BaseController {
        protected override IMiddleware[] Middleware() => new IMiddleware[] {new JwtMiddleware()};

        public CardWorkCrudController() {
            Post("/api/v1/card/work/submit", _ => {
                var errors = ValidationProcessor.Process(Request, new IValidatorRule[] {
                    new ShouldHaveParameters(new[] {"card_guid", "work_type_guid", "proof"}),
                    new ExistsInTable("card_guid", "cards", "guid"),
                    new ExistsInTable("work_type_guid", "project_work_types", "guid"),
                });
                if (errors.Count > 0) {
                    return HttpResponse.Errors(errors);
                }

                var me = UserRepository.Find(CurrentRequest.UserId);

                var card = CardRepository.FindByGuid(GetRequestStr("card_guid"));

                var workType = ProjectWorkTypeRepository.FindBy(
                    "guid", GetRequestStr("work_type_guid")
                );

                var work = CardWorkRepository.CreateAndGet(
                    me, card, workType, GetRequestStr("proof")
                );

                return HttpResponse.Item("work_item", new CardWorkTransformer().Transform(work));
            });
        }
    }
}