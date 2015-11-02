using System;
using System.Collections.Generic;

namespace Genetic_Algorithm_Commons
{
    public abstract class IndividualBase : IComparable
    {
        public ProblemBase Problem { get; protected set; }
        public double CrowdingDistance { get; set; }
        public List<double> ObjectivesValuesForMaximization { get; set; }
        public List<double> ObjectivesValuesForNormalization { get; set; }
        public bool WasEvaluated { get; set; }

		//NSGA2
        public int NonDominationRank { get; set; }
        public int DominatedByCount { get; set; }
        public List<IndividualBase> DominatesList { get; set; }

		//SPEA2
        public int dominationCount;
        public int Strength { get; set; }
        public double RawFitness;
        public double FinalFitness; // rawFitness + (1 / (h + 2)), onde h = maior distancia de outro elemento de population + archive
        public List<Tuple<IndividualBase, double>> DistancesToOtherIndividuals; // guarda todos os valores de distancia para os outros elementos de population + archive
        
		
        protected IndividualBase() { }
        public IndividualBase(ProblemBase problem) { 
            this.Problem = problem;
            this.ObjectivesValuesForMaximization = new List<double>();
            this.ObjectivesValuesForNormalization = new List<double>();
            this.DominatesList = new List<IndividualBase>();
            this.DistancesToOtherIndividuals = new List<Tuple<IndividualBase, double>>();
        }

        public abstract int CompareTo(object obj);

        public int CompareToByObjective(IndividualBase another, Objective objective) {
            double individualAttributeValue = GetFitnessForObjective(objective);
            double anotherIndividualAttributeValue = another.GetFitnessForObjective(objective);

            if (anotherIndividualAttributeValue > individualAttributeValue)
                return -1;
            else if (anotherIndividualAttributeValue == individualAttributeValue)
                return 0;
            else
                return 1;
        }

        public bool Dominates(IndividualBase another) {
            bool dominatesInAnObjective = false;

            foreach (Objective objective in Problem.MultiObjectiveGoal) {
                double individualAttributeValue = GetFitnessForObjective(objective);
                double anotherIndividualAttributeValue = another.GetFitnessForObjective(objective);
                if (individualAttributeValue > anotherIndividualAttributeValue)
                    dominatesInAnObjective = true;
                else if (individualAttributeValue < anotherIndividualAttributeValue) {
                    return false;
                }
            }
            if (dominatesInAnObjective)
                return true;
            return false;
        }

        public bool IsDominatedBy(IndividualBase another) {
            bool dominatedInAnObjective = false;

            foreach (Objective objective in Problem.MultiObjectiveGoal) {
                double individualAttributeValue = GetFitnessForObjective(objective);
                double anotherIndividualAttributeValue = another.GetFitnessForObjective(objective);
                if (individualAttributeValue > anotherIndividualAttributeValue)
                    return false;
                else if (individualAttributeValue < anotherIndividualAttributeValue) {
                    dominatedInAnObjective = true;
                }
            }
            if (dominatedInAnObjective)
                return true;
            return false;
        }

        public double GetFitnessForObjective(Objective objective) {
            return ObjectivesValuesForMaximization[objective.Index];
        }

        public void SetFitnessForObjective(Objective objective) {
            if (objective.Goal == Goal.Minimize)
                ObjectivesValuesForMaximization[objective.Index] = Math.Abs(objective.BigValueForMaximization - GetValueForObjective(objective));
            else
                ObjectivesValuesForMaximization[objective.Index] = GetValueForObjective(objective);           
        }

        public double GetValueForObjective(Objective objective) {
            return Convert.ToDouble(objective.Attribute.GetValue(this, null));
        }

        public void SetValueForObjective(Objective objective, double value) {
            objective.Attribute.SetValue(this, value, null);
        }

        public double GetNormalizedValueForObjective(Objective objective) {
            return ObjectivesValuesForNormalization[objective.Index];
        }

        public void SetNormalizedValueForObjective(Objective objective, double value) {
            ObjectivesValuesForNormalization[objective.Index] = value;
        }

        public abstract override bool Equals(Object obj);


        public override string ToString() {
            return Problem.NewSerializeIndividual(this);
        }
    }
}
