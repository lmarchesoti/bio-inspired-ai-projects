using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Genetic_Algorithm_Commons;
//using Multi_Objective_Genetic_Algorithm.Crossover_Methods;
//using Multi_Objective_Genetic_Algorithm.Reinsertion_Methods;
//using Multi_Objective_Genetic_Algorithm.Selection_Methods;
//eu fiz esses includes e criei as classes no pacote, mas eu não tenho idéia se era soh isso e pq ta dando erro
//pai da coisa, conserta isso pf

namespace Multi_Objective_Genetic_Algorithm {
    public abstract class MultiObjectiveGeneticAlgorithm : GeneticAlgorithm {
        protected Population_MultiObjective_AG Population;
        protected int mutationPct;

        //SelectionMethodBase selectionMethod;
        //CrossoverMethodBase crossoverMethod;
        //ReinsertionMethodBase reinsertionMethod;

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
        public MultiObjectiveGeneticAlgorithm(ProblemBase problem,
            //SelectionMethodBase selectionMethod, 
            //CrossoverMethodBase crossoverMethod, 
            //ReinsertionMethodBase reinsertionMethod, 
            int populationSize, int numberOfGenerations, int mutationPct)
        : base (problem, populationSize, numberOfGenerations) {

            this.InitialPopulationSize = populationSize;
            this.Problem = problem;
            this.NumberOfGenerations = numberOfGenerations;
            this.mutationPct = mutationPct;
            //this.selectionMethod = selectionMethod;
            //selectionMethod.Problem = problem;
            //this.crossoverMethod = crossoverMethod;
            //crossoverMethod.Problem = problem;
            //this.reinsertionMethod = reinsertionMethod;
        }

        public abstract IEnumerable<IndividualBase> Execute();

        protected abstract Population_MultiObjective_AG RunGeneration(Population_MultiObjective_AG currentGeneration);

        protected override void EvaluateObjectiveValues() {
            // Create Temporary Individuals
            List<IndividualBase> temporaryList = new List<IndividualBase>();

            for (int iCount = 0; iCount < 200; ++iCount) {
                IndividualBase i = Problem.CreateRandomSolution();
                IndividualEvaluator.Execute(i, Problem);
                temporaryList.Add(i);
            }

            foreach (Objective objective in Problem.MultiObjectiveGoal) {
                double maxValueFound = temporaryList.Max(I => I.GetFitnessForObjective(objective));
                double adaptedMaxValue = Math.Abs(maxValueFound) * 2;
                objective.BigValueForMaximization = adaptedMaxValue;
            }
        }
    }
}
