using System;
using App.AL.Utils.Entity;
using App.DL.Enum;
using App.DL.Model.Funding;
using App.DL.Model.User;
using App.DL.Repository.User;

namespace App.AL.Utils.Funding {
    public static class InvoiceUtils {
        public static Invoice ClientConfirmInvoicePaid(Invoice invoice) {
            return invoice.UpdateStatus(InvoiceStatus.RequiresConfirmation);
        }

        public static Invoice CancelInvoice(Invoice invoice) {
            return invoice.UpdateStatus(InvoiceStatus.Failed);
        }

        public static bool ActiveInvoiceExists(User user, int entityId, EntityType type)
            => Invoice.ActiveCount(user, entityId, type) > 0;

        // TODO: add confirmed by
        public static Invoice ConfirmInvoice(Invoice invoice) {
            if (invoice.status != InvoiceStatus.RequiresConfirmation) {
                return invoice;
            }

            invoice.UpdateStatus(InvoiceStatus.Confirmed);
            return invoice.Refresh();
        }

        public static Invoice ProcessConfirmedInvoice(Invoice invoice) {
            if (invoice.entity_type == EntityType.UserBalance) {
                UserBalanceRepository.FindOrCreate(invoice);
            }

            if (
                !EntityUtils.IsEntityExists(invoice.entity_id, invoice.entity_type) ||
                invoice.status != InvoiceStatus.Confirmed
            ) {
                throw new Exception("Entity entity not exists or has invalid status");
            }

            switch (invoice.entity_type) {
                case EntityType.UserBalance:
                    var user = invoice.User();
                    if (user == null) {
                        throw new Exception("Invoice user not exists");
                    }

                    UserBalanceUtils.Deposit(user, invoice);
                    break;
                default:
                    FundingBalanceUtils.FundEntity(invoice);
                    break;
            }

            invoice.UpdateStatus(InvoiceStatus.Done);
            return invoice.Refresh();
        }
    }
}