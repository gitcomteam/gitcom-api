using System;
using System.Collections.Generic;
using App.DL.Enum;
using App.DL.Repository.Project;
using App.DL.Repository.Repo;
using App.DL.Repository.User;
using App.PL.Transformer.Project;
using Micron.AL.Config.CLI;
using Micron.DL.Module.CLI;

namespace App.AL.CLI.Creation {
    public class CreateProject : BaseCommand, ICliCommand {
        public override string Signature { get; } = "create-project";

        public CreateProject() {
            StrOutput = new List<string>();
        }

        public CliResult Execute() {
            Output("Manually creating project");
            
            Output("Type creator login:");
            var userLogin = Console.ReadLine();

            var user = UserRepository.FindByLogin(userLogin);
            if (user == null) {
                Output("User not found");
                return new CliResult(CliExitCode.NotFound, StrOutput);
            }
            
            Output("Type repo title:"); 
            var repoTitle = Console.ReadLine();
            
            Output("Type repo url:"); 
            var repoUrl = Console.ReadLine();
                        
            // TODO: get service type

            var repo = RepoRepository.CreateAndGet(user, repoTitle, repoUrl, RepoServiceType.GitHub);

            Output("Type project name:"); 
            var projectName = Console.ReadLine();

            var project = ProjectRepository.FindOrCreate(projectName, user, repo);
            
            Output("Created project:"); 
            Output(new ProjectTransformer().Transform(project).ToString());
            
            return new CliResult(CliExitCode.Ok, StrOutput);
        }
    }
}