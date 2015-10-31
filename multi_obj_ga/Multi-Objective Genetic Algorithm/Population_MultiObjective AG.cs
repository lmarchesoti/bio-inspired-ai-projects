using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Genetic_Algorithm_Commons;

namespace Multi_Objective_Genetic_Algorithm {
    public class Population_MultiObjective_AG : PopulationBase {
        private ProblemBase Problem;

        public Population_MultiObjective_AG(ProblemBase problem, int populationSize) : base(populationSize) {
            this.Problem = problem;
        }

        public Population_MultiObjective_AG(ProblemBase problem)
            : base(int.MaxValue) {
            this.Problem = problem;
        }

        public void RandomInitialize() {
            // Generate Initial Population
            while (IndividualCount < this.InitialPopulationSize) {
                IndividualBase individual = Problem.CreateRandomSolution();
                Problem.ValidateIndividual(individual);
                IndividualEvaluator.Execute(individual, Problem);
                if (!Content.Contains(individual))
                    AddIndividual(individual);
            }
        }

        /*
        public void EvaluatePopulationForObjectives() {
            foreach (Objective objective in problem.MultiObjectiveGoal) {
                double maxValueFound = Content.Max(I => I.GetFitnessForObjective(objective));
                double adaptedMaxValue = Math.Pow(Math.Abs(maxValueFound)*2, 3);
                objective.BigValueForMaximization = adaptedMaxValue;

                foreach (IndividualBase individual in Content) {
                    if(objective.Goal == Goal.Minimize) {
                        individual.SetFitnessForObjective(
                            objective,
                            adaptedMaxValue - individual.GetFitnessForObjective(objective)
                        );
                    }
                }
            }
        }
        */

        internal Population_MultiObjective_AG Procreate(int mutationPct) {
            throw new NotImplementedException();
        }

        public void Truncate(int archiveSize)
        {
            CalculateSortedDistances();
            while (IndividualCount > archiveSize) {
                double smallestDistanceValue = double.MaxValue;
                int smallestDistanceIdx = 0;
                bool IsSmallestDistanceUniqueByIdx = true;
                for (int distanceIdx = 0; distanceIdx < Content.Count; ++distanceIdx ) {
                    IsSmallestDistanceUniqueByIdx = true;
                    foreach (IndividualBase individual in Content) {
                        double distanceForIdx = individual.DistancesToOtherIndividuals[distanceIdx].Item2;
                        if (distanceForIdx < smallestDistanceValue) {
                            IsSmallestDistanceUniqueByIdx = true;
                            smallestDistanceIdx = distanceIdx;
                            smallestDistanceValue = distanceForIdx;
                        }
                        else {
                            IsSmallestDistanceUniqueByIdx = false;
                            if (smallestDistanceValue == 0)
                                break;
                        }
                    }
                    if(IsSmallestDistanceUniqueByIdx)
                        break;
                }
                IndividualBase individualForRemoval = Content[smallestDistanceIdx];
                Content.Remove(individualForRemoval);
                foreach (IndividualBase i in Content) {
                    i.DistancesToOtherIndividuals.RemoveAll(I => I.Item1 == individualForRemoval);
                }
            }
        }

        //CONFERIR
        //compara pela menor distancia
        /*
        public int distanceComparisonInverse(IndividualBase i1, IndividualBase i2)
        {
            int result;
            //o comprimento sempre vai ser igual.. tomara
            for (int i = 0; i < i1.DistancesToOtherIndividuals.Count; ++i)
            {
                //ele inverte o resultado comparando i2 com i1 em vez do contrário, que seria o normal
                result = i2.DistancesToOtherIndividuals.ElementAt(i).CompareTo(i1.DistancesToOtherIndividuals.ElementAt(i));
                if (result.Equals(0))
                    continue;
                else
                    return result;
            }
            return 0;
        }
        */
        public void CalculateSortedDistances() {
            double[,] extremeValues = new double[2, Problem.MultiObjectiveGoal.Count()];
            //double 
            foreach (Objective objective in Problem.MultiObjectiveGoal)
            {
                //extremeValues[0, objective.Index] = Content.Min(I => I.GetFitnessForObjective(objective));
                //extremeValues[1, objective.Index] = Content.Max(I => I.GetFitnessForObjective(objective));

                double minValue = Content.Min(I => I.GetValueForObjective(objective)); 
                double maxValue = Content.Max(I => I.GetValueForObjective(objective)); 
                double difference = maxValue - minValue;

                foreach (IndividualBase i in Content) {
                    double currentValue = i.GetValueForObjective(objective);
                    double correspondingVal = currentValue - minValue;
                    i.SetNormalizedValueForObjective(objective, correspondingVal / difference);
                }
            }

            foreach (IndividualBase i in Content) {
                i.DistancesToOtherIndividuals = new List<Tuple<IndividualBase, double>>();
                foreach (IndividualBase j in Content) {
                    i.DistancesToOtherIndividuals.Add(new Tuple<IndividualBase, double>(j, DistanceBetween(i, j))); //calcula o vetor de distancia para todos os nos
                }
                i.DistancesToOtherIndividuals.Sort(
                    delegate (Tuple<IndividualBase, double> i1, Tuple<IndividualBase, double> i2) { return i1.Item2.CompareTo(i2.Item2); }
                );
            }
        }

        private double DistanceBetween(IndividualBase i1, IndividualBase i2) {
            double distance = 0;

            foreach (Objective objective in i1.Problem.MultiObjectiveGoal) {
                double difference = i1.GetNormalizedValueForObjective(objective) - i2.GetNormalizedValueForObjective(objective); //calcular a distancia parcial como sendo a diferença
                distance += Math.Pow(difference, 2);
            }
            return distance;
        }

        public List<Population_MultiObjective_AG> FastNonDominatedSort() {
            List<Population_MultiObjective_AG> Fronts = new List<Population_MultiObjective_AG>();

            Population_MultiObjective_AG NonDominatedFront = new Population_MultiObjective_AG(Problem);
            foreach (IndividualBase individual in Content) {
                individual.DominatesList = new List<IndividualBase>();
                individual.DominatedByCount = 0;

                foreach (IndividualBase another in Content.Where(I => !I.Equals(individual))) {
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
