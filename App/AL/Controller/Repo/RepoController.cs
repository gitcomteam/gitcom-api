using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using App.DL.Enum;
using App.DL.Repository.Repo;
using App.PL.Transformer.Repo;
using Micron.AL.Validation.Db;
using Micron.DL.Middleware;
using Micron.DL.Module.Controller;
using Micron.DL.Module.Http;
using Micron.DL.Module.Validator;
using Newtonsoft.Json.Linq;
using Sentry;
using YamlDotNet.Serialization;

namespace App.AL.Controller.Repo {
    public sealed class RepoController : BaseController {
        protected override IMiddleware[] Middleware() => new IMiddleware[] { };
        
        public RepoController() {
            Get("/api/v1/repository/get", _ => {
                var errors = ValidationProcessor.Process(Request, new IValidatorRule[] {
                    new ExistsInTable("repo_guid", "repositories", "guid"),
                });
                if (errors.Count > 0) return HttpResponse.Errors(errors);
 
                return HttpResponse.Item("repository", new RepoTransformer().Transform(
                    RepoRepository.FindByGuid((string) Request.Query["repo_guid"])
                ));
            });

            Get("/api/v1/repository/meta/get", _ => {
                var errors = ValidationProcessor.Process(Request, new IValidatorRule[] {
                    new ExistsInTable("repo_guid", "repositories", "guid"),
                });
                if (errors.Count > 0) return HttpResponse.Errors(errors);

                var sponsorLinks = new JObject();
                
                var repo = RepoRepository.FindByGuid((string) Request.Query["repo_guid"]);

                if (repo.service_type == RepoServiceType.GitHub) {
                    try {
                        var splitUrl = repo.repo_url.Split("/");
                        var response = new HttpClient().GetAsync(
                            $"https://raw.githubusercontent.com/{splitUrl[3]}/{splitUrl[4]}/master/.github/FUNDING.yml"
                        ).Result.Content.ReadAsStringAsync().Result;
                        var yamlObject = (Dictionary<object, object>) new DeserializerBuilder().Build()
                            .Deserialize(new StringReader(response));
                        sponsorLinks["github"] = yamlObject["github"]?.ToString();
                        sponsorLinks["patreon"] = yamlObject["patreon"]?.ToString();
                        sponsorLinks["open_collective"] = yamlObject["open_collective"]?.ToString();
                    }
                    catch (Exception e) {
                        SentrySdk.CaptureException(e);
                    }
                }

                return HttpResponse.Data(new JObject() {
                    ["repository"] = new RepoTransformer().Transform(repo)
                    ["meta"] = new JObject() {
                        ["sponsor_links"] = sponsorLinks
                    }
                });
            });
        }
    }
}