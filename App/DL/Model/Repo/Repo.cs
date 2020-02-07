using System;
using System.Linq;
using App.DL.Enum;
using App.DL.External.GitHub;
using App.DL.Repository.Repo;
using App.DL.Repository.User;
using Dapper;
using UserModel = App.DL.Model.User.User;

// ReSharper disable InconsistentNaming

namespace App.DL.Model.Repo {
    public class Repo : Micron.DL.Model.Model {
        public int id;

        public int? creator_id;

        public string guid;

        public string origin_id;

        public string title;

        public string repo_url;

        public RepoServiceType service_type;

        public DateTime created_at;

        public UserModel Creator() {
            int creatorId = creator_id ?? 0;
            return creatorId > 0 ? UserRepository.Find(creatorId) : null;
        }

        public static Repo Find(int id)
            => Connection().Query<Repo>(
                "SELECT * FROM repositories WHERE id = @id LIMIT 1",
                new {id}
            ).FirstOrDefault();

        public static Repo FindByGuid(string guid)
            => Connection().Query<Repo>(
                "SELECT * FROM repositories WHERE guid = @guid LIMIT 1",
                new {guid}
            ).FirstOrDefault();

        public static Repo FindBy(string col, string val)
            => Connection().Query<Repo>(
                $"SELECT * FROM repositories WHERE {col} = @val LIMIT 1",
                new {val}
            ).FirstOrDefault();
        
        public static Repo Find(string originId, RepoServiceType type)
            => Connection().Query<Repo>(
                $"SELECT * FROM repositories WHERE origin_id = @origin_id AND service_type = '{type.ToString()}' LIMIT 1",
                new {origin_id = originId}
            ).FirstOrDefault();

        public static Repo[] Paginate(int page, int size = 20)
            => Connection().Query<Repo>(
                "SELECT * FROM repositories OFFSET @offset LIMIT @size",
                new {offset = ((page-1) * size), size}
            ).ToArray();

        public static int Create(
            UserModel creator, string title, string repoUrl, RepoServiceType serviceType, string originId = ""
        ) {
            return ExecuteScalarInt(
                $@"INSERT INTO public.repositories(creator_id, guid, title, repo_url, service_type, origin_id) 
                        VALUES (@creator_id, @guid, @title, @repo_url, '{serviceType.ToString()}', @origin_id);
                        SELECT currval('repositories_id_seq');"
                , new {
                    creator_id = creator?.id, guid = Guid.NewGuid().ToString(), title, repo_url = repoUrl,
                    origin_id = originId
                }
            );
        }

        public Repo Save() {
            ExecuteSql(
                "UPDATE repositories SET title = @title, repo_url = @repo_url WHERE id = @id",
                new {title, repo_url, id}
            );
            return this;
        }

        public Octokit.Repository GithubRepo() => 
            GitHubApi.Client().Repository.Get(Convert.ToInt64(origin_id)).Result;

        public Project.Project Project() => Model.Project.Project.FindBy("repository_id", id);
        
        public Repo Refresh() => RepoRepository.Find(id);

        public void Delete() => ExecuteScalarInt("DELETE FROM repositories WHERE id = @id", new {id});
    }
}