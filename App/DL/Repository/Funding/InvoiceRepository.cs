using App.AL.Utils.External.Discord;
using App.DL.Enum;
using App.DL.Model.Funding;
using App.DL.Repository.User;
using UserModel = App.DL.Model.User.User;

namespace App.DL.Repository.Funding {
    public static class InvoiceRepository {
        public static Invoice Find(int id) => Invoice.Find(id);
        
        public static Invoice FindByGuid(string guid) => Invoice.FindByGuid(guid);
        
        public static Invoice Create(
            UserModel user, int entityId, EntityType entityType, decimal amount, CurrencyType currencyType,
            InvoiceStatus status, CurrencyWallet wallet
        ) {
            switch (entityType) {
                case EntityType.UserBalance:
                    UserBalanceRepository.FindOrCreate(user, currencyType);
                    break;
            }
            var invoice = Find(Invoice.Create(user, entityId, entityType, amount, currencyType, status, wallet));
            DiscordWebhooks.SendEvent("system-events", $"New invoice #{invoice.id} was created for user: {invoice.user_id}");
            return invoice;
        }

        public static Invoice UpdateStatus(Invoice invoice, InvoiceStatus status) {
            return invoice.UpdateStatus(status);
        }
    }
}