using System.Linq;
using App.AL.Config.Funding;
using App.AL.Utils.Entity;
using App.AL.Validation.Entity;
using App.AL.Validation.Funding;
using App.DL.Enum;
using App.DL.Repository.Funding;
using App.DL.Repository.User;
using App.PL.Transformer.Funding;
using Micron.AL.Validation.Basic;
using Micron.AL.Validation.Db;
using Micron.AL.Validation.String;
using Micron.DL.Middleware;
using Micron.DL.Middleware.Auth;
using Micron.DL.Module.Controller;
using Micron.DL.Module.Http;
using Micron.DL.Module.Validator;
using Nancy;

namespace App.AL.Controller.Funding.Invoice {
    public sealed class InvoiceController : BaseController {
        protected override IMiddleware[] Middleware() => new IMiddleware[] { new JwtMiddleware() };

        public InvoiceController() {
            Post("/api/v1/invoice/new", _ => {
                var me = DL.Model.User.User.Find(CurrentRequest.UserId);

                var errors = ValidationProcessor.Process(Request, new IValidatorRule[] {
                    new ShouldHaveParameters(new[] {"entity_guid", "entity_type", "amount", "currency_type"}),
                    new ShouldBeCorrectEnumValue("entity_type", typeof(EntityType)),
                    new ShouldBeCorrectEnumValue("currency_type", typeof(CurrencyType)),
                    new EntityShouldExist(),
                    new UserActiveInvoicesLimit(me, InvoiceConfig.UserActiveInvoicesLimit),
                }, true);
                if (errors.Count > 0) return HttpResponse.Errors(errors);
                
                var entityType = (EntityType) GetRequestEnum("entity_type", typeof(EntityType));

                var currencyType = (CurrencyType) GetRequestEnum("currency_type", typeof(CurrencyType));

                var wallet = CurrencyWalletRepository.FindRandom(currencyType);
                
                var invoice = InvoiceRepository.Create(
                    me,
                    EntityUtils.GetEntityId(GetRequestStr("entity_guid"), entityType),
                    entityType,
                    (decimal) Request.Query["amount"],
                    currencyType,
                    InvoiceStatus.Created,
                    wallet
                );

                return HttpResponse.Item(
                    "invoice", new InvoiceTransformer().Transform(invoice), HttpStatusCode.Created
                );
            });

            Get("/api/v1/me/invoice/get", _ => {
                var me = UserRepository.Find(CurrentRequest.UserId);

                var errors = ValidationProcessor.Process(Request, new IValidatorRule[] {
                    new ShouldHaveParameters(new[] {"invoice_guid"}),
                    new ExistsInTable("invoice_guid", "invoices", "guid"),
                    new StringShouldBeSameInDb(
                        "invoice_guid", "invoices", "guid", "user_id", me.id.ToString()
                    )
                }, true);
                if (errors.Count > 0) return HttpResponse.Errors(errors);
                
                return HttpResponse.Item("invoice", new InvoiceTransformer().Transform(
                    InvoiceRepository.FindByGuid(GetRequestStr("invoice_guid"))
                ));
            });
            
            Get("/api/v1/me/invoices/finished", _ => {
                var me = UserRepository.Find(CurrentRequest.UserId);

                var invoices = DL.Model.Funding.Invoice.GetForUserByStatuses(me, new [] {
                    InvoiceStatus.Confirmed, InvoiceStatus.Failed, InvoiceStatus.Done
                });
                
                return HttpResponse.Item("invoices", new InvoiceTransformer().Many(invoices));
            });
            
            Get("/api/v1/me/invoices/active", _ => {
                var me = UserRepository.Find(CurrentRequest.UserId);

                var invoices = DL.Model.Funding.Invoice.GetActiveForUser(me, 25);
                
                return HttpResponse.Item("invoices", new InvoiceTransformer().Many(invoices));
            });

            Patch("/api/v1/me/invoice/status/update", _ => {
                var me = UserRepository.Find(CurrentRequest.UserId);
                
                var errors = ValidationProcessor.Process(Request, new IValidatorRule[] {
                    new ShouldHaveParameters(new[] {"invoice_guid", "status"}),
                    new ExistsInTable("invoice_guid", "invoices", "guid"),
                    new ShouldBeCorrectEnumValue("status", typeof(InvoiceStatus)),
                    new StringShouldBeSameInDb(
                        "invoice_guid", "invoices", "guid", "user_id", me.id.ToString()
                    )
                }, true);
                if (errors.Count > 0) return HttpResponse.Errors(errors);
                
                var newStatus = (InvoiceStatus) GetRequestEnum("status", typeof(InvoiceStatus));
                
                var invoice = InvoiceRepository.FindByGuid(GetRequestStr("invoice_guid"));
                
                if (invoice.status != InvoiceStatus.Created) {
                    return HttpResponse.Error(new HttpError(HttpStatusCode.Forbidden, "Cannot update invoice with this status"));
                }

                var availableStatuses = new[] {InvoiceStatus.Failed, InvoiceStatus.RequiresConfirmation};
                
                if (!availableStatuses.Contains(newStatus)) {
                    return HttpResponse.Error(new HttpError(HttpStatusCode.Forbidden, "This status is not allowed"));
                }

                InvoiceRepository.UpdateStatus(invoice, newStatus);

                return HttpResponse.Item("invoice", new InvoiceTransformer().Transform(invoice.Refresh()));
            });
        }
    }
}