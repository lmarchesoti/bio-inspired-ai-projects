using System;
using System.Linq;
using Genetic_Algorithm_Commons;
using System.Collections.Generic;

namespace Mono_Objective_Genetic_Algorithm {
    public class Population_MonoObjective_AG : PopulationBase {
        private ProblemBase problem;
        public IndividualBase BestIndividual {
            get {
                return Content.First(I => I.GetFitnessForObjective(problem.MonoObjectiveGoal) == Content.Max(S => S.GetFitnessForObjective(problem.MonoObjectiveGoal)));
            }
        }

        public Population_MonoObjective_AG(ProblemBase problem, int populationSize) : base(populationSize) {
            this.problem = problem;
        }

        public void RandomInitialize() {
            // Generate Initial Population
            for (int idx = 0; idx < this.InitialPopulationSize; ++idx) {
                IndividualBase individual = problem.CreateRandomSolution();
                IndividualEvaluator.Execute(individual, problem);
                AddIndividual(individual);
            }
        }

        public void BestFirstSort() {
            Content.Sort();
            Content.Reverse();
        }
    }
}
