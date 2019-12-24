using App.DL.Enum;
using App.DL.External.GitLab;
using User = App.DL.Model.User.User;

namespace App.AL.Utils.External.GitLab {
    public static class GitLabUserUtils {

        public static (bool finished, string originUserId) UpdateOriginUserId(User me) {
            var serviceAccessToken = me.ServiceAccessToken(ServiceType.GitLab);

            if (serviceAccessToken == null) {
                return (false, "");
            }

            var client = new GitLabClient(serviceAccessToken.access_token);
            client.SetAuthorizedUser();

            var originUserId = client.User.Id.ToString();

            serviceAccessToken.UpdateCol("origin_user_id", originUserId);

            return (true, originUserId);
        }
    }
}