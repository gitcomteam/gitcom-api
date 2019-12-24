using App.DL.Enum;
using Newtonsoft.Json.Linq;
using UserModel = App.DL.Model.User.User;
using RepoModel = App.DL.Model.Repo.Repo;

namespace App.DL.Repository.Repo {
    public static class RepoRepository {
        public static RepoModel Find(int id) {
            return RepoModel.Find(id);
        }

        public static RepoModel FindByGuid(string guid) {
            return RepoModel.FindByGuid(guid);
        }

        public static RepoModel FindBy(string col, string val) {
            return RepoModel.FindBy(col, val);
        }

        public static RepoModel Find(string originId, RepoServiceType type) {
            return RepoModel.Find(originId, type);
        }

        public static RepoModel CreateAndGet(
            UserModel creator, string title, string repoUrl, RepoServiceType serviceType, string originId = ""
        ) {
            return RepoModel.Find(RepoModel.Create(creator, title, repoUrl, serviceType, originId));
        }

        public static RepoModel UpdateAndRefresh(RepoModel model, JObject data) {
            model.title = data.Value<string>("title") ?? model.title;
            model.repo_url = data.Value<string>("repo_url") ?? model.repo_url;
            return model.Save();
        }
    }
}