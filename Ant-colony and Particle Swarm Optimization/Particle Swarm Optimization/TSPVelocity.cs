using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Particle_Swarm_Optimization.Base;
using Commons;
using Commons.Util;

namespace Particle_Swarm_Optimization
{
    public class TSPVelocity : Velocity
    {
        /* contém o vetor de transposições para aplicação da velocidade */
        public List<Tuple<int, int>> transpositions;

        public TSPVelocity()
        {
            transpositions = new List<Tuple<int, int>>();
        }

        public TSPVelocity(List<Tuple<int, int>> t)
        {
            transpositions = t;
        }

        /* sobrecarga do operador - para a classe Speed
         * concatena dois vetores de velocidade
         */
        public static TSPVelocity operator +(TSPVelocity s1, TSPVelocity s2)
        {
            //retorna uma nova velocidade baseada na concatenação das transposições
            return new TSPVelocity(s1.transpositions.Concat(s2.transpositions).ToList());
        }

        /* sobrecarga do operador * para coef * velocidade
         * trata em casos baseados no valor do coeficiente:
         *  -coef = 0: return {};
         *  -coef < 0: raise exception;
         *  -coef > 0: concatenar s por c vezes para a parte inteira
         *    e truncar s por (1-coef)*|v| para a parte decimal(?)
         */
        public static TSPVelocity operator *(double coef, TSPVelocity s)
        {
            /* cria uma variável para guardar o resultado */
            TSPVelocity final = null;
            /* cria o resultado a partir da comparação de coef com 0 */
            switch (coef.CompareTo(0))
            {
                //coef = 0
                case 0:
                    //retorna um vetor vazio
                    final = new TSPVelocity();
                    break;
                //coef < 0
                case -1:
                    //throws exception of d00m
                    throw new Exception("Exception of d00m! Operação não suportada!");
                //coef > 0
                case 1:
                    final = new TSPVelocity(s.transpositions);
                    //concatena i vezes para a parte inteira de coef
                    for (int i = 0; i < (int)coef; ++i)
                        final.transpositions.AddRange(s.transpositions);
                    //trunca v por (1-c)*|v| e adiciona
                    final.transpositions.AddRange(s.transpositions.GetRange(0, (int)Math.Floor(s.transpositions.Count*(coef%1))));
                    break;
                default:
                    //YOU'LL NEVER GET ME ALIVE !!!
                    break;
            }
            /* retorna o objeto com o resultado */
            return final;
        }

        public override object Clone() {
            return new TSPVelocity(this.transpositions.ToList());
        }

        public static TSPVelocity RandomGenerate(TravellingSalesmanMap Map) {
            // 0.0 <= Valor < 1.0
            double speed = Aleatoriety.GetRandomDouble();
            int permutationsCount = (int)(speed * (Map.CityCount / 2)) + 1;
            List<Tuple<int, int>> permutations = new List<Tuple<int,int>>();
            for(int i = 0; i < permutationsCount; ++i) {
                Tuple<int, int> permutation = Aleatoriety.GetTwoDiferentValuesInRange(0, Map.CityCount);
                permutations.Add(permutation);
            }
            return new TSPVelocity(permutations);
        }

    }
}
