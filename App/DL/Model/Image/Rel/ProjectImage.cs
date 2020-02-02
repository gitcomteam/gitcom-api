using System;
using System.Linq;
using App.DL.Repository.Project;
using Dapper;

namespace App.DL.Model.Image.Rel {
    public class ProjectImage : Micron.DL.Model.Model {
        public int id;

        public string guid;

        public int project_id;
        
        public int image_id;

        public DateTime created_at;

        public static ProjectImage Find(int id)
            => Connection().Query<ProjectImage>(
                "SELECT * FROM project_images WHERE id = @id LIMIT 1", new {id}
            ).FirstOrDefault();
        
        public static ProjectImage[] Get(Project.Project project)
            => Connection().Query<ProjectImage>(
                "SELECT * FROM project_images WHERE project_id = @project_id LIMIT 20", new {
                    project_id = project.id
                }
            ).ToArray();

        public static int Create(Project.Project project, Image image) {
            return ExecuteScalarInt(
                @"INSERT INTO public.project_images(guid, project_id, image_id)
                       VALUES (@guid, @project_id, @image_id);
                       SELECT currval('project_images_id_seq');"
                , new {
                    guid = Guid.NewGuid().ToString(), project_id = project.id, image_id = image.id
                }
            );
        }

        public Project.Project Project() => ProjectRepository.Find(project_id);
        
        public Image Image() => Model.Image.Image.Find(image_id);

        public void Delete() => ExecuteScalarInt("DELETE FROM project_images WHERE id = @id", new {id});
    }
}