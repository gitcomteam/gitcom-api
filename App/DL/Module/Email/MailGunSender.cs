using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using App.AL.Utils.Email;
using Micron.DL.Module.Config;

namespace App.DL.Module.Email {
    public static class MailGunSender {
        public static void QueueTemplate(string template, string to, string subject, KeyValuePair<string, string>[] parameters = null) {
            parameters ??= new KeyValuePair<string, string>[] {};
            
            Task.Run(() => {
                using (var client = new HttpClient()) {
                    var domain = AppConfig.GetConfiguration("auth:external:mailgun:domain");
                    var apiToken = AppConfig.GetConfiguration("auth:external:mailgun:api_token");

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                        "Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"api:{apiToken}"))
                    );

                    client.PostAsync(
                        $"https://api.mailgun.net/v3/{domain}/messages",
                        new FormUrlEncodedContent(
                            new List<KeyValuePair<string, string>> {
                                new KeyValuePair<string, string>("from", "GitCom Team <hi@gitcom.org>"),
                                new KeyValuePair<string, string>("to", to),
                                new KeyValuePair<string, string>("subject", subject),
                                new KeyValuePair<string, string>("template", template),
                            }.Concat(MailGunUtils.Prepare(parameters)))
                    ).Result.Content.ReadAsStringAsync();
                }
            });
        }
    }
}