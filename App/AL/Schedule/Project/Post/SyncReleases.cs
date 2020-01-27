using System;
using System.Linq;
using System.Threading.Tasks;
using App.AL.Middleware.Schedule;
using App.DL.Enum;
using App.DL.Model.Project.Post;
using App.DL.Model.Repo;
using Micron.DL.Middleware;
using Micron.DL.Module.Config;
using Micron.DL.Module.Controller;
using Micron.DL.Module.Http;
using Newtonsoft.Json.Linq;
using Octokit;
using Sentry;

namespace App.AL.Schedule.Project.Post {
    public class SyncReleases : BaseController {
        protected override IMiddleware[] Middleware() => new IMiddleware[] {
            new ScheduleAuth(),
        };

        public SyncReleases() {
            Post("/api/v1/schedule/project/sync_releases/start", _ => {
                Task.Run(() => {
                    try {
                        var githubClient = new GitHubClient(new ProductHeaderValue("GitCom"));
                        var githubToken = AppConfig.GetConfiguration("auth:external:github:token");
                        if (githubToken != null) githubClient.Credentials = new Credentials(githubToken);

                        int pageIndex = 1;
                        var repos = Repo.Paginate(pageIndex);

                        while (repos.Length > 0) {
                            foreach (var repo in repos) {
                                if (repo.service_type != RepoServiceType.GitHub) continue;
                                var splitUrl = repo.repo_url.Split("/");
                                var releases = githubClient.Repository.Release
                                    .GetAll(splitUrl[3], splitUrl[4]).Result;
                                releases = releases.OrderBy(x => x.Id).ToArray();

                                foreach (var release in releases) {
                                    if (release.Body.Length < 100) continue;

                                    var existingPost = ProjectPost.FindBy("origin_id", release.Id.ToString());
                                    if (existingPost != null) continue;

                                    var post = ProjectPost.Create(
                                        repo.Project(), $"Released {release.Name}", release.Body
                                    );
                                    post.UpdateCol("origin_id", release.Id.ToString());
                                    post.UpdateCol("created_at", release.PublishedAt.ToString());
                                }
                            }

                            ++pageIndex;
                            repos = Repo.Paginate(pageIndex);
                        }
                    }
                    catch (Exception e) {
                        SentrySdk.CaptureException(e);
                    }
                });
                return HttpResponse.Data(new JObject());
            });
        }
    }
}