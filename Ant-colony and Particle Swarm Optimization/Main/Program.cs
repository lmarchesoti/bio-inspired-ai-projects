using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ant_Colony_Optimization;
using Particle_Swarm_Optimization;
using Commons;

namespace Main
{
    class Program
    {
        static void Main(string[] args)
        {
            TravellingSalesmanMap TSP = new TravellingSalesmanMap(TravellingSalesmanMap.MapInstance.RAT99);

            RunACO(TSP);
            //RunPSO(TSP);
        }

        static void RunACO(TravellingSalesmanMap TSP)
        {
            //cria novo ACO
            ACO aco = new ACO(TSP, TSP.CityCount);

            //roda o ACO
            TSPSolution s = aco.Run();

            //imprime a solução
            Console.WriteLine("Optimal Solution: " + TSP.OptimalTravelDistance);
            Console.WriteLine("Solution Found: " + s.Fitness);
            Console.Write("\nSteps: ");
            foreach(int visitedCity in s.TravelPlan)
                Console.Write(TSP.GetCityAlias(visitedCity) + " => ");
            Console.Write(TSP.GetCityAlias(s.TravelPlan.First()));
            Console.ReadKey();
        }

        static void RunPSO(TravellingSalesmanMap TSP)
        {
            //cria novo PSO
            PSO pso = new PSO(TSP, 100, 10000, TSP.OptimalTravelDistance);

            //roda o PSO
            TSPSolution s = (TSPSolution) pso.Run();

            //imprime a solução
            Console.WriteLine("Optimal Solution: " + TSP.OptimalTravelDistance);
            Console.WriteLine("Solution Found: " + s.Fitness);
            Console.ReadKey();
        }
    }
}
