
// ReSharper disable InconsistentNaming

using System;
using System.Linq;
using App.DL.Enum;
using Dapper;

namespace App.DL.Model.External {
    public class ExternalServiceData : Micron.DL.Model.Model {
        public int id;

        public string guid;

        public int user_id;

        public string origin_id;
        
        public string login;

        public ServiceType service_type;

        public DateTime created_at;

        public DateTime updated_at;

        public static ExternalServiceData Find(int id)
            => Connection().Query<ExternalServiceData>(
                "SELECT * FROM external_services_data WHERE id = @id LIMIT 1",
                new {id}
            ).FirstOrDefault();

        public static ExternalServiceData FindByGuid(string guid)
            => Connection().Query<ExternalServiceData>(
                "SELECT * FROM external_services_data WHERE guid = @guid LIMIT 1",
                new {guid}
            ).FirstOrDefault();

        public static ExternalServiceData Find(User.User user, ServiceType serviceType)
            => Connection().Query<ExternalServiceData>(
                $@"SELECT * FROM external_services_data 
                        WHERE user_id = @user_id AND service_type = '{serviceType.ToString()}' LIMIT 1",
                new {user_id = user.id}
            ).FirstOrDefault();

        public ExternalServiceData Refresh() => Find(id);

        public static int Create(User.User user, ServiceType serviceType, string origin_id, string login)
            => ExecuteScalarInt(
                $@"INSERT INTO external_services_data(guid, user_id, service_type, origin_id, login) 
                        VALUES (@guid, @user_id, '{serviceType.ToString()}', @origin_id, @login); 
                        SELECT currval('external_services_data_id_seq');"
                , new {
                    guid = Guid.NewGuid().ToString(), user_id = user.id, origin_id, login
                }
            );
    }
}