using System;
using System.Linq;
using Dapper;
using BoardModel = App.DL.Model.Board.Board;

// ReSharper disable InconsistentNaming

namespace App.DL.Model.BoardColumn {
    public class BoardColumn : Micron.DL.Model.Model {
        public int id;

        public string guid;

        public string name;

        public short board_order;

        public int board_id;


        public static BoardColumn Find(int id)
            => Connection().Query<BoardColumn>(
                "SELECT * FROM board_columns WHERE id = @id LIMIT 1", new {id}
            ).FirstOrDefault();

        public static BoardColumn FindByGuid(string guid)
            => Connection().Query<BoardColumn>(
                "SELECT * FROM board_columns WHERE guid = @guid LIMIT 1", new {guid}
            ).FirstOrDefault();

        public static BoardColumn Find(BoardModel board, short boardOrder) {
            return Connection().Query<BoardColumn>(
                "SELECT * FROM board_columns WHERE board_id = @board_id AND board_order = @board_order  LIMIT 1",
                new {
                    board_id = board.id, board_order = boardOrder
                }
            ).FirstOrDefault();
        }

        public static int Create(string name, BoardModel board, short boardOrder) {
            return ExecuteScalarInt(
                @"INSERT INTO board_columns(guid, name, board_id, board_order) 
                VALUES (@guid, @name, @board_id, @board_order); SELECT currval('board_columns_id_seq');"
                , new {guid = Guid.NewGuid().ToString(), name, board_id = board.id, board_order = boardOrder}
            );
        }

        public BoardColumn Save() {
            ExecuteSql(
                @"UPDATE board_columns 
                SET name = @name, board_id = @board_id, board_order = @board_order WHERE id = @id",
                new {name, board_id, board_order, id}
            );
            return this;
        }

        public BoardColumn Refresh() => Find(id);

        public static int Count() => ExecuteScalarInt("SELECT count(*) FROM board_columns WHERE id = @id");

        public void Delete() => ExecuteScalarInt("DELETE FROM board_columns WHERE id = @id", new {id});

        public BoardModel Board() => BoardModel.Find(board_id);

        public Card.Card[] Cards(int limit = 25)
            => Connection().Query<Card.Card>(
                @"SELECT * FROM cards WHERE column_id = @column_id LIMIT @limit",
                new {column_id = id, limit}
            ).ToArray();
    }
}
