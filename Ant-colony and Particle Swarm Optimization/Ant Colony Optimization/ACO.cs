using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commons;

namespace Ant_Colony_Optimization
{
    public class ACO
    {
        //problema a ser utilizado
        AntProblem problem;
        //lista de formigas do algoritmo
        List<Ant> ants;
        //melhor solução
        TSPSolution bestSolutionFound;
        //parâmetros a serem utilizados em formulas
        //taxa de evaporação
        double taxaEvaporacao;

        //construtor do ACO
        //recebe os parâmetros a serem utilizados nas funções de movimento da formiga e de atualização de feromônio
        public ACO(TravellingSalesmanMap TSP, int numAnts)
        {
            //inicializar variáveis
            problem = new AntProblem(TSP);
            bestSolutionFound = new TSPSolution();
            taxaEvaporacao = 0.9;
            //cria as formigas e coloca cada uma em um vértice aleatório do grafo
            System.Random random = new Random();
            ants = new List<Ant>();
            for (int i = 0; i < numAnts; i++)
                //ants.Add(new Ant(random.Next(problem.CityCount) + 1));
                ants.Add(new Ant((i + 1)%(TSP.CityCount + 1)));
        }

        //atualiza o valor dos feromônios no problema após cada iteração
        void UpdatePheromones()
        {
            int cityB, cityC;
            //atualiza o valor dos feromônios de cada aresta
            //para cada aresta no grafo
            for (int cityA = 1; cityA <= problem.CityCount; ++cityA) {
                for (cityB = cityA; cityB <= problem.CityCount; ++cityB) {
                    //evaporar feromônio de acordo com a fórmula do artigo
                    //(1-taxa de evaporação) * atual
                    problem.SetPheromoneBetween(cityA, cityB, (1 - taxaEvaporacao) * problem.GetPheromoneBetween(cityA, cityB));
                }

                //para cada aresta que cada formiga andou
                foreach (Ant ant in ants) {
                    TSPSolution sol = new TSPSolution(ant.getSolution(), problem);
                    int c1 = sol.TravelPlan[0];
                    foreach (int c2 in ant.getSolution())
                    {
                        cityB = c1;
                        cityC = cityB < c2 ? c2 : cityB;
                        cityB = cityB < c2 ? cityB : c2;
                        //atualizar o valor do feromônio de acordo com o artigo
                        //(+ persistencia * 1/f(S), onde f(S) é a distância entre as duas cidades)
                        //problem.SetPheromoneBetween(cityB, cityC, problem.GetPheromoneBetween(cityB, cityC) + (taxaEvaporacao * (1 / problem.GetDistanceBetween(cityB, cityC))));
                        problem.SetPheromoneBetween(cityB, cityC, problem.GetPheromoneBetween(cityB, cityC) + (taxaEvaporacao * (1 / sol.Fitness)));
                    }
                }
            }
        }

        //método principal do ACO
        public TSPSolution Run()
        {
            //verificar condição de término
            for (int iter = 0; iter < 5000; iter++)
            {
                //prepara as estruturas para a decisão de caminho das formigas
                Ant.PrepareMove(problem);

                //para cada formiga, movê-las até o destino
                foreach (Ant ant in ants)
                {
                    ant.Move(problem);
                }

                //avaliar o custo de todas as soluções
                TSPSolution partial;
                foreach (Ant ant in ants)
                {
                    partial = new TSPSolution(ant.getSolution(), problem);
                    if (partial.BetterThan(bestSolutionFound))
                    {
                        //guardar a melhor solução até o momento
                        bestSolutionFound = (TSPSolution) partial.Clone();
                        Console.WriteLine("Melhor Solucao Encontrada: " +  bestSolutionFound.Fitness);
                    }
                }
                //atualizar as trilhas de feromônio
                UpdatePheromones();
            }
            //retornar solução
            return bestSolutionFound;
        }

    }
}
