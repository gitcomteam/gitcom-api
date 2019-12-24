using System;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace App.DL.External.GitLab {
    public static class GitlabApi {
        private const string BaseUrl = "https://gitlab.com/api/v4";

        private static string ApiUrl(string postfix) => BaseUrl + postfix;

        private static JObject SimpleRequest(string url) {
            using (var client = new HttpClient()) {
                client.DefaultRequestHeaders.Add("Accept", "application/json");

                var response = client.GetAsync(url).Result;

                if (response.IsSuccessStatusCode) {
                    var result = response.Content.ReadAsStringAsync().Result;
                    try {
                        return JObject.Parse(result);
                    }
                    catch (Exception e) {
                        return null;
                    }
                }
            }

            return null;
        }
        
        private static JArray SimpleArrayRequest(string url) {
            using (var client = new HttpClient()) {
                client.DefaultRequestHeaders.Add("Accept", "application/json");

                var response = client.GetAsync(url).Result;

                if (response.IsSuccessStatusCode) {
                    var result = response.Content.ReadAsStringAsync().Result;
                    try {
                        return JArray.Parse(result);
                    }
                    catch (Exception e) {
                        return null;
                    }
                }
            }

            return null;
        }

        public static string GetMe(string accessToken) {
            using (var client = new HttpClient()) {
                client.DefaultRequestHeaders.Add("Accept", "application/json");

                var response = client.GetAsync(
                    ApiUrl("/user") + $"?access_token={accessToken}"
                ).Result;

                if (response.IsSuccessStatusCode) {
                    return response.Content.ReadAsStringAsync().Result;
                }
            }

            return "";
        }

        public static string GetUserPublicProjects(string userId) {
            using (var client = new HttpClient()) {
                client.DefaultRequestHeaders.Add("Accept", "application/json");

                var response = client.GetAsync(
                    ApiUrl($"/users/{userId}/projects")
                ).Result;

                if (response.IsSuccessStatusCode) {
                    return response.Content.ReadAsStringAsync().Result;
                }
            }

            return "";
        }

        public static bool IfRepoExists(string originId) {
            try {
                var response = GetPublicProject(originId);
                return response.Value<int>("id") > 0;
            }
            catch (Exception e) {
                return false;
            }
        }

        public static JObject GetPublicProject(string projectId) {
            return SimpleRequest(ApiUrl($"/projects/{projectId}"));
        }

        public static JArray GetProjectUsers(string projectId) {
            return SimpleArrayRequest(ApiUrl($"/projects/{projectId}/users"));
        }
    }
}