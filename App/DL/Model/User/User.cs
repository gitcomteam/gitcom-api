using System;
using System.Linq;
using System.Net.Mail;
using App.DL.Enum;
using App.DL.Model.Auth;
using App.DL.Model.User.Badge;
using App.DL.Repository.Auth;
using App.DL.Repository.Project;
using App.DL.Repository.User.Registration;
using Micron.DL.Module.Crypto;
using Dapper;

// ReSharper disable InconsistentNaming

namespace App.DL.Model.User {
    public class User : Micron.DL.Model.Model {

        public int id;

        public string guid;

        public string login;

        public string password;

        public string email;

        public DateTime register_date;

        public static User Find(int id)
            => Connection().Query<User>(
                "SELECT * FROM users WHERE id = @id LIMIT 1",
                new {id}
            ).FirstOrDefault();

        public static User FindByEmail(string email)
            => Connection().Query<User>(
                "SELECT * FROM users WHERE email = @email LIMIT 1",
                new {email}
            ).FirstOrDefault();

        public static User FindByLogin(string login)
            => Connection().Query<User>(
                "SELECT * FROM users WHERE login = @login LIMIT 1",
                new {login}
            ).FirstOrDefault();

        public static User FindByGuid(string guid)
            => Connection().Query<User>(
                "SELECT * FROM users WHERE guid = @guid LIMIT 1",
                new {guid}
            ).FirstOrDefault();

        public static int Create(string email, string login, string password) {
            try {
                new MailAddress(email);
            }
            catch (Exception e) {
                Console.WriteLine(e.StackTrace);
                throw new Exception($"Invalid user email: {email}");
            }

            return ExecuteScalarInt(
                @"INSERT INTO public.users(guid, email, login, password) VALUES (@guid, @email, @login, @password);
                SELECT currval('users_id_seq');"
                , new {guid = Guid.NewGuid().ToString(), email, login, password = Encryptor.Encrypt(password)}
            );
        }

        public static User[] Paginate(int page, int size = 10)
            => Connection().Query<User>(
                "SELECT * FROM users OFFSET @offset LIMIT @size",
                new {offset = ((page - 1) * size), size}
            ).ToArray();

        public ServiceAccessToken ServiceAccessToken(ServiceType serviceType) =>
            ServiceAccessTokenRepository.Find(this, serviceType);

        public Project.Project[] Projects() => ProjectRepository.GetBy("creator_id", id);

        public UserBadge[] Badges() => UserBadge.Get(this);

        public bool EmailConfirmed() {
            var queuedItem = RegistrationQueueItemRepository.Find(this);
            return queuedItem == null || queuedItem.email_confirmed;
        }
    }
}