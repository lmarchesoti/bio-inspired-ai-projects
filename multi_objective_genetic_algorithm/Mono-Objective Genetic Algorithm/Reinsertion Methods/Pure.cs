
namespace Mono_Objective_Genetic_Algorithm.Reinsertion_Methods {
    /// <summary>
    /// Method in which all parents are replaced by offspring
    /// </summary>
    public class Pure : ReinsertionMethodBase {
        public Pure() : base(100) {  }

        public override Population_MonoObjective_AG Execute(Population_MonoObjective_AG lastGeneration, Population_MonoObjective_AG generatedChildren) {
            return generatedChildren;
        }
    }
}
