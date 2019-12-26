using System;
using App.AL.Validation.Permission;
using App.DL.Enum;
using App.DL.Repository.Board;
using App.DL.Repository.BoardColumn;
using App.PL.Transformer.BoardColumn;
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

namespace App.AL.Controller.BoardColumn {
    public sealed class BoardColumnCrudController : BaseController {
        protected override IMiddleware[] Middleware() => new IMiddleware[] {
            new JwtMiddleware()
        };

        public BoardColumnCrudController() {
            Post("/api/v1/board_column/create", _ => {
                var me = UserRepository.Find(CurrentRequest.UserId);
                var board = BoardRepository.FindByGuid(GetRequestStr("board_guid"));
                
                var errors = ValidationProcessor.Process(Request, new IValidatorRule[] {
                    new ShouldHaveParameters(new[] {"name", "board_guid", "board_order"}),
                    new ExistsInTable("board_guid", "boards", "guid"),
                    new HasPermission(me, board.Project().id, EntityType.Project)
                }, true);
                if (errors.Count > 0) return HttpResponse.Errors(errors);

                var boardOrder = (short) Request.Query["board_order"];

                if (BoardColumnRepository.Find(board, boardOrder) != null)
                {
                    return HttpResponse.Error(HttpStatusCode.Conflict,
                        "Board's column with this board's order already exists ");
                }

                var boardColumn = BoardColumnRepository.CreateAndGet(
                    (string) Request.Query["name"],
                    board,
                    boardOrder
                );

                return HttpResponse.Item(
                    "board_column", new BoardColumnTransformer().Transform(boardColumn), HttpStatusCode.Created
                );
            });

            Patch("/api/v1/board_column/edit", _ => { 
                var me = UserRepository.Find(CurrentRequest.UserId);
                var boardColumn = BoardColumnRepository.FindByGuid(GetRequestStr("board_column_guid"));
                var board = BoardRepository.Find(boardColumn.board_id);
                
                var errors = ValidationProcessor.Process(Request, new IValidatorRule[] {
                    new ShouldHaveParameters(new[] {"board_column_guid"}),
                    new ExistsInTable("board_column_guid", "board_columns", "guid"),
                    new HasPermission(me, boardColumn.Board().id, EntityType.Board)
                }, true);
                if (errors.Count > 0) return HttpResponse.Errors(errors);
                
                if (BoardColumnRepository.Find(board,  Convert.ToInt16(GetRequestStr("board_order"))) != null)
                {
                    return HttpResponse.Error(HttpStatusCode.Conflict,
                        "Board's column with this board's order already exists ");
                }

                boardColumn = BoardColumnRepository.UpdateAndRefresh(boardColumn, new JObject() {
                    ["name"] = GetRequestStr("name"),
                    ["board_order"] = GetRequestStr("board_order")
                });

                return HttpResponse.Item("board_column", new BoardColumnTransformer().Transform(boardColumn));
            });
            
            Delete("/api/v1/board_column/delete", _ => {
                var me = UserRepository.Find(CurrentRequest.UserId);
                var boardColumn = BoardColumnRepository.FindByGuid(GetRequestStr("board_column_guid"));
                
                var errors = ValidationProcessor.Process(Request, new IValidatorRule[] {
                    new ShouldHaveParameters(new[] { "board_column_guid" }),
                    new ExistsInTable("board_column_guid", "board_columns", "guid"),
                    new HasPermission(me, boardColumn.Board().id, EntityType.Board)
                }, true);
                if (errors.Count > 0) return HttpResponse.Errors(errors);

                boardColumn.Delete();

                return HttpResponse.Item("board", new BoardColumnTransformer().Transform(boardColumn));
            });
        }
    }
}