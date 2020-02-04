using App.DL.Repository.BoardColumn;
using App.DL.Repository.Card;
using App.PL.Transformer.Card;
using Micron.AL.Validation.Basic;
using Micron.AL.Validation.Db;
using Micron.DL.Middleware;
using Micron.DL.Module.Controller;
using Micron.DL.Module.Http;
using Micron.DL.Module.Validator;
using Newtonsoft.Json.Linq;

namespace App.AL.Controller.Card {
    public sealed class CardController : BaseController {
        protected override IMiddleware[] Middleware() => new IMiddleware[] {};

        public CardController() {
            Get("/api/v1/card/get", _ => {
                var errors = ValidationProcessor.Process(Request, new IValidatorRule[] {
                    new ShouldHaveParameter("card_guid"),
                    new ExistsInTable("card_guid", "cards", "guid")
                });
                if (errors.Count > 0) return HttpResponse.Errors(errors);

                return HttpResponse.Item("card", new CardTransformer().Transform(
                    CardRepository.FindByGuid((string) Request.Query["card_guid"])
                ));
            });

            Get("/api/v1/board_column/cards/get", _ => {
                var errors = ValidationProcessor.Process(Request, new IValidatorRule[] {
                    new ShouldHaveParameter("column_guid"),
                    new ExistsInTable("column_guid", "board_columns", "guid")
                });
                if (errors.Count > 0) return HttpResponse.Errors(errors);

                var column = BoardColumnRepository.FindByGuid(GetRequestStr("column_guid"));

                var page = GetRequestInt("page");
                page = page > 0 ? page : 1;

                var pageSize = 25;

                return HttpResponse.Data(new JObject() {
                    ["cards"] = new CardTransformer().Many(column.Cards(page, pageSize))
                    ["meta"] = new JObject() {
                        ["pages_count"] = (column.CardsCount() / pageSize)+1,
                        ["current_page"] = page
                    }
                });
            });
        }
    }
}