using System.Globalization;
using App.DL.Enum;
using Micron.DL.Module.Auth;
using Micron.DL.Module.Misc;
using Nancy;
using Nancy.Testing;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Tests.Testing;
using Tests.Utils.Fake.Funding;
using Tests.Utils.Fake.Project;
using Tests.Utils.Fake.User;

namespace Tests.App.AL.Controller.Funding.Invoice {
    [TestFixture]
    public class InvoiceControllerTests : BaseTestFixture {
        [Test]
        public void Create_DataCorrect_InvoiceCreated() {
            var user = UserFaker.Create();

            var project = ProjectFaker.Create();

            var amount = Rand.SmallDecimal();

            CurrencyWalletFaker.Create(CurrencyType.BitCoin);
            
            var result = new Browser(new DefaultNancyBootstrapper())
                .Post("/api/v1/invoice/new", with => {
                    with.HttpRequest();
                    with.Query("api_token", Jwt.FromUserId(user.id));
                    with.Query("entity_guid", project.guid);
                    with.Query("entity_type", EntityType.Project.ToString());
                    with.Query("amount", amount.ToString(CultureInfo.InvariantCulture));
                    with.Query("currency_type", CurrencyType.BitCoin.ToString());
                }).Result;

            Assert.AreEqual(1, global::App.DL.Model.Funding.Invoice.GetActiveForUser(user).Length);
            Assert.AreEqual(HttpStatusCode.Created,result.StatusCode);
            
            var jsonData = JObject.Parse(result.Body.AsString())["data"]["invoice"];
            
            Assert.AreEqual(project.guid, jsonData.Value<string>("entity_guid") ?? "");
            Assert.AreEqual(amount, jsonData.Value<decimal?>("amount") ?? 0);
        }
        
        [Test]
        public void GetActive_DataCorrect_GotInvoices() {
            var user = UserFaker.Create();
            
            InvoiceFaker.Create(user);

            var result = new Browser(new DefaultNancyBootstrapper())
                .Get("/api/v1/me/invoices/active", with => {
                    with.HttpRequest();
                    with.Query("api_token", Jwt.FromUserId(user.id));
                }).Result;

            Assert.AreEqual(1, global::App.DL.Model.Funding.Invoice.GetActiveForUser(user).Length);
            Assert.AreEqual(HttpStatusCode.OK,result.StatusCode);
        }
        
        [Test]
        public void Edit_ChangingStatus_StatusUpdated() {
            var user = UserFaker.Create();

            var invoice = InvoiceFaker.Create(user);

            Assert.AreEqual(InvoiceStatus.Created,invoice.status);
            
            var result = new Browser(new DefaultNancyBootstrapper())
                .Patch("/api/v1/me/invoice/status/update", with => {
                    with.HttpRequest();
                    with.Query("api_token", Jwt.FromUserId(user.id));
                    with.Query("invoice_guid", invoice.guid);
                    with.Query("status", InvoiceStatus.RequiresConfirmation.ToString());
                }).Result;
            
            Assert.AreEqual(HttpStatusCode.OK,result.StatusCode);
            Assert.AreEqual(InvoiceStatus.RequiresConfirmation,invoice.Refresh().status);
        }
        
        [Test]
        public void Edit_ChangingStatusOfOtherUsersInvoice_Forbidden() {
            var user = UserFaker.Create();

            var invoice = InvoiceFaker.Create();

            Assert.AreEqual(InvoiceStatus.Created,invoice.status);
            
            var result = new Browser(new DefaultNancyBootstrapper())
                .Patch("/api/v1/me/invoice/status/update", with => {
                    with.HttpRequest();
                    with.Query("api_token", Jwt.FromUserId(user.id));
                    with.Query("invoice_guid", invoice.guid);
                    with.Query("status", InvoiceStatus.RequiresConfirmation.ToString());
                }).Result;
            
            Assert.AreEqual(HttpStatusCode.Forbidden,result.StatusCode);
            Assert.AreEqual(InvoiceStatus.Created,invoice.Refresh().status);
        }
    }
}