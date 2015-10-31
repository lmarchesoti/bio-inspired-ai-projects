using System;
using Genetic_Algorithm_Commons;

namespace Mono_Objective_Genetic_Algorithm.Selection_Methods {
    public class EstocasticTour : SelectionMethodBase {
        ProblemBase problem;
        private int tourSize;

        public EstocasticTour(ProblemBase problem, int tourSize) {
            this.problem = problem;
            this.tourSize = tourSize;
        }

        public override void Execute(PopulationBase population, out IndividualBase chosenIndividual1, out IndividualBase chosenIndividual2) {
            throw new NotImplementedException();
        }
    }
}
