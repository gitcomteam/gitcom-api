using System;
using System.Linq;
using Dapper;

namespace App.DL.Model.MailingList {
    public class MailingListSubscriber : Micron.DL.Model.Model {
        public int id;

        public string guid;

        public string email;

        public string unsubscribe_key;

        public DateTime created_at;

        public static MailingListSubscriber Find(int id)
            => Connection().Query<MailingListSubscriber>(
                "SELECT * FROM mailing_list_subscribers WHERE id = @id LIMIT 1", new {id}
            ).FirstOrDefault();

        public static MailingListSubscriber FindBy(string col, string val)
            => Connection().Query<MailingListSubscriber>(
                $"SELECT * FROM mailing_list_subscribers WHERE {col} = @val LIMIT 1", new {val}
            ).FirstOrDefault();

        public static MailingListSubscriber FindBy(string col, int val)
            => Connection().Query<MailingListSubscriber>(
                $"SELECT * FROM mailing_list_subscribers WHERE {col} = @val LIMIT 1", new {val}
            ).FirstOrDefault();

        public static int Create(string email) {
            return ExecuteScalarInt(
                @"INSERT INTO public.mailing_list_subscribers(guid, email, unsubscribe_key)
                        VALUES (@guid, @email, @unsubscribe_key); SELECT currval('mailing_list_subscribers_id_seq');"
                , new {
                    guid = Guid.NewGuid().ToString(), email,
                    unsubscribe_key = Guid.NewGuid().ToString() + Guid.NewGuid()
                }
            );
        }
        
        public void Delete() => ExecuteScalarInt("DELETE FROM mailing_list_subscribers WHERE id = @id", new {id});
    }
}