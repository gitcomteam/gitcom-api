using System;
using System.Linq;
using System.Threading.Tasks;
using App.AL.Middleware.Schedule;
using App.DL.External.GitHub;
using App.DL.Model.Card;
using App.DL.Module.Schedule;
using App.DL.Repository.Card;
using Micron.DL.Middleware;
using Micron.DL.Module.Controller;
using Micron.DL.Module.Http;
using Newtonsoft.Json.Linq;
using Octokit;
using Sentry;

namespace App.AL.Schedule.Sync.Issues {
    public class SyncIssues : BaseController {
        protected override IMiddleware[] Middleware() => new IMiddleware[] {
            new ScheduleAuth(),
        };

        public SyncIssues() {
            Post("/api/v1/schedule/issues/sync/start", _ => {
                var task = Task.Run(() => {
                    var githubClient = GitHubApi.Client();

                    int pageIndex = 1;
                    var projects = DL.Model.Project.Project.Paginate(pageIndex);

                    while (projects.Length > 0) {
                        foreach (var project in projects) {
                            var board = project.Boards().First(x => x.name == "Development");
                            if (board == null) continue;

                            var todoColumn = board.Columns().First(c => c.name == "TODO");
                            if (todoColumn == null) continue;

                            try {
                                var originId = project.Repository().origin_id;
                                Console.WriteLine($"getting issues for repo {originId}");
                                var issues = githubClient.Issue.GetAllForRepository(
                                    Convert.ToInt64(originId)
                                    , new ApiOptions() {
                                        PageSize = 100
                                    }
                                ).Result;
                                foreach (var issue in issues) {
                                    try {
                                        Console.WriteLine($"Checking issue {issue.HtmlUrl}");
                                        var existingCard = Card.FindBy("origin_id", issue.Id.ToString());
                                        if (existingCard != null) continue;
                                        var card = CardRepository.CreateAndGet(
                                            issue.Title, issue.Body ?? "", 1, todoColumn, null
                                        );
                                        card.UpdateCol("origin_id", issue.Id.ToString());
                                    }
                                    catch (Exception e) {
                                        Console.WriteLine(e.Message);
                                    }
                                }
                            }
                            catch (Exception e) {
                                Console.WriteLine(e.Message);
                                SentrySdk.CaptureException(e);
                            }
                        }

                        ++pageIndex;
                        projects = DL.Model.Project.Project.Paginate(pageIndex);
                    }
                    
                    Console.WriteLine("Finished!");
                });
                JobsPool.Get().Push(task);
                return HttpResponse.Data(new JObject());
            });
        }
    }
}