using System;
using System.Linq;
using App.DL.Enum;
using App.DL.Model.Auth;
using App.DL.Repository.Auth;
using App.DL.Repository.Project;
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

        public static int Create(string email, string login, string password)
            => ExecuteScalarInt(
                @"INSERT INTO public.users(guid, email, login, password) VALUES (@guid, @email, @login, @password);
                SELECT currval('users_id_seq');"
                , new {guid = Guid.NewGuid().ToString(), email, login, password = Encryptor.Encrypt(password)}
            );

        public ServiceAccessToken ServiceAccessToken(ServiceType serviceType) =>
            ServiceAccessTokenRepository.Find(this, serviceType);

        public Project.Project[] Projects() => ProjectRepository.GetBy("creator_id", id);
    }
}