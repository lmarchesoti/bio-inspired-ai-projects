using Genetic_Algorithm_Commons;
using System;

namespace Mono_Objective_Genetic_Algorithm.Selection_Methods {
    public class Tour : SelectionMethodBase {
        private int tourSize;

        public Tour(int tourSize) {
            this.tourSize = tourSize;
        }

        public override void Execute(PopulationBase population, out IndividualBase chosenIndividual1, out IndividualBase chosenIndividual2) {
            IndividualBase[] selectedParents = new IndividualBase[2];

            for (int individualIdx = 0; individualIdx < 2; ++individualIdx) {
                selectedParents[individualIdx] = population.GetRandomIndividual();
                for (int selectionIdx = 1; selectionIdx < tourSize; ++selectionIdx) {
                    IndividualBase randomIndividual = population.GetRandomIndividual();
                    if (randomIndividual.GetFitnessForObjective(Problem.MonoObjectiveGoal) > selectedParents[individualIdx].GetFitnessForObjective(Problem.MonoObjectiveGoal))
                        selectedParents[individualIdx] = randomIndividual;
                }
            }
            chosenIndividual1 = selectedParents[0];
            chosenIndividual2 = selectedParents[1];
        }
    }
}
