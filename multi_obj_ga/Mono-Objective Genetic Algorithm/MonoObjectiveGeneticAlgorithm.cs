using System;
using System.Collections.Generic;
using System.Linq;
using Genetic_Algorithm_Commons;

namespace Mono_Objective_Genetic_Algorithm {
    public class MonoObjectiveGeneticAlgorithm : GeneticAlgorithm {
        Population_MonoObjective_AG population;
        int mutationPct;

        SelectionMethodBase selectionMethod;
        CrossoverMethodBase crossoverMethod;
        ReinsertionMethodBase reinsertionMethod;

        /// <summary>
        /// Represents an Entity able to execute the Genetic Algorithm for the input parameters
        /// </summary>
        /// <param name="problem">Instance of a Problem to be Solved</param>
        /// <param name="selectionMethodType">The Method to Be used for Parent Selection in Crossover Methods</param>
        /// <param name="tourSize">In case the 'Tour' Method is chosen as the current Selection Method, this parameter will be considered in its evaluation</param>
        /// <param name="crossoverMethodType">The Method to be used for Crossover between two parents</param>
        /// <param name="reinsertionMethodType">The Method by which Individuals are chosen to be part of the next Generation</param>
        /// <param name="populationSize">The Individual Count in a Population that is considered to be a Generation</param>
        /// <param name="numberOfGenerations">The Number of Generations to run before the algorithm returns an Result</param>
        /// <param name="crossoverPct">The Crossover percentage involved in the calculation</param>
        /// <param name="mutationPct">The Mutation percentage involved in the calculation</param>
        public MonoObjectiveGeneticAlgorithm(ProblemBase problem, SelectionMethodBase selectionMethod, CrossoverMethodBase crossoverMethod, ReinsertionMethodBase reinsertionMethod, int populationSize, int numberOfGenerations, int mutationPct)
            : base(problem, populationSize, numberOfGenerations) {
            this.mutationPct = mutationPct;
            this.selectionMethod = selectionMethod;
            selectionMethod.Problem = problem;
            this.crossoverMethod = crossoverMethod;
            crossoverMethod.Problem = problem;
            this.reinsertionMethod = reinsertionMethod;
        }

        public IndividualBase Execute() {
            // Clears the Individual Population
            population = new Population_MonoObjective_AG(Problem, InitialPopulationSize);

            population.RandomInitialize();

            // Make the Population become the Current Generation
            Population_MonoObjective_AG currentGeneration = population;
            Population_MonoObjective_AG newGeneration = null;

            for (int idxGeracao = 0; idxGeracao < NumberOfGenerations; ++idxGeracao) {
                newGeneration = RunGeneration(currentGeneration, selectionMethod, crossoverMethod, reinsertionMethod);
                newGeneration.BestFirstSort();
                currentGeneration = newGeneration;
            }

            return currentGeneration.BestIndividual;
        }

        private Population_MonoObjective_AG RunGeneration(Population_MonoObjective_AG currentGeneration, SelectionMethodBase selectionMethod, CrossoverMethodBase crossoverMethod, /*IReinsertionMethod reinsertionMethod, */ReinsertionMethodBase reinsertionMethod) {
            Population_MonoObjective_AG recentlyBorn = new Population_MonoObjective_AG(Problem, InitialPopulationSize);

            int expectedChildCount = ((InitialPopulationSize * reinsertionMethod.OffspringPercentage) / 100);

            while (recentlyBorn.IndividualCount < expectedChildCount) {
                IndividualBase parent1 = null, parent2 = null, child1 = null, child2 = null;
                selectionMethod.Execute(currentGeneration, out parent1, out parent2);
                crossoverMethod.Execute(parent1, parent2, out child1, out child2);
                recentlyBorn.AddIndividual(child1);
                recentlyBorn.AddIndividual(child2);
            }

            int mutatedChildCount = ((recentlyBorn.IndividualCount * mutationPct) / 100);
            for (int idx = 0; idx < mutatedChildCount; ++idx) {
                IndividualBase randomIndividual = recentlyBorn.GetRandomIndividual();
                Problem.MutateIndividual(randomIndividual);
            }

            IndividualBase individual = null;
            for (int idx = 0; idx < recentlyBorn.IndividualCount; ++idx) {
                individual = recentlyBorn.Content[idx];
                Problem.ValidateIndividual(individual);
                IndividualEvaluator.Execute(individual, Problem);
            }

            Population_MonoObjective_AG newGeneration = reinsertionMethod.Execute(currentGeneration, recentlyBorn);
            return newGeneration;
        }

        public IndividualBase Execute(int numberOfExecutions) {
            int bestResultCount;
            return Execute(numberOfExecutions, out bestResultCount);
        }

        private IndividualBase Execute(int numberOfExecutions, out int bestResultCount) {
            IndividualBase bestIndividual = Execute();
            double bestCurrentValue = bestIndividual.GetFitnessForObjective(Problem.MonoObjectiveGoal);
            bestResultCount = 1;
            for (int executionCount = 1; executionCount < numberOfExecutions; ++executionCount) {
                IndividualBase currentIndividual = Execute();
                double currentValue = currentIndividual.GetFitnessForObjective(Problem.MonoObjectiveGoal);
                if (currentValue > bestCurrentValue) {
                    bestIndividual = currentIndividual;
                    bestCurrentValue = currentValue;
                    bestResultCount = 1;
                }
                else if (currentValue == bestCurrentValue) {
                    ++bestResultCount;
                }
            }
            return bestIndividual;
        }

        public IndividualBase Execute(int numberOfExecutions, out string convergenceReport) {
            int bestResultCount;
            IndividualBase bestSolutionFound = Execute(numberOfExecutions, out bestResultCount);
            int bestSolutionOccurencePercentage = (bestResultCount * 100 / numberOfExecutions);
            convergenceReport = "A melhor solução encontrada foi de valor: " +
                bestSolutionFound.GetValueForObjective(Problem.MonoObjectiveGoal) +
                ", tendo ocorrido: " + bestSolutionOccurencePercentage + "% das execuções.";
            return bestSolutionFound;
        }

        protected override void EvaluateObjectiveValues() {
            // Create Temporary Individuals
            List<IndividualBase> temporaryList = new List<IndividualBase>();

            for (int iCount = 0; iCount < 200; ++iCount) {
                IndividualBase i = Problem.CreateRandomSolution();
                IndividualEvaluator.Execute(i, Problem);
                temporaryList.Add(i);
            }
            double maxValueFound = temporaryList.Max(I => I.GetFitnessForObjective(Problem.MonoObjectiveGoal));
            double adaptedMaxValue = Math.Abs(maxValueFound) * 2;
            Problem.MonoObjectiveGoal.BigValueForMaximization = adaptedMaxValue;
        }
    }
}
