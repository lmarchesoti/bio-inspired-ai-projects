using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Genetic_Algorithm_Commons;

namespace Mono_Objective_Genetic_Algorithm {
    public static class IndividualEvaluator {
        public static void Execute(IndividualBase individual, ProblemBase problem) {
            problem.EvaluateIndividual(individual);

            if(!individual.WasEvaluated) {
                individual.ObjectivesValuesForMaximization.Add(double.MaxValue);
                individual.ObjectivesValuesForNormalization.Add(double.MaxValue);
            }

            Objective objective = problem.MonoObjectiveGoal;
            individual.SetFitnessForObjective(objective);
            individual.WasEvaluated = true;
        }
    }
}
