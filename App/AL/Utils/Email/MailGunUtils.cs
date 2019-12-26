using System.Collections.Generic;
using System.Linq;

namespace App.AL.Utils.Email {
    public static class MailGunUtils {
        public static KeyValuePair<string, string>[] Prepare(KeyValuePair<string, string>[] parameters) {
            return parameters.Select(c => {
                return new KeyValuePair<string,string>($"v:{c.Key}", c.Value);
            }).ToArray();
        }
    }
}