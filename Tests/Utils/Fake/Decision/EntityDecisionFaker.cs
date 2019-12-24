using System;
using App.DL.Enum;
using App.DL.Repository.Decision;
using Tests.Utils.Fake.Project;
using Tests.Utils.Fake.User;
using UserModel = App.DL.Model.User.User;
using DecisionModel = App.DL.Model.Decision.EntityDecision;

namespace Tests.Utils.Fake.Decision { 
    public static class EntityDecisionFaker {
        public static DecisionModel Create(UserModel user = null, int entityId = 0, EntityType type = EntityType.Project) {

            if (user == null) {
                user = UserFaker.Create();
            }
            
            if (entityId == 0 && type == EntityType.Project) {
                var project = ProjectFaker.Create();
                entityId = project.id;
            }
            
            return EntityDecisionRepository.Create(
                user,
                entityId,
                type,
                "test title",
                "test content",
                DateTime.Now.AddDays(1)
            );
        }
    }
}