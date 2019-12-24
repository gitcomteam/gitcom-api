using Newtonsoft.Json.Linq;
using ProjectModel = App.DL.Model.Project.Project;
using UserModel = App.DL.Model.User.User;
using BoardModel = App.DL.Model.Board.Board;

namespace App.DL.Repository.Board {
    public static class BoardRepository {
        public static BoardModel Find(int id) {
            return BoardModel.Find(id);
        }

        public static BoardModel FindByGuid(string guid) {
            return BoardModel.FindByGuid(guid);
        }

        public static BoardModel Find(ProjectModel project, UserModel creator) {
            return BoardModel.Find(project, creator);
        }

        public static BoardModel CreateAndGet(string name, string description, ProjectModel project, UserModel user) {
            return BoardModel.Find(BoardModel.Create(name, description, project, user));
        }

        public static BoardModel UpdateAndRefresh(BoardModel model, JObject data) {
            model.name = data.Value<string>("name") ?? model.name;
            model.description = data.Value<string>("description") ?? model.description;
            return model.Save();
        }
    }
}