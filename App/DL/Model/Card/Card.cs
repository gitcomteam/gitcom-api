using System;
using System.Linq;
using App.DL.Model.Work;
using App.DL.Repository.Project;
using Dapper;
using UserModel = App.DL.Model.User.User;
using BoardColumnModel = App.DL.Model.BoardColumn.BoardColumn;

// ReSharper disable InconsistentNaming

namespace App.DL.Model.Card {
    public class Card : Micron.DL.Model.Model {
        public int id;

        public string guid;

        public string name;

        public string description;
        
        public int creator_id;

        public int column_order;

        public int column_id;

        public DateTime created_at;

        public DateTime updated_at;

        public static Card Find(int id)
            => Connection().Query<Card>(
                "SELECT * FROM cards WHERE id = @id LIMIT 1", new {id}
            ).FirstOrDefault();

        public static Card FindByGuid(string guid)
            => Connection().Query<Card>(
                "SELECT * FROM cards WHERE guid = @guid LIMIT 1", new {guid}
            ).FirstOrDefault();

        public static Card Find(string name, UserModel creator, BoardColumnModel column) {
            return Connection().Query<Card>(
                "SELECT * FROM cards WHERE name = @name AND creator_id = @creator_id AND column_id = @column_id  LIMIT 1",
                new {
                    name, creator_id = creator.id, column_id = column.id
                }
            ).FirstOrDefault();
        }

        public static int Create(
            string name, string description, int columnOrder, BoardColumnModel column, UserModel creator
        ) {
            return ExecuteScalarInt(
                @"INSERT INTO cards(guid, name, description, column_order, column_id, creator_id) 
                VALUES (@guid, @name, @description, @column_order, @column_id, @creator_id); SELECT currval('cards_id_seq');"
                , new {
                    guid = Guid.NewGuid().ToString(), name, description, column_order = columnOrder, 
                    column_id = column.id, creator_id = creator?.id
                }
            );
        }

        public Card Save() {
            ExecuteSql(
                @"UPDATE cards 
                SET name = @name, description = @description, column_order = @column_order, column_id = @column_id
                WHERE id = @id",
                new {name, description, column_order, column_id, id}
            );
            return this;
        }

        public Card Refresh() => Find(id);

        public static int Count() => ExecuteScalarInt("SELECT count(*) FROM cards WHERE id = @id");

        public void Delete() => ExecuteScalarInt("DELETE FROM cards WHERE id = @id", new {id});

        public BoardColumnModel Column() => BoardColumnModel.Find(column_id);

        public Board.Board Board() => Column().Board();

        public Project.Project Project() {
            var projectId = ExecuteScalarInt(@"SELECT projects.id
                FROM cards
                LEFT JOIN board_columns ON cards.column_id = board_columns.id
                LEFT JOIN boards ON board_columns.board_id = boards.id
                LEFT JOIN projects ON boards.project_id = projects.id
                WHERE cards.id = @id LIMIT 1;
            ", new {id});
            return ProjectRepository.Find(projectId);
        }

        public UserModel Creator() => UserModel.Find(creator_id);
        
        public CardWork[] SubmittedWork(int limit = 10)
            => Connection().Query<CardWork>(
                @"SELECT * FROM card_works WHERE card_id = @card_id LIMIT @limit",
                new {card_id = id, limit}
            ).ToArray();
    }
}
