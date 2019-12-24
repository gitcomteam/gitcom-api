using App.AL.Validation.Permission;
using App.DL.Enum;
using App.DL.Repository.BoardColumn;
using App.DL.Repository.User;
using App.DL.Repository.Card;
using App.PL.Transformer.Card;
using Micron.AL.Validation.Basic;
using Micron.AL.Validation.Db;
using Micron.DL.Middleware;
using Micron.DL.Middleware.Auth;
using Micron.DL.Module.Controller;
using Micron.DL.Module.Http;
using Micron.DL.Module.Validator;
using Nancy;
using Newtonsoft.Json.Linq;

namespace App.AL.Controller.Card {
    public sealed class CardCrudController : BaseController {
        protected override IMiddleware[] Middleware() => new IMiddleware[] {
            new JwtMiddleware()
        };

        public CardCrudController() {
            Post("/api/v1/card/create", _ => {
                var me = UserRepository.Find(CurrentRequest.UserId);
                var column = BoardColumnRepository.FindByGuid(GetRequestStr("column_guid"));
                var board = column.Board();
                
                var errors = ValidationProcessor.Process(Request, new IValidatorRule[] {
                    new ShouldHaveParameters(new[] {"name", "column_guid"}),
                    new ExistsInTable("column_guid", "board_columns", "guid"),
                    new HasPermission(me, board.id, EntityType.Board)
                }, true);
                if (errors.Count > 0) return HttpResponse.Errors(errors);
                var description = (string) Request.Query["description"] ?? "";
                int columnOrder = Request.Query["column_order"] ?? 0; 

                var card = CardRepository.CreateAndGet(
                    (string) Request.Query["name"],
                    description,
                    columnOrder,
                    column,
                    me
                );

                return HttpResponse.Item(
                    "card", new CardTransformer().Transform(card), HttpStatusCode.Created
                );
            });

            Patch("/api/v1/card/edit", _ => { 
                var me = UserRepository.Find(CurrentRequest.UserId);
                var card = CardRepository.FindByGuid(GetRequestStr("card_guid"));
                
                var errors = ValidationProcessor.Process(Request, new IValidatorRule[] {
                    new ShouldHaveParameters(new[] {"card_guid"}),
                    new ExistsInTable("card_guid", "cards", "guid"),
                    new HasPermission(me, card.id, EntityType.Card)
                }, true);
                if (errors.Count > 0) return HttpResponse.Errors(errors);

                card = CardRepository.UpdateAndRefresh(card, new JObject() {
                    ["name"] = GetRequestStr("name"),
                    ["description"] = GetRequestStr("description"),
                    ["column_order"] = GetRequestStr("column_order"),
                    ["column_guid"] = GetRequestStr("column_guid"),
                });

                return HttpResponse.Item("card", new CardTransformer().Transform(card));
            });
            
            Delete("/api/v1/card/delete", _ => {
                var me = UserRepository.Find(CurrentRequest.UserId);
                var card = CardRepository.FindByGuid(GetRequestStr("card_guid"));
                
                var errors = ValidationProcessor.Process(Request, new IValidatorRule[] {
                    new ShouldHaveParameters(new[] { "card_guid" }),
                    new ExistsInTable("card_guid", "cards", "guid"),
                    new HasPermission(me, card.id, EntityType.Card)
                }, true);
                if (errors.Count > 0) return HttpResponse.Errors(errors);

                card.Delete();

                return HttpResponse.Item("card", new CardTransformer().Transform(card));
            });
        }
    }
}