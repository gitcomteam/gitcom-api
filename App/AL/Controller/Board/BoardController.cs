using App.DL.Repository.Board;
using App.DL.Repository.Project;
using App.PL.Transformer.Board;
using Micron.AL.Validation.Basic;
using Micron.AL.Validation.Db;
using Micron.DL.Middleware;
using Micron.DL.Module.Controller;
using Micron.DL.Module.Http;
using Micron.DL.Module.Validator;

namespace App.AL.Controller.Board {
    public sealed class BoardController : BaseController {
        protected override IMiddleware[] Middleware() => new IMiddleware[] {};

        public BoardController() {
            Get("/api/v1/board/get", _ => {
                var errors = ValidationProcessor.Process(Request, new IValidatorRule[] {
                    new ShouldHaveParameter("board_guid"),
                    new ExistsInTable("board_guid", "boards", "guid")
                });
                if (errors.Count > 0) return HttpResponse.Errors(errors);

                return HttpResponse.Item("board", new BoardTransformer().Transform(
                    BoardRepository.FindByGuid((string) Request.Query["board_guid"])
                ));
            });

            Get("/api/v1/project/boards/get", _ => {
                var errors = ValidationProcessor.Process(Request, new IValidatorRule[] {
                    new ShouldHaveParameter("project_guid"),
                    new ExistsInTable("project_guid", "projects", "guid")
                });
                if (errors.Count > 0) return HttpResponse.Errors(errors);

                var project = ProjectRepository.FindByGuid(GetRequestStr("project_guid"));

                return HttpResponse.Item("boards", new BoardTransformer().Many(
                    project.Boards()
                ));
            });
        }
    }
}