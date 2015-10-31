using System;
using System.Collections.Generic;
using System.Linq;
using Genetic_Algorithm_Commons;

namespace Mono_Objective_Genetic_Algorithm.Reinsertion_Methods {
    /// <summary>
    /// Reinserting method resulting in less offspring than parents and replacing the worst parents
    /// </summary>
    public class Elitist : ReinsertionMethodBase {
        public Elitist(int offspringPercentage) : base(offspringPercentage) {
            if (offspringPercentage >= 50) {
                throw new ArgumentException("Erro: A Porcentagem Definida Foi Igual ou Superior a 50% para o método de Reinserção Elitismo.");
            }
        }

        public override Population_MonoObjective_AG Execute(Population_MonoObjective_AG lastGeneration, Population_MonoObjective_AG generatedChildren) {
            Population_MonoObjective_AG newPopulation = generatedChildren;
            lastGeneration.BestFirstSort();
            IEnumerable<IndividualBase> remainingParents = lastGeneration.Content.Take(lastGeneration.IndividualCount - generatedChildren.IndividualCount);
            newPopulation.AddRange(remainingParents);
            return newPopulation;
        }
    }
}
