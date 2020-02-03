// ReSharper disable InconsistentNaming

using System;
using System.Linq;
using App.DL.Enum;
using Dapper;
using UserModel = App.DL.Model.User.User;

namespace App.DL.Model.Funding {
    public class FundingTransaction : Micron.DL.Model.Model {
        public int id;

        public string guid;

        public int from_user_id;

        public int entity_id;

        public EntityType entity_type;

        public CurrencyType currency_type;

        public decimal amount;

        public DateTime created_at;

        public UserModel FromUser() => UserModel.Find(from_user_id);

        public static FundingTransaction Find(int id)
            => Connection().Query<FundingTransaction>(
                "SELECT * FROM funding_transactions WHERE id = @id LIMIT 1",
                new {id}
            ).FirstOrDefault();

        public static FundingTransaction FindByGuid(string guid)
            => Connection().Query<FundingTransaction>(
                "SELECT * FROM funding_transactions WHERE guid = @guid LIMIT 1",
                new {guid}
            ).FirstOrDefault();

        public static FundingTransaction Find(Invoice invoice)
            => Connection().Query<FundingTransaction>(
                "SELECT * FROM funding_transactions WHERE invoice_id = @invoice_id LIMIT 1",
                new {invoice_id = invoice.id}
            ).FirstOrDefault();

        public static FundingTransaction Find(UserModel user, Invoice invoice, int entityId, EntityType entityType)
            => Connection().Query<FundingTransaction>(
                $@"SELECT * FROM funding_transactions 
                    WHERE from_user_id = @from_user_id AND invoice_id = @invoice_id AND entity_id = @entity_id
                    AND entity_type = '{entityType.ToString()}'
                    LIMIT 1",
                new {
                    from_user_id = user.id,
                    invoice_id = invoice.id,
                    entity_id = entityId
                }
            ).FirstOrDefault();

        public static FundingTransaction[] Get(
            int entityId, EntityType entityType, CurrencyType currencyType, int limit = 10
        )
            => Connection().Query<FundingTransaction>(
                $@"SELECT * FROM funding_transactions
                WHERE entity_id = @entity_id AND entity_type = '{entityType.ToString()}' 
                AND currency_type = '{currencyType.ToString()}' LIMIT @limit",
                new {entity_id = entityId, limit}
            ).ToArray();

        public static FundingTransaction[] Get(
            UserModel user, int limit = 10
        )
            => Connection().Query<FundingTransaction>(
                @"SELECT * FROM funding_transactions
                WHERE from_user_id = @from_user_id LIMIT @limit",
                new {from_user_id = user.id, limit}
            ).ToArray();

        public static int Create(
            UserModel from, int entityId, EntityType entityType, Invoice invoice, decimal amount,
            CurrencyType currencyType, string note = null
        ) {
            return ExecuteScalarInt(
                $@"INSERT INTO public.funding_transactions(from_user_id, guid, entity_id, entity_type, invoice_id, 
                        amount, currency_type, note)
                    VALUES (@from_user_id, @guid, @entityId, '{entityType.ToString()}', @invoice_id, @amount, 
                        '{currencyType.ToString()}', @note);
                    SELECT currval('funding_transactions_id_seq');"
                , new {
                    from_user_id = from.id, guid = Guid.NewGuid().ToString(), entityId,
                    invoice_id = invoice != null ? invoice.id : (int?) null, amount, note
                }
            );
        }

        public static FundingTransaction[] GetLatest(
            int entityId, EntityType entityType, int limit = 10, int offset = 0
        )
            => Connection().Query<FundingTransaction>(
                $@"SELECT * FROM funding_transactions 
                    WHERE entity_id = @entity_id AND entity_type = '{entityType.ToString()}'
                    ORDER BY id DESC
                    OFFSET @offset LIMIT @limit",
                new {entity_id = entityId, offset, limit}
            ).ToArray();
    }
}