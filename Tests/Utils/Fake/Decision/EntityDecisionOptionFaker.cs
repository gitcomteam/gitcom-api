using System.Collections.Generic;
using App.DL.Model.Decision;
using App.DL.Repository.Decision;
using Micron.DL.Module.Misc;
using DecisionModel = App.DL.Model.Decision.EntityDecision;

namespace Tests.Utils.Fake.Decision {
    public class EntityDecisionOptionFaker {
        public static EntityDecisionOption Create(DecisionModel decision = null) {
            decision ??= EntityDecisionFaker.Create();
            return EntityDecisionOptionRepository.Create(decision, "option " + Rand.RandomString());
        }

        public static List<EntityDecisionOption> CreateMany(ushort amount, DecisionModel decision = null) {
            var result = new List<EntityDecisionOption>();
            
            for (ushort i = 0; i < amount; i++) {
                result.Add(Create(decision));
            }

            return result;
        }
    }
}