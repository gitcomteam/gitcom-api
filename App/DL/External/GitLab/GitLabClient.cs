using System.Collections.Generic;
using System.Net.Http;
using App.DL.CustomObj.Repo;
using App.DL.Enum;
using App.DL.External.GitLab.User;
using Newtonsoft.Json.Linq;
using UserModel = App.DL.Model.User.User;

namespace App.DL.External.GitLab {
    public class GitLabClient {
        public GitLabUser User { get; set; }

        private string AccessToken { get; }
        
        public GitLabClient(string accessToken) { 
            AccessToken = accessToken;
            LogIn();
        }

        public bool LogIn() {
            var responseBody = "";
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                    
                var response = client.PostAsync(
                    "https://github.com/login/oauth/access_token", 
                    new FormUrlEncodedContent(new[] {
                        new KeyValuePair<string, string>("access_token", AccessToken),
                    })
                ).Result;

                if (response.IsSuccessStatusCode) {
                    responseBody = response.Content.ReadAsStringAsync().Result;
                }
            }

            if (string.IsNullOrEmpty(responseBody)) {
                return false;
            }
            
            SetAuthorizedUser();
            
            return true;
        }

        public void SetAuthorizedUser() {
            var responseBody = GitlabApi.GetMe(AccessToken);

            if (string.IsNullOrEmpty(responseBody)) {
                return;
            }

            var json = JObject.Parse(responseBody);

            User = new GitLabUser() {
                Id = json.Value<int>("id"),
                Login = json.Value<string>("username"),
                Email = json.Value<string>("email")
            };
        }

        public ExternalRepo[] GetMyPublicRepositories(UserModel user) {
            var response = GitlabApi.GetUserPublicProjects(User.Id.ToString());
            
            var result = new List<ExternalRepo>();
            
            var repos = JArray.Parse(response);

            foreach (var repo in repos.Children()) {
                result.Add(new ExternalRepo() {
                    Owner = user,
                    Id = repo.Value<string>("id"),
                    Name = User.Login + "/" + repo.Value<string>("name").ToLower().Replace(' ', '-'),
                    Description = repo.Value<string>("description"),
                    ServiceType = ServiceType.GitLab
                });
            }

            return result.ToArray();
        }
    }
}