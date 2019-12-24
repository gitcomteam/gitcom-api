using System;
using System.Linq;
using App.DL.Repository.Card;
using App.DL.Repository.User;
using App.DL.Repository.Work;
using Dapper;

namespace App.DL.Model.Work {
    public class CardWork : Micron.DL.Model.Model {
        public int id;

        public string guid;

        public int user_id;

        public int card_id;

        public int work_type_id;

        public string proof;

        public DateTime created_at;

        public DateTime updated_at;

        public static CardWork Find(int id)
            => Connection().Query<CardWork>(
                "SELECT * FROM card_works WHERE id = @id LIMIT 1", new {id}
            ).FirstOrDefault();

        public static CardWork FindBy(string col, string val)
            => Connection().Query<CardWork>(
                $"SELECT * FROM card_works WHERE {col} = @val LIMIT 1", new {val}
            ).FirstOrDefault();

        public static CardWork FindBy(string col, int val)
            => Connection().Query<CardWork>(
                $"SELECT * FROM card_works WHERE {col} = @val LIMIT 1", new {val}
            ).FirstOrDefault();

        public static int Create(User.User user, Card.Card card, ProjectWorkType workType, string proof)
            => ExecuteScalarInt(
                @"INSERT INTO card_works(guid, user_id, card_id, work_type_id, proof)
                               VALUES (@guid, @user_id, @card_id, @work_type_id, @proof);
                               SELECT currval('card_works_id_seq');"
                , new {
                    guid = Guid.NewGuid().ToString(), user_id = user.id, card_id = card.id, work_type_id = workType.id,
                    proof
                }
            );

        public void Update(string col, string val) {
            ExecuteSql(
                $"UPDATE card_works SET {col} = @val, updated_at = CURRENT_TIMESTAMP WHERE id = @id",
                new {val, id}
            );
        }

        public User.User User() => UserRepository.Find(user_id);

        public Card.Card Card() => CardRepository.Find(card_id);

        public ProjectWorkType WorkType() => ProjectWorkTypeRepository.Find(work_type_id);
    }
}