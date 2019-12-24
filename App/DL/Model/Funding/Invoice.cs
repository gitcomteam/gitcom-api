// ReSharper disable InconsistentNaming

using System;
using System.Linq;
using App.DL.Enum;
using App.DL.Repository.Funding;
using App.DL.Repository.User;
using App.DL.Utils.String;
using Dapper;
using UserModel = App.DL.Model.User.User;

namespace App.DL.Model.Funding {
    public class Invoice : Micron.DL.Model.Model {
        public int id;

        public string guid;

        public int user_id;

        public int entity_id;

        public EntityType entity_type;

        public decimal amount;

        public CurrencyType currency_type;

        public InvoiceStatus status;

        public int wallet_id;

        public DateTime created_at;
        
        public DateTime updated_at;

        public UserModel User() => UserRepository.Find(user_id);
        
        public static Invoice Find(int id)
            => Connection().Query<Invoice>(
                "SELECT * FROM invoices WHERE id = @id LIMIT 1",
                new {id}
            ).FirstOrDefault();

        public static Invoice FindByGuid(string guid)
            => Connection().Query<Invoice>(
                "SELECT * FROM invoices WHERE guid = @guid LIMIT 1",
                new {guid}
            ).FirstOrDefault();
        
        public static int Create(
            UserModel user, int entityId, EntityType entityType, decimal amount, CurrencyType currencyType, InvoiceStatus status,
            CurrencyWallet wallet
        )
            => ExecuteScalarInt(
                $@"INSERT INTO invoices(guid, user_id, entity_id, entity_type, amount, currency_type, status, wallet_id, updated_at) 
                VALUES (@guid, @user_id, @entity_id, '{entityType.ToString()}', @amount, '{currencyType.ToString()}', '{status.ToString()}', @wallet_id, CURRENT_TIMESTAMP); 
                SELECT currval('invoices_id_seq');"
                , new {
                    guid = Guid.NewGuid().ToString(), user_id = user.id, entity_id = entityId, amount, wallet_id = wallet.id
                }
            );

        public Invoice Refresh() => Find(id);

        public Invoice UpdateStatus(InvoiceStatus newStatus) {
            ExecuteSql(
                $"UPDATE invoices SET status = '{newStatus.ToString()}', updated_at = CURRENT_TIMESTAMP WHERE id = @id", new {id}
            );
            return this;
        }

        public static Invoice[] GetActiveForUser(
            UserModel user, int limit = 10, int offset = 0
        )
            => GetForUserByStatuses(
                user, new[] {InvoiceStatus.Created, InvoiceStatus.RequiresConfirmation}, limit, offset
            );

        public static Invoice[] GetForUserByStatuses(
            UserModel user, InvoiceStatus[] enumStatuses, int limit = 10, int offset = 0
        ) 
            => Connection().Query<Invoice>(
                $@"SELECT * FROM invoices
                    WHERE user_id = @user_id 
                    AND status IN ({StringUtils.Implode(enumStatuses.Select(x => x.ToString()).ToArray(), ",", true)})
                    ORDER BY id DESC
                    OFFSET @offset LIMIT @limit",
                new {user_id = user.id, offset, limit}
            ).ToArray();
        
        public static int ActiveCount(UserModel user)
            => ExecuteScalarInt(
                $@"SELECT COUNT(*) FROM invoices
                    WHERE user_id = @user_id 
                    AND status IN ({StringUtils.Implode(new [] {
                    InvoiceStatus.Created.ToString(),
                    InvoiceStatus.RequiresConfirmation.ToString()
                }, ",", true)})",
                new {user_id = user.id}
            );
        
        public static int ActiveCount(UserModel user, int entityId, EntityType type)
            => ExecuteScalarInt(
                $@"SELECT COUNT(*) FROM invoices
                    WHERE user_id = @user_id AND entity_id = @entityId AND entity_type = '{type.ToString()}'
                    AND status IN ({StringUtils.Implode(new [] {
                    InvoiceStatus.Created.ToString(),
                    InvoiceStatus.RequiresConfirmation.ToString()
                }, ",", true)})",
                new {user_id = user.id, entityId, type}
            );
        
        public CurrencyWallet Wallet() => CurrencyWalletRepository.Find(wallet_id);
    }
}