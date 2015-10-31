using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Genetic_Algorithm_Commons;
using Genetic_Algorithm_Commons.Util;

namespace Multi_Objective_Genetic_Algorithm {
    public class NSGA2 : MultiObjectiveGeneticAlgorithm {
        protected Population_MultiObjective_AG offspring;
        protected int offspringPopSize;

        public NSGA2(ProblemBase problem,
            //SelectionMethodBase selectionMethod, 
            //CrossoverMethodBase crossoverMethod, 
            //ReinsertionMethodBase reinsertionMethod, 
            int populationSize, int offspringPopSize, int numberOfGenerations, int mutationPct)
            : base(problem, populationSize, numberOfGenerations, mutationPct) {
                this.offspringPopSize = offspringPopSize;
        }

        public override IEnumerable<IndividualBase> Execute() {
            //Initialize Population + Evaluate Objective Values
            Population = new Population_MultiObjective_AG(Problem, InitialPopulationSize);
            Population.RandomInitialize();
            
            // Assign Rank Based on Pareto Dominance
            FastNonDominatedSort(Population);

            // Assign Crowding Distance for the Population
            CrowdingDistanceAssignment(Population.Content);

            // Creates Offspring through Selection, Recombination, and Mutation
            Population_MultiObjective_AG childPopulation = Procreate(Population);

            Population_MultiObjective_AG Pnext = Population;
            Population_MultiObjective_AG Qnext = childPopulation;

            Population_MultiObjective_AG currentGeneration = new Population_MultiObjective_AG(Problem, 2 * InitialPopulationSize);
            currentGeneration.AddPopulation(Pnext);
            currentGeneration.AddPopulation(Qnext);

            for (int idxGeracao = 1; idxGeracao < NumberOfGenerations; ++idxGeracao) {
                Pnext = RunGeneration(currentGeneration);
                Qnext = Procreate(Pnext);

                currentGeneration = new Population_MultiObjective_AG(Problem, 2 * InitialPopulationSize);
                currentGeneration.AddPopulation(Pnext);
                currentGeneration.AddPopulation(Qnext);
            }
            return FastNonDominatedSort(currentGeneration).First().Content;
        }

        private void CrowdingDistanceAssignment(List<IndividualBase> individuals) {
            int l = individuals.Count();

            foreach (IndividualBase individual in individuals) {
                individual.CrowdingDistance = 0;
            }

            foreach (Objective m in Problem.MultiObjectiveGoal) {
                individuals.Sort(
                    delegate(IndividualBase i1, IndividualBase i2) {
                        return i1.CompareToByObjective(i2, m);
                    });
                double infiniteValue = double.MaxValue;
                individuals.First().CrowdingDistance = infiniteValue;
                individuals.Last().CrowdingDistance = infiniteValue;
                double firstIndividualObjectiveValue = individuals.First().GetFitnessForObjective(m);
                double lastIndividualObjectiveValue = individuals.Last().GetFitnessForObjective(m);
                for (int individualIdx = 1; individualIdx < l - 1; ++individualIdx) {
                    double previousIndividualObjectiveValue = individuals[individualIdx - 1].GetFitnessForObjective(m);
                    double nextIndividualObjectiveValue = individuals[individualIdx + 1].GetFitnessForObjective(m);
                    individuals[individualIdx].CrowdingDistance += (
                        (nextIndividualObjectiveValue - previousIndividualObjectiveValue) / (lastIndividualObjectiveValue - firstIndividualObjectiveValue)
                    );
                }
            }
        }

        protected override Population_MultiObjective_AG RunGeneration(Population_MultiObjective_AG currentGeneration) {
            List<Population_MultiObjective_AG> fronts = FastNonDominatedSort(currentGeneration);

            Population_MultiObjective_AG Pnext = new Population_MultiObjective_AG(Problem, InitialPopulationSize);
            int i = 0;
            while ((Pnext.IndividualCount + fronts[i].IndividualCount) < InitialPopulationSize) {
                CrowdingDistanceAssignment(fronts[i].Content);
                Pnext.AddPopulation(fronts[i]);
                i++;
            }
            fronts[i].Content.Sort(CrowdedComparison);
            fronts[i].Content.Reverse();
            Pnext.AddRange(fronts[i].Content.Take(InitialPopulationSize - Pnext.IndividualCount));
            return Pnext;
        }

