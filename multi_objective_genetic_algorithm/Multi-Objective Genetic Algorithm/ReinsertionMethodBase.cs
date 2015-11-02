using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Multi_Objective_Genetic_Algorithm {
    public abstract class ReinsertionMethodBase {
        public int OffspringPercentage { get; private set; }

        private ReinsertionMethodBase() { }
        public ReinsertionMethodBase(int offspringPercentage) {
            this.OffspringPercentage = offspringPercentage;
        }

        public abstract Population_MultiObjective_AG Execute(Population_MultiObjective_AG lastGeneration, Population_MultiObjective_AG generatedChildren);
    }
}
