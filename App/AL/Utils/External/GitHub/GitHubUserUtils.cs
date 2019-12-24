using App.DL.Enum;
using Octokit;
using User = App.DL.Model.User.User;

namespace App.AL.Utils.External.GitHub {
    public static class GitHubUserUtils {
        public static (bool finished, string originUserId) UpdateOriginUserId(User me) {
            var serviceAccessToken = me.ServiceAccessToken(ServiceType.GitHub);

            if (serviceAccessToken == null) {
                return (false, "");
            }

            var client = new GitHubClient(new ProductHeaderValue("SupportHub"));
            client.Credentials = new Credentials(serviceAccessToken.access_token);

            var githubUser = client.User.Current().Result;

            var originUserId = githubUser.Id.ToString();
            
            serviceAccessToken.UpdateCol("origin_user_id", originUserId);
            
            return (true, originUserId);
        }
    }
}