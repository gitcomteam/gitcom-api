using System;
using System.Net.Http;
using Micron.DL.Module.Config;
using Newtonsoft.Json.Linq;
using Octokit;

namespace App.DL.External.GitHub {
    public static class GitHubApi {
        public static GitHubClient Client() {
            var client = new GitHubClient(new ProductHeaderValue("GitCom"));
            var githubToken = Token();
            if (githubToken != null) client.Credentials = new Credentials(githubToken);
            return client;
        }

        public static string Token() => AppConfig.GetConfiguration("auth:external:github:token");

        public static HttpClient HttpClient() {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("User-Agent", "PostmanRuntime/7.20.1");
            client.DefaultRequestHeaders.Add("Authorization", $"token {Token()}");
            return client;
        }

        public static int TimeUntilReset() {
            var result = HttpClient().GetAsync(
                "https://api.github.com/rate_limit"
            ).Result.Content.ReadAsStringAsync().Result;
            var timestamp = JObject.Parse(result)["rate"].Value<int>("reset");

            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0)
                .AddSeconds(timestamp);
            TimeSpan diff = origin - DateTime.Now;
            return diff.Seconds + 10;
        }
    }
}