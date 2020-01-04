using System.Collections.Generic;
using System.Linq;
using Dapper;
using Micron.AL.Config.CLI;
using Micron.DL.Module.CLI;
using Micron.DL.Module.Db;

namespace App.AL.CLI.SystemStats {
    public class DbTablesCount : BaseCommand, ICliCommand {
        public override string Signature { get; } = "db-tables-count";

        public DbTablesCount() {
            StrOutput = new List<string>();
        }

        public CliResult Execute() {
            var tables = DbConnection.Connection().Query<string>(
                "SELECT table_name FROM information_schema.tables WHERE table_schema='public'"
            ).ToList();

            foreach (var table in tables) {
                var count = DbConnection.Connection().ExecuteScalar<int>($"SELECT COUNT(*) FROM {table}");
                Output($"records: {count}    table: {table}");
            }

            Output("Finished");

            return new CliResult(CliExitCode.Ok, StrOutput);
        }
    }
}