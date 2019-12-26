using System;
using App.AL.Validation.Permission;
using App.DL.Enum;
using App.DL.Repository.Project;
using App.DL.Repository.User;
using App.DL.Repository.Board;
using App.PL.Transformer.Board;
using Micron.AL.Validation.Basic;
using Micron.AL.Validation.Db;
using Micron.DL.Middleware;
using Micron.DL.Middleware.Auth;
using Micron.DL.Module.Controller;
using Micron.DL.Module.Http;
using Micron.DL.Module.Validator;
using Nancy;
using Newtonsoft.Json.Linq;

namespace App.AL.Controller.Board {
    public sealed class BoardCrudController : BaseController {
        protected override IMiddleware[] Middleware() => new IMiddleware[] {
            new JwtMiddleware()
        };

        public BoardCrudController() {
            Post("/api/v1/board/create", _ => {
                var me = UserRepository.Find(CurrentRequest.UserId);
                var project = ProjectRepository.FindByGuid(GetRequestStr("project_guid"));
                
                var errors = ValidationProcessor.Process(Request, new IValidatorRule[] {
                    new ShouldHaveParameters(new[] {"name", "description", "project_guid"}),
                    new ExistsInTable("project_guid", "projects", "guid"),
                    new HasPermission(me, project.id, EntityType.Project)
                }, true);
                if (errors.Count > 0) return HttpResponse.Errors(errors);

                var board = BoardRepository.CreateAndGet(
                    (string) Request.Query["name"],
                    (string) Request.Query["description"],
                    project,
                    me
                );

                return HttpResponse.Item(
                    "board", new BoardTransformer().Transform(board), HttpStatusCode.Created
                );
            });

            Patch("/api/v1/board/edit", _ => { 
                var me = UserRepository.Find(CurrentRequest.UserId);
                var board = BoardRepository.FindByGuid(GetRequestStr("board_guid"));
                
                var errors = ValidationProcessor.Process(Request, new IValidatorRule[] {
                    new ShouldHaveParameters(new[] {"board_guid"}),
                    new ExistsInTable("board_guid", "boards", "guid"),
                    new HasPermission(me, board.id, EntityType.Board)
                }, true);
                if (errors.Count > 0) return HttpResponse.Errors(errors);

                board = BoardRepository.UpdateAndRefresh(board, new JObject() {
                    ["name"] = GetRequestStr("name"),
                    ["description"] = GetRequestStr("description"),
                    ["updated_at"] = DateTime.Now, 
                });

                return HttpResponse.Item("board", new BoardTransformer().Transform(board));
            });
            
            Delete("/api/v1/board/delete", _ => {
                var me = UserRepository.Find(CurrentRequest.UserId);
                var board = BoardRepository.FindByGuid(GetRequestStr("board_guid"));
                
                var errors = ValidationProcessor.Process(Request, new IValidatorRule[] {
                    new ShouldHaveParameters(new[] { "board_guid" }),
                    new ExistsInTable("board_guid", "boards", "guid"),
                    new HasPermission(me, board.id, EntityType.Board)
                }, true);
                if (errors.Count > 0) return HttpResponse.Errors(errors);

                board.Delete();

                return HttpResponse.Item("board", new BoardTransformer().Transform(board));
            });
        }
    }
}