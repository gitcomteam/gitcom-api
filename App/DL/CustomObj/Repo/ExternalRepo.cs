using App.DL.Enum;
using App.DL.Model.User;

namespace App.DL.CustomObj.Repo {
    public class ExternalRepo {
        public User Owner { get; set; }
        
        public string Id { get; set; }
        
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public ServiceType ServiceType { get; set; }
    }
}