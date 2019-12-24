using System.Linq;
using Micron.DL.Module.Db;
using Dapper;

namespace Tests.Utils.DB {
    public class DbCleaner {
        public static string tablesToCleanUp = "";
        
        public static void TruncateAll() {
            var connection = DbConnection.RootConnection();
            
            if (tablesToCleanUp == "") {
                var tables = connection.Query<string>(
                    "SELECT table_name FROM information_schema.tables WHERE table_schema='public'"
                ).ToList();

                tables.RemoveAll(x => x == "phinxlog");

                var listOfTables = "";

                foreach (var table in tables) {
                    listOfTables += table + ",";
                }

                tablesToCleanUp = listOfTables.Substring(0, listOfTables.Length - 1);
            }

            connection.Execute("TRUNCATE " + tablesToCleanUp + ";");
        }
    }
}