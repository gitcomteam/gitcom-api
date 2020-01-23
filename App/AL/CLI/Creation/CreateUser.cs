using System;
using System.Collections.Generic;
using App.DL.Repository.User;
using App.PL.Transformer.User;
using Micron.AL.Config.CLI;
using Micron.DL.Module.CLI;
using Micron.DL.Module.Misc;

namespace App.AL.CLI.Creation {
    public class CreateUser : BaseCommand, ICliCommand {
        public override string Signature { get; } = "create-user";

        public CreateUser() {
            StrOutput = new List<string>();
        }

        public CliResult Execute() {
            Output("Type email:");
            var email = Console.ReadLine();
            var user = UserRepository.FindByEmail(email);
            if (user != null) {
                Output("User with this email already exists");
                return new CliResult(CliExitCode.UnknownError, StrOutput);
            }
            
            Output("Type login:");
            var login = Console.ReadLine();
            user = UserRepository.FindByLogin(login);
            if (user != null) {
                Output("User with this login already exists");
                return new CliResult(CliExitCode.UnknownError, StrOutput);
            }
            
            Output("Type password:");
            var password = Console.ReadLine();
            password = string.IsNullOrEmpty(password) ? Rand.RandomString() : password;
            Console.WriteLine($"New password: {password}");

            user = UserRepository.FindOrCreateByEmailAndLogin(email, login, password);
            
            Output("Created user:"); 
            Output(new UserTransformer().Transform(user).ToString());
            
            return new CliResult(CliExitCode.Ok, StrOutput);
        }
    }
}