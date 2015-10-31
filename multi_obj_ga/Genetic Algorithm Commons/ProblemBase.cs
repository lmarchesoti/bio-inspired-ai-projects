using System.Collections.Generic;
using System.Linq;
using System;

namespace Genetic_Algorithm_Commons {
    public abstract class ProblemBase {
        public ProblemBase() {
            MonoObjectiveGoal = DefineMonoObjetiveGeneticAlgorithmGoal();
            MultiObjectiveGoal = DefineMultiObjetiveGeneticAlgorithmGoals();
        }

        public Objective MonoObjectiveGoal { get; protected set; }
        public IEnumerable<Objective> MultiObjectiveGoal { get; protected set; }

        public abstract IndividualBase CreateRandomSolution();
        public abstract void EvaluateIndividual(IndividualBase individual);
        public abstract string SerializeIndividual(IndividualBase individual);
        public abstract string NewSerializeIndividual(IndividualBase individualBase);
        public abstract void MutateIndividual(IndividualBase individual);
        public abstract void ValidateIndividual(IndividualBase individual);
        public abstract Objective DefineMonoObjetiveGeneticAlgorithmGoal();
        public abstract IEnumerable<Objective> DefineMultiObjetiveGeneticAlgorithmGoals();

        #region Crossover Methods
        public abstract void CiclicCrossover(IndividualBase parent1, IndividualBase parent2, out IndividualBase child1, out IndividualBase child2);
        public abstract void PMXCrossover(IndividualBase parent1, IndividualBase parent2, out IndividualBase child1, out IndividualBase child2);
        #endregion
    }
}
