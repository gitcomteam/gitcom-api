using System;
using System.Collections.Generic;
using App.AL.Utils.Funding;
using App.DL.Enum;
using App.DL.Model.Funding;
using App.DL.Repository.User;
using App.PL.Transformer.Funding;
using App.PL.Transformer.User;
using Micron.AL.Config.CLI;
using Micron.DL.Module.CLI;
using Newtonsoft.Json.Linq;

namespace App.AL.CLI.Finding {
    public class ApproveInvoice : BaseCommand, ICliCommand {
        public override string Signature { get; } = "approve-invoice";

        public ApproveInvoice() {
            StrOutput = new List<string>();
        }

        public CliResult Execute() {
            Output("Approving invoice");
            Output("Type invoice id:");
            
            var id = Convert.ToInt32(Console.ReadLine());
            var invoice = Invoice.Find(id);
            if (invoice == null) {
                Output($"Invoice with id {id} not found");
                return new CliResult(CliExitCode.NotFound, StrOutput);
            }
            
            Output("Invoice:");
            Output(JObject.FromObject(invoice).ToString());
            Output("Transformed invoice:");
            Output(new InvoiceTransformer().Transform(invoice).ToString());
            Output("Invoice user:");
            Output(JObject.FromObject(invoice.User()).ToString());

            if (invoice.status != InvoiceStatus.RequiresConfirmation) {
                Output("Invoice has invalid status - allowed only: " + InvoiceStatus.RequiresConfirmation);
                return new CliResult(CliExitCode.UnknownError, StrOutput);
            } 

            var isApproving = Ask("Approve that invoice?");
            
            if (!isApproving) {
                Output("Aborted.");
                return new CliResult(CliExitCode.Ok, StrOutput);
            }
            
            Output("Approving invoice...");

            invoice = InvoiceUtils.ConfirmInvoice(invoice);
            
            var isProcessing = Ask("Process confirmed invoice?");

            if (!isProcessing) {
                return new CliResult(CliExitCode.Ok, StrOutput);
            }

            var balance = UserBalanceRepository.Find(invoice.User());
            
            Output("Balance before:");
            Output(new UserBalanceTransformer().Transform(balance).ToString());
            
            Output("Processing invoice...");
            invoice = InvoiceUtils.ProcessConfirmedInvoice(invoice);
            Output("Invoice processing finished...");
            
            Output("Balance after:");
            Output(new UserBalanceTransformer().Transform(balance.Refresh()).ToString());
            
            Output("Transformed invoice:");
            Output(new InvoiceTransformer().Transform(invoice).ToString());

            return new CliResult(CliExitCode.Ok, StrOutput);
        }
    }
}