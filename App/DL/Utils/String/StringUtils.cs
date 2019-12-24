namespace App.DL.Utils.String {
    public static class StringUtils {
        public static string Implode(string[] strings, string delimiter, bool inQuotes = false) {
            var result = "";

            if (strings.Length == 1) {
                return strings[0];
            }
            
            foreach (var str in strings) {
                result += (inQuotes ? $"'{str}'" : str) + delimiter;
            }
            
            return result.Substring(0, result.Length - delimiter.Length);
        }
    }
}