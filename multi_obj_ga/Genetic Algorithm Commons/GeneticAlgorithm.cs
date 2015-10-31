using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Genetic_Algorithm_Commons {
    public abstract class GeneticAlgorithm {
        public ProblemBase Problem { get; set; }
        public int NumberOfGenerations { get; set; }
        public int InitialPopulationSize { get; set; }

        private GeneticAlgorithm() { }

        public GeneticAlgorithm(ProblemBase problem, int initialPopulationSize, int numberOfGenerations) {
            this.Problem = problem;
            this.InitialPopulationSize = initialPopulationSize;
            this.NumberOfGenerations = numberOfGenerations;
            EvaluateObjectiveValues();
        }

        protected abstract void EvaluateObjectiveValues();
    }
}
