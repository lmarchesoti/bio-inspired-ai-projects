using Genetic_Algorithm_Commons;

namespace Mono_Objective_Genetic_Algorithm.Reinsertion_Methods {
    public class BestAmongstAll : ReinsertionMethodBase {
        public BestAmongstAll(int offspringPercentage) : base(offspringPercentage) {  }

        public override Population_MonoObjective_AG Execute(Population_MonoObjective_AG lastGeneration, Population_MonoObjective_AG generatedChildren) {
            Population_MonoObjective_AG currentPopulation = lastGeneration;
            currentPopulation.AddPopulation(generatedChildren);
            currentPopulation.BestFirstSort();
            currentPopulation.TrimTo(lastGeneration.IndividualCount);
            return currentPopulation;
        }
    }
}
