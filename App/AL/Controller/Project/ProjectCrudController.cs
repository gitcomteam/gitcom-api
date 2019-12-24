using System.Collections.Generic;
using App.DL.Repository.Project;
using App.DL.Repository.Repo;
using App.DL.Repository.User;
using App.PL.Transformer.Project;
using Micron.AL.Validation.Basic;
using Micron.AL.Validation.Db;
using Micron.DL.Middleware;
using Micron.DL.Middleware.Auth;
using Micron.DL.Module.Controller;
using Micron.DL.Module.Http;
using Micron.DL.Module.Validator;
using Nancy;
using Newtonsoft.Json.Linq;

namespace App.AL.Controller.Project {
    public sealed class ProjectCrudController : BaseController {
        protected override IMiddleware[] Middleware() => new IMiddleware[] {
            new JwtMiddleware()
        };

        public ProjectCrudController() {
            Post("/api/v1/project/create", _ => {
                var errors = ValidationProcessor.Process(Request, new IValidatorRule[] {
                    new ShouldHaveParameters(new[] {"name", "creator_guid", "repository_guid"}),
                    new ShouldNotExistInTable("name", "projects")
                });
                if (errors.Count > 0) {
                    return HttpResponse.Errors(errors);
                }

                var creator = UserRepository.FindByGuid(Request.Query["creator_guid"]);
                var repository = RepoRepository.FindByGuid(Request.Query["repository_guid"]);
                
                var project = ProjectRepository.CreateAndGet((string) Request.Query["name"], creator,
                    repository
                );

                return HttpResponse.Item(
                    "project", new ProjectTransformer().Transform(project), HttpStatusCode.Created
                );
            });

            Patch("/api/v1/project/edit", _ => {
                var rules = new List<IValidatorRule>() {
                    new ExistsInTable("project_guid", "projects", "guid")
                };

                var errors = ValidationProcessor.Process(Request, rules);
                if (errors.Count > 0) {
                    return HttpResponse.Errors(errors);
                }

                var project = ProjectRepository.FindByGuid((string) Request.Query["project_guid"]);

                project = ProjectRepository.UpdateAndRefresh(project, new JObject() {
                    ["name"] = (string) Request.Query["name"]
                });

                return HttpResponse.Item("project", new ProjectTransformer().Transform(project));
            });
            
            Delete("/api/v1/project/delete", _ => {
                var errors = ValidationProcessor.Process(Request, new IValidatorRule[] {
                    new ExistsInTable("project_guid", "projects", "guid"),
                });
                if (errors.Count > 0) {
                    return HttpResponse.Errors(errors);
                }

                var project = ProjectRepository.FindByGuid((string) Request.Query["project_guid"]);
                ProjectRepository.Delete(project);

                return HttpResponse.Item("project", new ProjectTransformer().Transform(project));
            });
        }
    }
}