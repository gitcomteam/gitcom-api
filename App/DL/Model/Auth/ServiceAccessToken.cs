using System;
using System.Linq;
using App.DL.Enum;
using Dapper;
using UserModel = App.DL.Model.User.User;

// ReSharper disable InconsistentNaming

namespace App.DL.Model.Auth {
    public class ServiceAccessToken : Micron.DL.Model.Model {
        public int id;

        public string guid;

        public int user_id;

        public string origin_user_id;

        public string access_token;

        public ServiceType service_type;

        public static ServiceAccessToken Find(int id)
            => Connection().Query<ServiceAccessToken>(
                "SELECT * FROM service_access_tokens WHERE id = @id LIMIT 1", new {id}
            ).FirstOrDefault();
        
        public static ServiceAccessToken FindBy(string col, string val)
            => Connection().Query<ServiceAccessToken>(
                $"SELECT * FROM service_access_tokens WHERE {col} = @val LIMIT 1", new {val}
            ).FirstOrDefault();

        public static ServiceAccessToken FindByGuid(string guid)
            => Connection().Query<ServiceAccessToken>(
                "SELECT * FROM service_access_tokens WHERE guid = @guid LIMIT 1", new {guid}
            ).FirstOrDefault();

        public static ServiceAccessToken Find(UserModel user, ServiceType serviceType)
            => Connection().Query<ServiceAccessToken>(
                $"SELECT * FROM service_access_tokens WHERE user_id = @user_id AND service_type = '{serviceType.ToString()}' LIMIT 1",
                new {user_id = user.id}
            ).FirstOrDefault();
        
        public ServiceAccessToken UpdateToken(string accessToken) {
            ExecuteSql(
                "UPDATE service_access_tokens SET access_token = @access_token, updated_at = CURRENT_TIMESTAMP WHERE id = @id", 
                new {access_token = accessToken, id}
            );
            return this;
        }

        public ServiceAccessToken UpdateCol(string col, string val) {
            ExecuteSql(
                $"UPDATE service_access_tokens SET {col} = @val, updated_at = CURRENT_TIMESTAMP WHERE id = @id",
                new {val, id}
            );
            return this;
        }

        public static int Create(User.User user, string accessToken, ServiceType serviceType)
            => ExecuteScalarInt(
                $@"INSERT INTO service_access_tokens(guid, user_id, access_token, service_type) 
                VALUES (@guid, @user_id, @access_token, '{serviceType.ToString()}'); 
                SELECT currval('service_access_tokens_id_seq');"
                , new {
                    guid = Guid.NewGuid().ToString(), user_id = user.id, access_token = accessToken,
                    service_type = serviceType
                }
            );

        public ServiceAccessToken Refresh() => Find(id);
    }
}