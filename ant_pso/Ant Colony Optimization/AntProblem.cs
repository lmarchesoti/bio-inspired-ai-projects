using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commons;

namespace Ant_Colony_Optimization
{
    //classe que herda de 'Problem', um grafo a ser utilizado comumente nos dois algoritmos
    //implementa as estruturas necessárias para que o grafo seja utilizado no algoritmo ACO
    public class AntProblem : TravellingSalesmanMap
    {
        //matriz de feromônios do problema
        protected double[,] PheromoneMatrix;
        //peso do feromônio na solução
        public double pheromoneWeight;
        //peso da distância na solução
        public double distanceWeight;

        public AntProblem(TravellingSalesmanMap TSP) : base(TSP.map)
        {
            PheromoneMatrix = new double[TSP.CityCount, TSP.CityCount];
            for (int p1 = 0; p1 < TSP.CityCount; p1++)
                for (int p2 = p1; p2 < TSP.CityCount; p2++)
                    PheromoneMatrix[p1, p2] = 1;

            pheromoneWeight = 1;
            distanceWeight = 5;
        }

        public double GetPheromoneBetween(int cityA, int cityB) {
            return PheromoneMatrix[cityA - 1, cityB - 1];
        }

        public void SetPheromoneBetween(int cityA, int cityB, double value) {
            PheromoneMatrix[cityA - 1, cityB - 1] = value;
            PheromoneMatrix[cityB - 1, cityA - 1] = value;
        }
    }
}
