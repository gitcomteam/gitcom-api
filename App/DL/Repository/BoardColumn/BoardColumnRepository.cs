using Newtonsoft.Json.Linq;
using BoardColumnModel = App.DL.Model.BoardColumn.BoardColumn;
using BoardModel = App.DL.Model.Board.Board;

namespace App.DL.Repository.BoardColumn {
    public static class BoardColumnRepository {
        public static BoardColumnModel Find(int id) {
            return BoardColumnModel.Find(id);
        }

        public static BoardColumnModel FindByGuid(string guid) {
            return BoardColumnModel.FindByGuid(guid);
        }

        public static BoardColumnModel Find(BoardModel board, short boardOrder) {
            return BoardColumnModel.Find(board, boardOrder);
        }

        public static BoardColumnModel CreateAndGet(string name, BoardModel board, short boardOrder) {
            return BoardColumnModel.Find(BoardColumnModel.Create(name, board, boardOrder));
        }

        public static BoardColumnModel UpdateAndRefresh(BoardColumnModel model, JObject data) {
            model.name = data.Value<string>("name") ?? model.name;
            model.board_order = data.Value<short?>("board_order") ?? model.board_order;
            return model.Save();
        }
    }
}