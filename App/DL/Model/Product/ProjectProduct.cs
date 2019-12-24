using System;
using System.Linq;
using App.DL.Repository.Product;
using App.DL.Repository.Project;
using Dapper;

namespace App.DL.Model.Product {
    public class ProjectProduct : Micron.DL.Model.Model {
        public int id;

        public string guid;
        
        public string name;

        public string description;

        public string url;

        public string use_url;

        public int usd_price_pennies;

        public int duration_hours;
        
        public int project_id;

        public DateTime created_at;

        public DateTime updated_at;

        public static ProjectProduct Find(int id)
            => Connection().Query<ProjectProduct>(
                "SELECT * FROM project_products WHERE id = @id LIMIT 1", new {id}
            ).FirstOrDefault();

        public static ProjectProduct FindBy(string col, string val)
            => Connection().Query<ProjectProduct>(
                $"SELECT * FROM project_products WHERE {col} = @val LIMIT 1", new {val}
            ).FirstOrDefault();

        public static int Create(Project.Project project, string name, string description, string url, string useUrl) {
            return ExecuteScalarInt(
                @"INSERT INTO project_products(guid, project_id, name, description, url, use_url, updated_at)
                        VALUES (@guid, @project_id, @name, @description, @url, @use_url, CURRENT_TIMESTAMP); 
                        SELECT currval('project_products_id_seq');"
                , new {
                    guid = Guid.NewGuid().ToString(), project_id = project.id, name, description, url, use_url = useUrl
                }
            );
        }

        public ProjectProduct UpdateCol(string col, string val) {
            ExecuteSql(
                $"UPDATE project_products SET {col} = @val, updated_at = CURRENT_TIMESTAMP WHERE id = @id",
                new {val, id}
            );
            return this;
        }
        
        public ProjectProduct UpdateCol(string col, int val) {
            ExecuteSql(
                $"UPDATE project_products SET {col} = @val, updated_at = CURRENT_TIMESTAMP WHERE id = @id",
                new {val, id}
            );
            return this;
        }
        
        public void Delete() => ExecuteScalarInt("DELETE FROM project_products WHERE id = @id", new {id});

        public ProjectProduct Refresh() => ProjectProductRepository.Find(id);

        public Project.Project Project() => ProjectRepository.Find(project_id);
    }
}