using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mono_Objective_Genetic_Algorithm {
    public abstract class ReinsertionMethodBase {
        public int OffspringPercentage { get; private set; }

        private ReinsertionMethodBase() { }
        public ReinsertionMethodBase(int offspringPercentage) {
            this.OffspringPercentage = offspringPercentage;
        }

        public abstract Population_MonoObjective_AG Execute(Population_MonoObjective_AG lastGeneration, Population_MonoObjective_AG generatedChildren);
    }
}
