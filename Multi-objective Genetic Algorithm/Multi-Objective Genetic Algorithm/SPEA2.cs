using System;
using System.Collections.Generic;
using System.Linq;
using Genetic_Algorithm_Commons;
using Genetic_Algorithm_Commons.Problems;
using Genetic_Algorithm_Commons.Util;

namespace Multi_Objective_Genetic_Algorithm {
    public class SPEA2 : MultiObjectiveGeneticAlgorithm {
        protected Population_MultiObjective_AG Archive;
        protected int ArchivePopulationSize;

        public SPEA2(ProblemBase problem, int populationSize, int archivePopSize, int numberOfGenerations, int mutationPct)
            : base(problem, populationSize, numberOfGenerations, mutationPct) {
            this.ArchivePopulationSize = archivePopSize;
        }

        public override IEnumerable<IndividualBase> Execute() {
            //Inicializacao
            Population = new Population_MultiObjective_AG(Problem, InitialPopulationSize);
            Population.RandomInitialize();

            Archive = new Population_MultiObjective_AG(Problem, ArchivePopulationSize);

            Population_MultiObjective_AG currentGeneration = Population;
            Population_MultiObjective_AG newGeneration = null;

            for (int idxGeracao = 0; idxGeracao < NumberOfGenerations; ++idxGeracao) {
                newGeneration = RunGeneration(currentGeneration);
                currentGeneration = newGeneration;
            }

            IEnumerable<IndividualBase> nonDominatedFront = currentGeneration.FastNonDominatedSort().First().Content;
            IEnumerable<IndividualBase> firstItemsByMakeSpan = (from i in nonDominatedFront.Cast<TaskSchedulingSolution>()
                                                               group i by i.MakeSpan into g
                                                               select g.First()).OrderBy(I => I.MakeSpan);

            return firstItemsByMakeSpan;
        }

        protected override Population_MultiObjective_AG RunGeneration(Population_MultiObjective_AG currentGeneration) {
            Population_MultiObjective_AG everyone = new Population_MultiObjective_AG(Problem, InitialPopulationSize + ArchivePopulationSize);
            everyone.AddPopulation(currentGeneration);
            everyone.AddPopulation(Archive);

            #region Fitness Assignment
            // The Strength of an Individual is the Count of those he Dominates
            foreach (IndividualBase individual in everyone.Content) {
                individual.Strength = everyone.Content.Where(I => !I.Equals(individual)).Count(I => individual.Dominates(I));
            }

            // The RawFitness of an Individual is the sum of the strengths of all those who Dominate him
            foreach (IndividualBase individual in everyone.Content) {
                individual.RawFitness = everyone.Content.Where(I => I.Dominates(individual)).Sum(D => D.Strength);
            }

            // The True Fitness is Calculated
            everyone.CalculateSortedDistances();
            int k = (int)Math.Sqrt(InitialPopulationSize + ArchivePopulationSize);
            foreach (IndividualBase i in everyone.Content) {
                double distanceSought = i.DistancesToOtherIndividuals[k].Item2;
                double density = (1 / (distanceSought + 2));
                i.FinalFitness = i.RawFitness + density;
            }
            #endregion

            #region Environment Selection
            Population_MultiObjective_AG nextArchive = new Population_MultiObjective_AG(Problem, ArchivePopulationSize);
            List<IndividualBase> excludeList = new List<IndividualBase>();

            //insere em nextArchive todos os elementos da populacao total com finalFitness < 1
            //poderia ser tambem rawFitness = 0, mas no artigo especifica o dito acima, podemos seguir
            //aproveitar pra remover do total os elementos inseridos, vai facilitar no proximo passo..
            IEnumerable<IndividualBase> nonDominatedSolutions = everyone.Content.Where(I => I.FinalFitness < 1);
            nextArchive.AddRange(nonDominatedSolutions);
            everyone.Content.RemoveAll(I => nonDominatedSolutions.Contains(I));

            if (nextArchive.IndividualCount < ArchivePopulationSize) {
                everyone.Content.Sort(finalFitnessComparison);

                //TODO: Confirmar se é melhor maior ou menor, esse reverse faz pegar os menores
                everyone.Content.Reverse();

                IEnumerable<IndividualBase> complement = everyone.Content.Take(ArchivePopulationSize - nextArchive.IndividualCount);
                nextArchive.AddRange(complement);
            }
            else if(nextArchive.IndividualCount > ArchivePopulationSize) {
                //trunca a população pela distancia
                nextArchive.Truncate(ArchivePopulationSize);
            }

            //acho que não vai precisar do antigo mais.. conferir !
            this.Archive = nextArchive;
            #endregion

            #region Mating Selection
            //Seleção dos Pais da proxima populacao, leva em conta apenas archive
            Population_MultiObjective_AG matingPool = Procreate(nextArchive);
            #endregion

            return matingPool;
        }

        private IndividualBase BinaryTournment(Population_MultiObjective_AG population) {
            IndividualBase[] contestants = new IndividualBase[2];

            contestants[0] = population.GetRandomIndividual();
            for (int i = 0; i < 10; ++i) {
                contestants[1] = population.GetRandomIndividual();
                if ((contestants[0] != contestants[1]) || i == 9) {
                    break;
                }
            }
            //do {
            //    contestants[1] = population.GetRandomIndividual();
            //} while (contestants[0] == contestants[1]);

            return finalFitnessComparison(contestants[0], contestants[1]) >= 0 ? contestants[0] : contestants[1];
        }

        private Population_MultiObjective_AG Procreate(Population_MultiObjective_AG mating) {
            int expectedChildCount = InitialPopulationSize;

            Population_MultiObjective_AG newGeneration = new Population_MultiObjective_AG(Problem, expectedChildCount);
            while(newGeneration.IndividualCount < expectedChildCount) {
                // Selection Method
                IndividualBase parent1 = BinaryTournment(mating);
                IndividualBase parent2 = BinaryTournment(mating);
                IndividualBase child1 = null, child2 = null;
                
                // Crossover Method
                Problem.PMXCrossover(parent1, parent2, out child1, out child2);
                for (int i = 0; i < 10; ++i) {
                    if (!newGeneration.Content.Contains(child1) || i == 9) {
                        newGeneration.AddIndividual(child1);
                        break;
                    }
                }
                for (int i = 0; i < 10; ++i) {
                    if (!newGeneration.Content.Contains(child2) || i == 9) {
                        newGeneration.AddIndividual(child2);
                        break;
                    }
                }
            }

            foreach (IndividualBase individual in newGeneration.Content) {
                int aleatoryPercentage = Aleatoriety.GetRandomInt(100);
                if (aleatoryPercentage < mutationPct)
                    Problem.MutateIndividual(individual);
                Problem.ValidateIndividual(individual);
                IndividualEvaluator.Execute(individual, Problem);
            }
            return newGeneration;
        }

        //CONFERIR
        public int finalFitnessComparison(IndividualBase i1, IndividualBase i2) {
            return i1.FinalFitness < i2.FinalFitness ? // se i1 for menor
                1 : //retorna 1 (i1)
                i2.FinalFitness < i1.FinalFitness ? //se i2 for menor
                -1 : //retorna -1 (i2)
                0; //se for igual, retorna 0
        }
    }
}