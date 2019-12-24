using System.Collections.Generic;
using App.DL.CustomObj.Repo;
using App.DL.Enum;
using Octokit;
using User = App.DL.Model.User.User;

namespace App.PL.CustomTransformer.External {
    public static class ExternalRepoTransformer {
        public static ExternalRepo CreateFrom(User owner, Repository repository) {
            if (repository.Fork || repository.Archived) {
                return null;
            }
            return new ExternalRepo() {
                Owner = owner,
                Id = repository.Id.ToString(),
                Name = repository.FullName,
                Description = repository.Description,
                ServiceType = ServiceType.GitHub
            };
        }
        
        public static ExternalRepo[] CreateFromMany(User owner, IEnumerable<Repository> repositories) {
            var result = new List<ExternalRepo>();

            foreach (var repo in repositories) {
                var externalRepo = CreateFrom(owner, repo);
                if (externalRepo == null) {
                    continue;
                }
                result.Add(externalRepo);
            }

            return result.ToArray();
        }
    }
}