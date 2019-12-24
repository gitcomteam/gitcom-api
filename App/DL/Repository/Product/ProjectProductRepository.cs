using App.DL.Model.Product;

namespace App.DL.Repository.Product {
    public static class ProjectProductRepository {
        public static ProjectProduct Find(int id) => ProjectProduct.Find(id);
        
        public static ProjectProduct[] Get(Model.Project.Project project) => ProjectProduct.Get(project);

        public static ProjectProduct FindBy(string col, string val) => ProjectProduct.FindBy(col, val);

        public static ProjectProduct FindByGuid(string val) => ProjectProduct.FindBy("guid", val);

        public static ProjectProduct Create(
            Model.Project.Project project, string name, string description, string url, string useUrl
        ) {
            return Find(ProjectProduct.Create(project, name, description, url, useUrl));
        }

        public static ProjectProduct UpdateCol(ProjectProduct product, string col, string val) {
            return product.UpdateCol(col, val);
        }
    }
}