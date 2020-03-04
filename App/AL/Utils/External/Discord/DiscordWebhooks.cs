using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Micron.DL.Module.Config;

namespace App.AL.Utils.External.Discord {
    public static class DiscordWebhooks {
        public static void SendEvent(string type, string content) {
            Task.Run(() => {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Accept", "application/json");

                var webhookUrl = AppConfig.GetConfiguration($"external:discord:webhook_tokens:{type}");

                if (string.IsNullOrEmpty(webhookUrl)) return;
                
                var response = client.PostAsync(
                    webhookUrl,
                    new FormUrlEncodedContent(new[] {
                        new KeyValuePair<string, string>("content", content),
                    })
                ).Result;
            });
        }
    }
}