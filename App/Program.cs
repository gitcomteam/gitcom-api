using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using App.AL;
using App.AL.Config.CLI;
using App.AL.Middleware;
using Micron.DL.Module.CLI;
using Micron.DL.Module.Config;
using Micron.DL.Module.Http;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Configuration;
using Nancy.Hosting.Self;
using Nancy.TinyIoc;
using Sentry;

namespace App {
    public class Bootstrapper : DefaultNancyBootstrapper {
        public override void Configure(INancyEnvironment environment) {
            
            var config = new TraceConfiguration(enabled: false, displayErrorTraces: true);
            environment.AddValue(config);
        }
        
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines) {
            pipelines.AfterRequest += (ctx) => {
                ctx = new CorsMiddleware().Process(ctx);
                ctx = new AddHeaderMiddleware().Process(ctx, "Content-Type", "application/json");
            };
            pipelines.OnError += (context, exception) => HandleException(context, exception);
        }
        
        private static Response HandleException(NancyContext context, Exception exception) {
            Console.WriteLine(exception.Message);
            Console.WriteLine(exception.StackTrace);

            try {
                exception.Data.Add("request_url", context.Request.Url.ToString());
                
                Program.logWriter.WriteLine(DateTime.Now.ToString(CultureInfo.InvariantCulture));
                Program.logWriter.WriteLine(exception);
                Program.logWriter.WriteLine(exception.StackTrace);
                Program.logWriter.WriteLine("----------");
                
                SentrySdk.CaptureException(exception);
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }

            return HttpResponse.Error(new HttpError(HttpStatusCode.InternalServerError, "Unknown error"));
        }
    }

    class Program {
        public static StreamWriter logWriter = File.AppendText("error.log");
        
        static void Main() {
            var sentryKey = AppConfig.GetConfiguration("external:sentry:key");
            var sentryId = AppConfig.GetConfiguration("external:sentry:id");
            
            var hostUri = AppConfig.GetConfiguration("server:host_uri") ?? "http://localhost:8000";
            
            using (SentrySdk.Init($"https://{sentryKey}@sentry.io/{sentryId}")) {
                var host = new NancyHost(new Bootstrapper(), new Uri(hostUri));

                AppBoot.Boot();

                Console.WriteLine("Server version: " + Assembly.GetExecutingAssembly().GetName().Version);
                Console.WriteLine("Starting server");
                Console.WriteLine("Starting server...");
                host.Start();
                Console.WriteLine("Server started");

                Cli.CliLoop(CommandsList.Get());
            }
        }
    }
}