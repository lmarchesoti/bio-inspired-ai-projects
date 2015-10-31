using System;
using Genetic_Algorithm_Commons;

namespace Mono_Objective_Genetic_Algorithm.Selection_Methods {
    public class Roulette : SelectionMethodBase {
        ProblemBase problem;

        public Roulette(ProblemBase problem) {
            this.problem = problem;
        }

        public override void Execute(PopulationBase population, out IndividualBase chosenIndividual1, out IndividualBase chosenIndividual2) {
            throw new NotImplementedException();
        }
    }
}
