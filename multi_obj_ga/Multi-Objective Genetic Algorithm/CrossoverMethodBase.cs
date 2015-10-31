using Genetic_Algorithm_Commons;

namespace Multi_Objective_Genetic_Algorithm
{
    public abstract class CrossoverMethodBase
    {
        public ProblemBase Problem { get; set; }

        public abstract void Execute(IndividualBase parent1, IndividualBase parent2, out IndividualBase child1, out IndividualBase child2);
    }
}

