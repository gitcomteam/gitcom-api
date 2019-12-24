using Micron.DL.Module.Config;
using Nancy;

namespace App.AL.Middleware {
    public class CorsMiddleware {
        public NancyContext Process(NancyContext ctx) {
            ctx.Response.Headers.Add("Access-Control-Allow-Origin", AppConfig.GetConfiguration("server:headers:allow_origin") ?? "*");
            ctx.Response.Headers.Add("Access-Control-Allow-Methods", "POST, GET, OPTIONS, DELETE, PUT, PATCH");
            ctx.Response.Headers.Add("Access-Control-Allow-Headers", AppConfig.GetConfiguration("server:headers:allow_headers") ?? "*");
            return ctx;
        }
    }
}