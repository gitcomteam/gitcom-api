using System.Collections.Generic;
using App.DL.Enum;
using App.DL.Repository.Repo;
using App.DL.Repository.User;
using App.PL.Transformer.Repo;
using Micron.AL.Validation.Basic;
using Micron.AL.Validation.Db;
using Micron.AL.Validation.String;
using Micron.DL.Middleware;
using Micron.DL.Middleware.Auth;
using Micron.DL.Module.Controller;
using Micron.DL.Module.Http;
using Micron.DL.Module.Validator;
using Nancy;
using Newtonsoft.Json.Linq;

namespace App.AL.Controller.Repo {
    public sealed class RepoCrudController : BaseController {
        protected override IMiddleware[] Middleware() => new IMiddleware[] {
            new JwtMiddleware()
        };

        public RepoCrudController() {
            Post("/api/v1/repository/create", _ => {
                var errors = ValidationProcessor.Process(Request, new IValidatorRule[] {
                    new ShouldHaveParameters(new[] {"title", "repo_url", "service_type"}),
                    new ShouldBeCorrectEnumValue("service_type", typeof(RepoServiceType)),
                    new ShouldNotExistInTable("repo_url", "repositories")
                });
                if (errors.Count > 0) {
                    return HttpResponse.Errors(errors);
                }

                var me = UserRepository.Find(CurrentRequest.UserId);
                
                var repository = RepoRepository.CreateAndGet(
                    me, (string) Request.Query["title"], (string) Request.Query["repo_url"],
                    (RepoServiceType) GetRequestEnum("service_type", typeof(RepoServiceType))
                );

                return HttpResponse.Item(
                    "repository", new RepoTransformer().Transform(repository), HttpStatusCode.Created
                );
            });

            Patch("/api/v1/repository/edit", _ => {
                var rules = new List<IValidatorRule>() {
                    new ExistsInTable("repo_guid", "repositories", "guid")
                };

                if (Request.Query["repo_url"]) {
                    rules.Add(new ShouldNotExistInTable("repo_url", "repositories"));
                }
                
                var errors = ValidationProcessor.Process(Request, rules);
                if (errors.Count > 0) {
                    return HttpResponse.Errors(errors);
                }

                var repo = RepoRepository.FindByGuid((string) Request.Query["repo_guid"]);

                repo = RepoRepository.UpdateAndRefresh(repo, new JObject() {
                    ["title"] = (string) Request.Query["title"],
                    ["repo_url"] = (string) Request.Query["repo_url"]
                });

                return HttpResponse.Item("repository", new RepoTransformer().Transform(repo));
            });
            
            Delete("/api/v1/repository/delete", _ => {
                var errors = ValidationProcessor.Process(Request, new IValidatorRule[] {
                    new ExistsInTable("repo_guid", "repositories", "guid"),
                });
                if (errors.Count > 0) {
                    return HttpResponse.Errors(errors);
                }

                var repo = RepoRepository.FindByGuid((string) Request.Query["repo_guid"]);
                repo.Delete();

                return HttpResponse.Item("repository", new RepoTransformer().Transform(repo));
            });
        }
    }
}