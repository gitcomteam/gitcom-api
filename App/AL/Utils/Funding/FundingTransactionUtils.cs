using App.DL.Model.Funding;
using App.DL.Repository.User;

namespace App.AL.Utils.Funding {
    public static class FundingTransactionUtils {
        public static void CreateTxFromInvoice(Invoice invoice) {
            FundingTransaction.Create(
                UserRepository.Find(invoice.user_id),
                invoice.entity_id,
                invoice.entity_type,
                invoice,
                invoice.amount,
                invoice.currency_type
            );
        }
    }
}