        public static int CrowdedComparison(IndividualBase i1, IndividualBase i2) {
            int partial = i1.NonDominationRank < i2.NonDominationRank ? // se i1 for de um front menor
                1 : //retorna 1 (i1)
                i2.NonDominationRank < i1.NonDominationRank ? //se i2 for de um front menor
                -1 : //retorna -1 (i2)
                0; //se for igual, retorna a comparação entre as distâncias
            if (!partial.Equals(0))
                return partial;
            return i1.CrowdingDistance.CompareTo(i2.CrowdingDistance);
        }

        private Population_MultiObjective_AG Procreate(Population_MultiObjective_AG population) {
            int expectedChildCount = InitialPopulationSize;
            Population_MultiObjective_AG newGeneration = new Population_MultiObjective_AG(Problem, expectedChildCount);

            while (newGeneration.IndividualCount < expectedChildCount)
            {
                // Selection Method
                IndividualBase parent1 = BinaryTournment(population);
                IndividualBase parent2 = BinaryTournment(population);

                // Recombination Method
                IndividualBase child1 = null, child2 = null;
                Problem.PMXCrossover(parent1, parent2, out child1, out child2);

                if(!newGeneration.Content.Contains(child1))
                    newGeneration.AddIndividual(child1);
                if (!newGeneration.Content.Contains(child2))
                    newGeneration.AddIndividual(child2);
            }

            foreach (IndividualBase individual in newGeneration.Content) {
                // Mutation Method
                int aleatoryPercentage = Aleatoriety.GetRandomInt(100);
                if (aleatoryPercentage < mutationPct)
                    Problem.MutateIndividual(individual);
                Problem.ValidateIndividual(individual);
				IndividualEvaluator.Execute(individual, Problem);
            }
            return newGeneration;
        }

        private IndividualBase BinaryTournment(Population_MultiObjective_AG population) {
            IndividualBase[] contestants = new IndividualBase[2];

            contestants[0] = population.GetRandomIndividual();
            do {
                contestants[1] = population.GetRandomIndividual();
            } while (contestants[0] == contestants[1]);

            return BestByCrowdedComparison(contestants[0], contestants[1]);
        }

        private IndividualBase BestByCrowdedComparison(IndividualBase individual1, IndividualBase individual2) {
            if (individual1.NonDominationRank < individual2.NonDominationRank)
                return individual1;
            if (individual2.NonDominationRank < individual1.NonDominationRank)
                return individual2;
            // Se Estão no Mesmo Ranking de Não-Dominância
            if (individual1.CrowdingDistance > individual2.CrowdingDistance)
                return individual1;
            if (individual2.CrowdingDistance > individual1.CrowdingDistance)
                return individual2;
            // Se têm a Mesma CrowdingDistance
            if (Aleatoriety.GetRandomInt(2) == 0)
                return individual1;
            else
                return individual2;
        }

        private List<Population_MultiObjective_AG> FastNonDominatedSort(Population_MultiObjective_AG population) {
            List<Population_MultiObjective_AG> Fronts = new List<Population_MultiObjective_AG>();

            Population_MultiObjective_AG NonDominatedFront = new Population_MultiObjective_AG(Problem);
            foreach (IndividualBase individual in population.Content) {
                individual.DominatesList = new List<IndividualBase>();
                individual.DominatedByCount = 0;

                foreach (IndividualBase another in population.Content.Where(I => !I.Equals(individual))) {
                    if (individual.Dominates(another)) {
                        individual.DominatesList.Add(another);  // Lista dos elementos que sao dominados
                    }
                    else if (another.Dominates(individual)) {
                        individual.DominatedByCount++; // O atributo np nao foi implementado ainda, se refere a qnt de elementos q o dominam 
                    }
                }
                if (individual.DominatedByCount == 0) {
                    individual.NonDominationRank = 1;
                    NonDominatedFront.AddIndividual(individual);
                }
            }
            int NonDominationRank = 2;
            Population_MultiObjective_AG CurrentFront = NonDominatedFront;
            Population_MultiObjective_AG FrontQ = NonDominatedFront;
            do {
                Fronts.Add(FrontQ);
                FrontQ = new Population_MultiObjective_AG(Problem);
                foreach (IndividualBase individual in CurrentFront.Content) {
                    foreach (IndividualBase dominatedIndividual in individual.DominatesList) {
                        dominatedIndividual.DominatedByCount--;
                        if (dominatedIndividual.DominatedByCount == 0) {
                            dominatedIndividual.NonDominationRank = NonDominationRank;
                            FrontQ.AddIndividual(dominatedIndividual);
                        }
                    }
                }
                CurrentFront = FrontQ;
                NonDominationRank++;
            } while (FrontQ.IndividualCount != 0);
            return Fronts;
        }
    }
}
