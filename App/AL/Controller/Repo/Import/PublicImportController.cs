using System;
using System.Linq;
using App.AL.Utils.External.GitHub;
using App.DL.Enum;
using App.DL.External.GitHub;
using App.DL.Repository.Repo;
using App.PL.Transformer.Project;
using App.PL.Transformer.Repo;
using Micron.AL.Validation.Basic;
using Micron.DL.Middleware;
using Micron.DL.Module.Controller;
using Micron.DL.Module.Http;
using Micron.DL.Module.Validator;
using Nancy;
using Newtonsoft.Json.Linq;
using Octokit;

namespace App.AL.Controller.Repo.Import {
    public class PublicImportController : BaseController {
        protected override IMiddleware[] Middleware() => new IMiddleware[] {};

        // TODO: add support for GitLab
        public PublicImportController() {
            Post("/api/v1/repository/submit/post", _ => {
                var errors = ValidationProcessor.Process(Request, new IValidatorRule[] {
                    new ShouldHaveParameters(new[] {"url"}),
                });
                if (errors.Count > 0) return HttpResponse.Errors(errors);

                var url = GetRequestStr("url");

                Uri parsedUrl;
                try {
                    parsedUrl = new Uri(url);
                }
                catch (Exception e) {
                    return HttpResponse.Error(HttpStatusCode.UnprocessableEntity, "Invalid url");
                }

                string host = parsedUrl.Host;

                if (
                    !(new[] {"github.com"}.Contains(host)) || parsedUrl.Segments.Length < 3
                ) {
                    return HttpResponse.Error(HttpStatusCode.UnprocessableEntity, "Invalid hostname");
                }

                RepoServiceType serviceType = host == "github.com" ? RepoServiceType.GitHub : RepoServiceType.GitLab;
                
                (DL.Model.Project.Project project, DL.Model.Repo.Repo repo) result = (null, null);

                if (serviceType == RepoServiceType.GitHub) {
                    var githubClient = GitHubApi.Client();
                    Repository repo;
                    try {
                        repo = githubClient.Repository.Get(
                            parsedUrl.Segments[1].Replace("/", ""),
                            parsedUrl.Segments[2].Replace("/", "")
                        ).Result;
                    }
                    catch (Exception e) {
                        return HttpResponse.Error(HttpStatusCode.NotFound, "GitHub repository does not exist");
                    }
                    
                    var existingRepo = RepoRepository.Find(repo.Id.ToString(), serviceType);

                    if (existingRepo != null) {
                        return HttpResponse.Error(HttpStatusCode.UnprocessableEntity, "Project is already imported",
                            new JObject() {
                                ["project"] = new ProjectTransformer().Transform(existingRepo.Project()),
                            });
                    }

                    result = GitHubRepositoriesUtils.ImportProject(null, repo.Id.ToString());
                }

                return HttpResponse.Data(new JObject() {
                    ["project"] = new ProjectTransformer().Transform(result.project),
                    ["repository"] = new RepoTransformer().Transform(result.repo)
                });
            });
        }
    }
}