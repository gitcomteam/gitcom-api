using System;
using System.Linq;
using Dapper;
using Micron.DL.Module.Misc;

namespace App.DL.Model.User.Registration {
    public class RegistrationQueueItem : Micron.DL.Model.Model {
        public int id;

        public string guid;

        public int user_id;

        public bool email_confirmed;

        public string confirmation_key;

        public static RegistrationQueueItem Find(int id)
            => Connection().Query<RegistrationQueueItem>(
                "SELECT * FROM registration_queue WHERE id = @id LIMIT 1", new {id}
            ).FirstOrDefault();

        public static RegistrationQueueItem Find(User user)
            => Connection().Query<RegistrationQueueItem>(
                "SELECT * FROM registration_queue WHERE user_id = @user_id LIMIT 1", new {user_id = user.id}
            ).FirstOrDefault();

        public static RegistrationQueueItem FindBy(string col, string val)
            => Connection().Query<RegistrationQueueItem>(
                $"SELECT * FROM registration_queue WHERE {col} = @val LIMIT 1", new {val}
            ).FirstOrDefault();

        public void EmailConfirmed() {
            ExecuteSql("UPDATE registration_queue SET email_confirmed = true WHERE id = @id", new {id});
        }

        public static int Create(User user)
            => ExecuteScalarInt(
                @"INSERT INTO registration_queue(guid, user_id, confirmation_key)
                           VALUES (@guid, @user_id, @confirmation_key);
                           SELECT currval('registration_queue_id_seq');"
                , new {
                    guid = Guid.NewGuid().ToString(), user_id = user.id, confirmation_key = Rand.RandomString()
                }
            );
    }
}