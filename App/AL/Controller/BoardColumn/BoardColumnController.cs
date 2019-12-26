using App.DL.Repository.Board;
using App.DL.Repository.BoardColumn;
using App.PL.Transformer.BoardColumn;
using Micron.AL.Validation.Basic;
using Micron.AL.Validation.Db;
using Micron.DL.Middleware;
using Micron.DL.Module.Controller;
using Micron.DL.Module.Http;
using Micron.DL.Module.Validator;

namespace App.AL.Controller.BoardColumn {
    public sealed class BoardColumnController : BaseController {
        protected override IMiddleware[] Middleware() => new IMiddleware[] {};

        public BoardColumnController() {
            Get("/api/v1/board_column/get", _ => {
                var errors = ValidationProcessor.Process(Request, new IValidatorRule[] {
                    new ShouldHaveParameter("board_column_guid"),
                    new ExistsInTable("board_column_guid", "board_columns", "guid")
                });
                if (errors.Count > 0) return HttpResponse.Errors(errors);

                return HttpResponse.Item("board_column", new BoardColumnTransformer().Transform(
                    BoardColumnRepository.FindByGuid((string) Request.Query["board_column_guid"])
                ));
            });
            
            Get("/api/v1/board/columns/get", _ => {
                var errors = ValidationProcessor.Process(Request, new IValidatorRule[] {
                    new ShouldHaveParameter("board_guid"),
                    new ExistsInTable("board_guid", "boards", "guid")
                });
                if (errors.Count > 0) return HttpResponse.Errors(errors);

                var board = BoardRepository.FindByGuid(GetRequestStr("board_guid"));
                
                return HttpResponse.Item("board_columns", new BoardColumnTransformer().Many(board.Columns()));
            });
        }
    }
}