using System;
using App.AL.Utils.Entity;
using App.AL.Validation.Permission;
using App.DL.Enum;
using App.DL.Repository.Product;
using App.DL.Repository.Project;
using App.DL.Repository.User;
using App.PL.Transformer.Product;
using Micron.AL.Validation.Basic;
using Micron.AL.Validation.Db;
using Micron.DL.Middleware;
using Micron.DL.Middleware.Auth;
using Micron.DL.Module.Controller;
using Micron.DL.Module.Http;
using Micron.DL.Module.Validator;
using Nancy;

namespace App.AL.Controller.Product {
    public class ProjectProductCrudController : BaseController {
        protected override IMiddleware[] Middleware() => new IMiddleware[] {
            new JwtMiddleware()
        };

        public ProjectProductCrudController() {
            Post("/api/v1/project/product/create", _ => {
                var projectGuid = GetRequestStr("project_guid");

                var me = UserRepository.Find(CurrentRequest.UserId);
                var errors = ValidationProcessor.Process(Request, new IValidatorRule[] {
                    new ShouldHaveParameters(new[] {"project_guid", "name", "description"}),
                    new ExistsInTable("project_guid", "projects", "guid"),
                    new HasPermission(me, EntityUtils.GetEntityId(projectGuid, EntityType.Project), EntityType.Project)
                });
                if (errors.Count > 0) return HttpResponse.Errors(errors);

                var project = ProjectRepository.FindByGuid(GetRequestStr("project_guid"));

                var product = ProjectProductRepository.Create(
                    project, GetRequestStr("name"),
                    GetRequestStr("description"),
                    GetRequestStr("url"),
                    GetRequestStr("use_url")
                );
                
                if (!String.IsNullOrEmpty(GetRequestStr("usd_price"))) {
                    var price = Convert.ToDecimal(GetRequestStr("usd_price"));
                    int priceInPennies = (int) (price * 100);

                    product.UpdateCol("usd_price_pennies", priceInPennies);
                }

                if (!String.IsNullOrEmpty(GetRequestStr("duration_hours"))) {
                    product.UpdateCol("duration_hours", GetRequestInt("duration_hours"));
                }

                return HttpResponse.Item(
                    "product", new ProjectProductTransformer().Transform(product.Refresh()), HttpStatusCode.Created
                );
            });

            Patch("/api/v1/project/product/edit", _ => {
                var me = UserRepository.Find(CurrentRequest.UserId);

                var product = ProjectProductRepository.FindByGuid(GetRequestStr("product_guid"));

                var errors = ValidationProcessor.Process(Request, new IValidatorRule[] {
                    new ShouldHaveParameters(new[] {"product_guid"}),
                    new ExistsInTable("product_guid", "project_products", "guid"),
                    new HasPermission(me, product.project_id, EntityType.Project)
                }, true);
                if (errors.Count > 0) return HttpResponse.Errors(errors);

                if (!String.IsNullOrEmpty(GetRequestStr("name"))) {
                    product.UpdateCol("name", GetRequestStr("name"));
                }

                if (!String.IsNullOrEmpty(GetRequestStr("description"))) {
                    product.UpdateCol("description", GetRequestStr("description"));
                }

                if (!String.IsNullOrEmpty(GetRequestStr("url"))) {
                    product.UpdateCol("url", GetRequestStr("url"));
                }

                if (!String.IsNullOrEmpty(GetRequestStr("set_url"))) {
                    product.UpdateCol("set_url", GetRequestStr("set_url"));
                }
                
                if (!String.IsNullOrEmpty(GetRequestStr("usd_price"))) {
                    var price = Convert.ToDecimal(GetRequestStr("usd_price"));
                    int priceInPennies = (int) (price * 100);

                    product.UpdateCol("usd_price_pennies", priceInPennies);
                }

                if (!String.IsNullOrEmpty(GetRequestStr("duration_hours"))) {
                    product.UpdateCol("duration_hours", GetRequestInt("duration_hours"));
                }

                return HttpResponse.Item("product", new ProjectProductTransformer().Transform(product.Refresh()));
            });

            Delete("/api/v1/project/product/delete", _ => {
                var me = UserRepository.Find(CurrentRequest.UserId);

                var product = ProjectProductRepository.FindByGuid(GetRequestStr("product_guid"));

                var errors = ValidationProcessor.Process(Request, new IValidatorRule[] {
                    new ShouldHaveParameters(new[] {"product_guid"}),
                    new ExistsInTable("product_guid", "project_products", "guid"),
                    new HasPermission(me, product?.project_id ?? 0, EntityType.Project)
                }, true);
                if (errors.Count > 0) return HttpResponse.Errors(errors);
                
                product?.Delete();
                
                return HttpResponse.Item("product", new ProjectProductTransformer().Transform(product));
            });
        }
    }
}