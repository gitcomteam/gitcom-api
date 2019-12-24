using System.Collections.Generic;

namespace App.AL.Config.UserSetting {
    public static class AllowedValues {
        // equal null means that all values are allowed
        public static Dictionary<string, string[]> GetAllowed()
            => new Dictionary<string, string[]> {
                ["subscription_currency"] = new[] {
                    "Usd", "BitCoin", "Ethereum", "Erc20Token", "Waves", "WavesToken", "LiteCoin"
                },
                ["subscription_amount"] = null
            };
    }
}