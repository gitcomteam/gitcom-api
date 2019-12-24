using App.AL.Validation.Permission;
using App.DL.Enum;
using App.DL.Repository.Project;
using App.DL.Repository.ProjectTeamMember;
using App.DL.Repository.User;
using App.PL.Transformer.ProjectTeamMember;
using Micron.AL.Validation.Basic;
using Micron.AL.Validation.Db;
using Micron.DL.Middleware;
using Micron.DL.Middleware.Auth;
using Micron.DL.Module.Controller;
using Micron.DL.Module.Http;
using Micron.DL.Module.Validator;
using Nancy;

namespace App.AL.Controller.ProjectTeamMember {
    public sealed class ProjectTeamMemberCrudController : BaseController {
        protected override IMiddleware[] Middleware() => new IMiddleware[] {
            new JwtMiddleware()
        };

        public ProjectTeamMemberCrudController() {
            Post("/api/v1/project_team_member/create", _ => {
                var me = UserRepository.Find(CurrentRequest.UserId);
                var project = ProjectRepository.FindByGuid(GetRequestStr("project_guid"));
                
                var errors = ValidationProcessor.Process(Request, new IValidatorRule[] {
                    new ShouldHaveParameters(new[] {"project_guid", "user_guid"}),
                    new ExistsInTable("project_guid", "projects", "guid"), 
                    new ExistsInTable("user_guid", "users", "guid"),
                    new HasPermission(me, project.id, EntityType.Project),
                }, true);
                if (errors.Count > 0) {
                    return HttpResponse.Errors(errors);
                }
                
                var user = UserRepository.FindByGuid(GetRequestStr("user_guid"));

                var projectTeamMember = ProjectTeamMemberRepository.CreateAndGet(project, user);

                return HttpResponse.Item(
                    "project_team_member", 
                    new ProjectTeamMemberTransformer().Transform(projectTeamMember), HttpStatusCode.Created
                );
            });

            

            Delete("/api/v1/project_team_member/delete", _ => {
                var me = UserRepository.Find(CurrentRequest.UserId);
                var project = ProjectRepository.FindByGuid(GetRequestStr("project_guid"));
                
                var errors = ValidationProcessor.Process(Request, new IValidatorRule[] {
                    new ExistsInTable("project_guid", "projects", "guid"),
                    new ExistsInTable("user_guid", "users", "guid"),
                    new HasPermission(me, project.id, EntityType.Project)
                }, true);
                if (errors.Count > 0) {
                    return HttpResponse.Errors(errors);
                }

                var projectTeamMember = ProjectTeamMemberRepository.FindByProjectAndUser(
                    GetRequestStr("project_guid"),
                    GetRequestStr("user_guid")
                );
                projectTeamMember.Delete();

                return HttpResponse.Item(
                    "project_team_member", new ProjectTeamMemberTransformer().Transform(projectTeamMember)
                );
            });
        }
    }
}