using Nancy;

namespace App.AL.Middleware {
    public class AddHeaderMiddleware {
        public NancyContext Process(NancyContext ctx, string header, string value) {
            ctx.Response.Headers.Add(header, value);
            return ctx;
        }
    }
}