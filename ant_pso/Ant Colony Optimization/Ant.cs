using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ant_Colony_Optimization
{
    class Ant
    {
        System.Random random = new Random();
        //lista de vértices que a formiga não pode retornar
        //aka solução parcial
        List<int> tabooList;

        //lista de notas para cada caminho entre as cidades
        static double[,] ProbabilityMatrix;

        //posição inicial da formiga
        int startCity;

        public Ant(int start)
        {
            this.startCity = start;
            tabooList = new List<int>();
        }

        //função que decide para qual vértice a formiga se moverá
        int Decide()
        {
            int proximo = tabooList.First();
            /* cria um vetor com todos os vértices ainda não selecionados */
            int cityCount = ProbabilityMatrix.GetLength(0);
            List<int> tabooless = Enumerable.Range(1, cityCount).Where(C => !tabooList.Contains(C)).ToList();
            int cityA, cityB;

            /* utiliza um numero aleatório e o vetor de densidade de probabilidade para decidir qual a próxima cidade */
            double choice = ((double)random.Next(1000)) / 1000;
            //itera sobre todas as cidades que ainda não foram selecionadas
            foreach (int i in tabooless) {
                //quando o proximo valor for maior que a escolha, significa que está no vértice correto
                cityA = tabooList.Last() - 1;
                cityB = cityA < i ? i - 1 : cityA;
                cityA = cityA < i ? cityA : i - 1;
                //if (ProbabilityMatrix[tabooList.Last() - 1, i - 1] > choice)
                if (ProbabilityMatrix[cityA, cityB] > choice)
                    //retorna o vértice escolhido
                    return i;
            }
            //se cair no último, retorna último vértice
            return tabooless.Last();
        }

        //cria uma solução parcial
        public void Move(AntProblem problem)
        {
            //inicializa solução parcial
            tabooList.Clear();
            tabooList.Add(startCity);

            int visit = 0;
            /* move a formiga até que todas as cidades sejam visitadas */
            //enquanto não atender à condição final da solução parcial
            while (visit++ < problem.CityCount - 1)
            {
                //se mover para o próximo vértice
                //levando em conta heurística e lista tabu
                tabooList.Add(Decide());
            }
        }

        //disponibiliza a solução
        public List<int> getSolution()
        {
            return tabooList;
        }

        //calcula os valores para a chance de cada aresta ser escolhida para fazer parte da solução final
        public static void PrepareMove(AntProblem problem) {
            //vetor que guarda as notas individuais de cada aresta
            double[,] notaTrilhas = new double[problem.CityCount, problem.CityCount];
            //vetor que guarda a soma das notas das arestas visíveis de cada cidade
            double[] somaNotas = new double[problem.CityCount];

            /* calcula a nota de cada aresta separadamente */
            //para cada aresta no grafo
            for (int cityA = 1; cityA <= problem.CityCount; cityA++) {
                for (int cityB = cityA + 1; cityB <= problem.CityCount; cityB++) {
                    // Otimizar Atribuição pelo Corte pela Metade
                    /*if (cityA == cityB) {
                        notaTrilhas[cityA - 1, cityB - 1] = 0;
                        continue;
                    }*/
                    //calcula o valor p de acordo com a trilha de feromonios e a distancia
                    notaTrilhas[cityA - 1, cityB - 1] = System.Math.Pow(problem.GetPheromoneBetween(cityA, cityB), problem.pheromoneWeight) * System.Math.Pow(1 / problem.GetDistanceBetween(cityA, cityB), problem.distanceWeight);
                    //e atualiza a soma das notas daquela cidade
                    somaNotas[cityA - 1] += notaTrilhas[cityA - 1, cityB - 1];
                }
            }

            ProbabilityMatrix = new double[problem.CityCount, problem.CityCount];
            /* calcula o vetor de densidade de probabilidade para cada cidade */
            //para cada cidade cityA no grafo
            for (int cityA = 1; cityA <= problem.CityCount; cityA++) {
                double acumulado = 0;
                //calcula a chance de visitar a cidade cityB
                for (int cityB = cityA + 1; cityB <= problem.CityCount; cityB++) {
                    // Otimizar Atribuição pelo Corte pela Metade
                    //como sendo a nota da aresta [cityA, cityB] dividido pela soma de todas as arestas visíveis de cityA (todas)
                    acumulado += notaTrilhas[cityA - 1, cityB - 1] / somaNotas[cityA - 1];
                    ProbabilityMatrix[cityA - 1, cityB - 1] = acumulado;
                    ProbabilityMatrix[cityB - 1, cityA - 1] = acumulado;
                }
            }
        }
    }
}
