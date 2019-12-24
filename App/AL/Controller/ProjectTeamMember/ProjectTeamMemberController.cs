using App.DL.Repository.Project;
using App.DL.Repository.ProjectTeamMember;
using App.DL.Repository.User;
using App.PL.Transformer.ProjectTeamMember;
using Micron.AL.Validation.Db;
using Micron.DL.Middleware;
using Micron.DL.Module.Controller;
using Micron.DL.Module.Http;
using Micron.DL.Module.Validator;

namespace App.AL.Controller.ProjectTeamMember {
    public class ProjectTeamMemberController : BaseController {
        protected override IMiddleware[] Middleware() => new IMiddleware[] {};

        public ProjectTeamMemberController() {
            Get("/api/v1/project_team_member/get", _ => {
                var errors = ValidationProcessor.Process(Request, new IValidatorRule[] {
                    new ExistsInTable("project_guid", "projects", "guid"),
                    new ExistsInTable("user_guid", "users", "guid")
                });
                if (errors.Count > 0) {
                    return HttpResponse.Errors(errors);
                }

                var project = ProjectRepository.FindByGuid(GetRequestStr("project_guid"));
                var user = UserRepository.FindByGuid(GetRequestStr("user_guid"));

                var teamMember = ProjectTeamMemberRepository.Find(project, user);

                return HttpResponse.Item(
                    "project_team_member",
                    teamMember != null ? new ProjectTeamMemberTransformer().Transform(teamMember) : null
                );
            });
        }
    }
}