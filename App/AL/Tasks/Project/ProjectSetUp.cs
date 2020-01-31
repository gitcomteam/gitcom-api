using App.AL.Utils.Work;
using App.DL.Model.User;
using App.DL.Repository.Alias;
using App.DL.Repository.Board;
using App.DL.Repository.BoardColumn;
using App.DL.Repository.Card;
using App.DL.Repository.ProjectTeamMember;
using App.DL.Repository.UserLibrary;

namespace App.AL.Tasks.Project {
    public static class ProjectSetUp {
        private const string DefaultCardDescription = "Task description goes here, people can fund this project and" +
        "after each task is finished all funding will be disctributed across all participants based on their effort";
        
        public static void Run(DL.Model.Project.Project project, User creator) {
            ProjectTeamMemberRepository.CreateAndGet(project, creator);
            ProjectAliasRepository.Create(project, creator.login);
            ProjectWorkUtils.SetUp(project);

            // Basic boards set up
            var board = BoardRepository.CreateAndGet("Development", "Basic board", project, creator);
            var todoColumn = BoardColumnRepository.CreateAndGet("TODO", board, 1);
            BoardColumnRepository.CreateAndGet("In progress", board, 2);
            BoardColumnRepository.CreateAndGet("Done", board, 3);
            CardRepository.CreateAndGet(
                "Example card", DefaultCardDescription, 1, todoColumn, creator
            );
            UserLibraryItemRepository.FindOrCreate(project.Creator(), project);
        }
    }
}