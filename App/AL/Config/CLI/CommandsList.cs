using App.AL.CLI.Creation;
using App.AL.CLI.Funding;
using App.AL.CLI.SystemStats;
using Micron.AL.CLI.Basic;
using Micron.DL.Module.CLI;

namespace App.AL.Config.CLI {
    public static class CommandsList {
        public static ICliCommand[] Get()
            => new ICliCommand[] {
                new PrintFrameworkVersion(),
                new ApproveInvoice(),
                new DbTablesCount(),
                new CreateProject(),
                new CreateUser(),
            };
    }
}