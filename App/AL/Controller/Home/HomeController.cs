using System.Reflection;
using Micron.DL.Middleware;
using Micron.DL.Module.Controller;
using Micron.DL.Module.Http;
using Newtonsoft.Json.Linq;

namespace App.AL.Controller.Home {
    public sealed class HomeController : BaseController {
        protected override IMiddleware[] Middleware() => new IMiddleware[] {};

        public HomeController() {
            Get("/", _ => {
                return HttpResponse.Data(new JObject());
            });
            Get("/version", _ => {
                return HttpResponse.Data(new JObject() {
                    ["api_version"] = Assembly.GetExecutingAssembly().GetName().Version.ToString()
                });
            });
        }
    }
}