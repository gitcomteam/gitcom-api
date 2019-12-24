using System;
using App.DL.Repository.User;
using App.DL.Repository.BoardColumn;
using Newtonsoft.Json.Linq;
using CardModel = App.DL.Model.Card.Card;
using UserModel = App.DL.Model.User.User;
using BoardColumnModel = App.DL.Model.BoardColumn.BoardColumn;

namespace App.DL.Repository.Card {
    public static class CardRepository {
        public static CardModel Find(int id) {
            return CardModel.Find(id);
        }

        public static CardModel FindByGuid(string guid) {
            return CardModel.FindByGuid(guid);
        }

        public static CardModel Find(string name, UserModel creator, BoardColumnModel column) {
            return CardModel.Find(name, creator, column);
        }

        public static CardModel CreateAndGet(
            string name, string description, int columnOrder, BoardColumnModel column, UserModel creator
        ) {
            return CardModel.Find(CardModel.Create(name, description, columnOrder, column, creator));
        }

        public static CardModel UpdateAndRefresh(CardModel model, JObject data) {
            var column = BoardColumnRepository.FindByGuid(data.Value<string>("column_guid"));

            if (column != null) {
                model.column_id = column.id;
            }

            model.name = data.Value<string>("name") ?? model.name;
            model.column_order = data.Value<int?>("column_order") ?? model.column_order;
            model.description = data.Value<string>("description") ?? model.description;
            return model.Save();
        }
    }
}