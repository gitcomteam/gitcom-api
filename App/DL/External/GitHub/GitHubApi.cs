using Micron.DL.Module.Config;
using Octokit;

namespace App.DL.External.GitHub {
    public static class GitHubApi {
        public static GitHubClient Client() {
            var client = new GitHubClient(new ProductHeaderValue("GitCom"));
            var githubToken = AppConfig.GetConfiguration("auth:external:github:token");
            if (githubToken != null) client.Credentials = new Credentials(githubToken);
            return client;
        }
    }
}