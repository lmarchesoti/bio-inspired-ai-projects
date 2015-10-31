using System.Collections.Generic;
using System.Linq;
using Genetic_Algorithm_Commons.Util;

namespace Genetic_Algorithm_Commons {
    public abstract class PopulationBase {
        public List<IndividualBase> Content;
        protected int InitialPopulationSize;

        public PopulationBase(int populationSize) {
            this.Content = new List<IndividualBase>();
            this.InitialPopulationSize = populationSize;
        }

        public IndividualBase GetRandomIndividual() {
            return Content[Aleatoriety.GetRandomInt(Content.Count)];
        }

        public void AddIndividual(IndividualBase individual) {
            Content.Add(individual);
        }

        public void AddRange(IEnumerable<IndividualBase> individualRange) {
            Content.AddRange(individualRange);
        }

        public int IndividualCount {
            get {
                return Content.Count;
            }
        }

        public void AddPopulation(PopulationBase generatedChildren) {
            Content.AddRange(generatedChildren.Content);
        }

        public void TrimTo(int size) {
            Content = Content.Take(size).ToList();
        }
    }
}
