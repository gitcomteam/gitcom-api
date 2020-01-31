using Micron.DL.Middleware;
using Micron.DL.Module.Config;
using Micron.DL.Module.Http;
using Nancy;

namespace App.AL.Middleware.Schedule {
    public class ScheduleAuth : IMiddleware {
        public ProcessedRequest Process(ProcessedRequest request) {
            var scheduleToken = AppConfig.GetConfiguration("auth:schedule:token");

            if (
                string.IsNullOrEmpty(scheduleToken) || scheduleToken != request.GetRequestStr("schedule_token")
            ) {
                request.AddError(new HttpError(HttpStatusCode.Unauthorized, "Schedule token is invalid"));
            }

            return request;
        }
    }
}