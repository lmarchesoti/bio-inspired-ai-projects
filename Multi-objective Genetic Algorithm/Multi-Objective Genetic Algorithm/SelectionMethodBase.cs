using Genetic_Algorithm_Commons;

namespace Multi_Objective_Genetic_Algorithm {
    public abstract class SelectionMethodBase {
        public ProblemBase Problem { get; set; }

        public abstract void Execute(PopulationBase population, out IndividualBase chosenIndividual1, out IndividualBase chosenIndividual2);
    }
}
