using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Genetic_Algorithm_Commons {
    public static class IndividualEvaluator {
        public static void Execute(IndividualBase individual, ProblemBase problem) {
            problem.EvaluateIndividual(individual);

            if(!individual.WasEvaluated) {
                foreach (Objective objective in problem.MultiObjectiveGoal) {
                    individual.ObjectivesValuesForMaximization.Add(double.MaxValue);
                    individual.ObjectivesValuesForNormalization.Add(double.MaxValue);
                }
            }

            foreach (Objective objective in problem.MultiObjectiveGoal)
                individual.SetFitnessForObjective(objective);

            individual.WasEvaluated = true;
        }
    }
}
