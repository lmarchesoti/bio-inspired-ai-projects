using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Particle_Swarm_Optimization.Base;
using Commons.Util;

namespace Particle_Swarm_Optimization {
    class TSPPosition : Position {
        /* contém a rota, ie, os passos da solução que a partícula representa */
        public List<int> Route;

        public TSPPosition() {
            Route = new List<int>();
        }

        public TSPPosition(List<int> route) {
            this.Route = route;
        }

        /*  sobrecarga do operador - para a classe Position
         *  retorna o vetor de transposições(speed) para transformar p1 em p2 
         *  P1 - P2 = v => P1 = P2 + v
         */
        public static TSPVelocity operator -(TSPPosition p1, TSPPosition p2) {
            /* cria uma variável para carregar o resultado */
            TSPVelocity s = new TSPVelocity();

            //variável temporária para criar as transposições
            List<int> p2Clone = p2.Route.ToList();

            for (int idxP1 = 0; idxP1 < p1.Route.Count; ++idxP1) {
                if (p1.Route[idxP1] != p2Clone[idxP1]) {
                    int idxElem = p2Clone.IndexOf(p1.Route[idxP1]);
                    p2Clone.Swap(idxElem, idxP1);
                    //normalizando transposições equivalentes
                    s.transpositions.Add(idxElem > idxP1 ? new Tuple<int, int>(idxP1, idxElem) : new Tuple<int, int>(idxElem, idxP1));
                }
            }
            return s;
        }

        /*  sobrecarga do operador + para posição + velocidade
         *  retorna a nova posição após aplicar as transposições presentes na velocidade
         */
        public static Position operator +(TSPPosition p, TSPVelocity s) {
            /* cria um novo elemento que carrega o resultado */
            TSPPosition final = (TSPPosition)p.Clone();
            /* aplica as transposições */
            //para cada transposição presente na velocidade
            foreach (Tuple<int, int> transp in s.transpositions) {
                //transpõe
                final.Route.Swap(transp.Item1, transp.Item2);
            }
            /* retorna o objeto com o resultado */
            return final;
        }

        //  método que realiza a transposição de dois nós no vetor de rotas
        public void transpose(Tuple<int, int> transp) {
            this.Route.Swap(transp.Item1, transp.Item2);
        }

        public override object Clone() {
            return new TSPPosition(Route.ToList());
        }
    }
}
