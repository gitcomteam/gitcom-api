using System.Collections.Generic;
using Micron.AL.Config.CLI;
using Micron.DL.Module.CLI;

namespace App.AL.CLI.Repo {
    public class ImportRepo : BaseCommand, ICliCommand {
        public override string Signature { get; } = "import-repo";

        public ImportRepo() {
            StrOutput = new List<string>();
        }

        public CliResult Execute() {
            Output("Test");

            return new CliResult(CliExitCode.Ok, StrOutput);
        }
    }
}