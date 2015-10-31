using System;
using System.Collections.Generic;
using System.Linq;
using Genetic_Algorithm_Commons;

namespace Mono_Objective_Genetic_Algorithm.Reinsertion_Methods {
    public class FitnessBased : ReinsertionMethodBase {
        public FitnessBased(int offspringPercentage) : base(offspringPercentage) {
            if (offspringPercentage < 100) {
                throw new ArgumentException("Erro: A Porcentagem Definida Foi Igual ou Inferior a 1000% para o método de Reinserção Baseada em Fitness.");
            }
        }

        public override Population_MonoObjective_AG Execute(Population_MonoObjective_AG lastGeneration, Population_MonoObjective_AG generatedChildren) {
            generatedChildren.BestFirstSort();
            generatedChildren.TrimTo(lastGeneration.IndividualCount);
            Population_MonoObjective_AG newPopulation = generatedChildren;
            return newPopulation;
        }
    }
}

