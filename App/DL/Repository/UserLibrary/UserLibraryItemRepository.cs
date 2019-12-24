using App.DL.Model.UserLibrary;

namespace App.DL.Repository.UserLibrary {
    public static class UserLibraryItemRepository {
        public static UserLibraryItem Find(int id) => UserLibraryItem.Find(id);
        
        public static UserLibraryItem Find(Model.User.User user, Model.Project.Project project) 
            => UserLibraryItem.Find(user, project);
        
        public static UserLibraryItem FindByGuid(string guid) => UserLibraryItem.FindBy("guid", guid);
        
        public static UserLibraryItem FindBy(string col, string val) => UserLibraryItem.FindBy(col, val);

        public static UserLibraryItem[] Get(Model.User.User user) => UserLibraryItem.Get(user);

        public static UserLibraryItem FindOrCreate(Model.User.User user, Model.Project.Project project) {
            var item = Find(user, project);
            if (item != null) {
                return item;
            }
            return Find(UserLibraryItem.Create(user, project));
        }
    }
}