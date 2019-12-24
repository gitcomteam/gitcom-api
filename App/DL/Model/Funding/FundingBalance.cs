
// ReSharper disable InconsistentNaming

using System;
using System.Linq;
using App.DL.Enum;
using App.DL.Repository.User;
using Dapper;
using UserModel = App.DL.Model.User.User;

namespace App.DL.Model.Funding {
    public class FundingBalance : Micron.DL.Model.Model {
        public int id;

        public string guid;

        public int entity_id;

        public EntityType entity_type;

        public CurrencyType currency_type;

        public decimal balance;

        public DateTime created_at;

        public DateTime updated_at;

        public static FundingBalance Find(int id)
            => Connection().Query<FundingBalance>(
                "SELECT * FROM funding_balances WHERE id = @id LIMIT 1",
                new {id}
            ).FirstOrDefault();

        public static FundingBalance FindByGuid(string guid)
            => Connection().Query<FundingBalance>(
                "SELECT * FROM funding_balances WHERE guid = @guid LIMIT 1",
                new {guid}
            ).FirstOrDefault();

        public static FundingBalance FindBy(string col, int val)
            => Connection().Query<FundingBalance>(
                $"SELECT * FROM funding_balances WHERE {col} = @val LIMIT 1",
                new {val}
            ).FirstOrDefault();

        public static FundingBalance Find(int entityId, EntityType entityType, CurrencyType currencyType)
            => Connection().Query<FundingBalance>(
                $@"SELECT * FROM funding_balances 
                WHERE entity_id = @entity_id AND entity_type = '{entityType.ToString()}' 
                AND currency_type = '{currencyType.ToString()}' LIMIT 1",
                new {entity_id = entityId}
            ).FirstOrDefault();

        public FundingBalance Refresh() => Find(id);

        public static int Create(int entityId, EntityType entityType, CurrencyType currencyType)
            => ExecuteScalarInt(
                $@"INSERT INTO funding_balances(guid, entity_id, entity_type, currency_type) 
                VALUES (@guid, @entity_id, '{entityType.ToString()}', '{currencyType.ToString()}'); 
                SELECT currval('funding_balances_id_seq');"
                , new {
                    guid = Guid.NewGuid().ToString(), entity_id = entityId
                }
            );

        public static FundingBalance[] Get(int entityId, EntityType type, int limit = 10)
            => Connection().Query<FundingBalance>(
                $@"SELECT * FROM funding_balances WHERE entity_id = @entity_id AND entity_type = '{type.ToString()}'
                     LIMIT @limit",
                new {entity_id = entityId, limit}
            ).ToArray();

        public FundingBalance UpdateBalance(decimal diff) {
            balance += diff;
            ExecuteSql(
                @"UPDATE funding_balances SET balance = @balance, updated_at = CURRENT_TIMESTAMP " +
                "WHERE id = @id", new {balance, id}
            );
            return this;
        }
    }
}