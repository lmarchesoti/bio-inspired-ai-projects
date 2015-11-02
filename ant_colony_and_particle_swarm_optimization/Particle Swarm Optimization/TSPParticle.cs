using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commons;
using Particle_Swarm_Optimization.Base;
using Commons.Util;

namespace Particle_Swarm_Optimization {
    public class TSPParticle : Particle {
        // Mapa das Cidades
        protected TravellingSalesmanMap Map;// { get; set; }

        //Referencia a melhor particula atualmente
        //public TSPParticle gBest;
        /*
        public TSPParticle(TSPParticle particle) : base() {
            this.Position = particle.Position;
            this.Velocity = particle.Velocity;
            this.Fitness = particle.Fitness;
        }
        
        private TSPParticle() : base() {

        }
        */
        private TSPParticle(ParticleSwarm containingSwarm, TravellingSalesmanMap Map)
            : base(containingSwarm) {
            this.Map = Map;
        }

        void Init() {
            //inicializar:
            //posição
            //velocidade
            //avaliar fitness
            //inicializar gbest
            //pbest
        }
        /*
        public void UpdatePbest() {
            if (Fitness > pBest.Fitness)
                pBest = (TSPParticle)this.Clone();
        }
        */
        // Método que atualiza posição e velocidade
        public void UpdateSelf() {
            /* COPIADO DO ARTIGO
                Vid = W* Vid + c1 * rand(.) * (Pid–Xid) + c2 * Rand(.) * (Pgd –Xid) (4)
                Xid = Xid + Vid (5)
                Onde:
                c1 e c2: são duas constantes positivas que correspondem às componentes cognitivas e sociais.
                rand(.) e Rand(.) - são duas funções aleatórias no intervalo [0,1].
                W: é o fator de inércia que determina a diversificação ou intensificação das partículas.
            */

            //Hard-coded por hora
            double c1 = 0.25;
            double c2 = 0.25;
            double w = 0.25;
            Random aleatorio = new Random();

            //Problema: acessar gBest
            //Problema: implementar + para speed + speed jah estava implementado, porem como -
            this.Velocity = (TSPVelocity) (w * (TSPVelocity)this.Velocity + 
                c1 * aleatorio.Next(0, 1) * ((TSPPosition)pBest.Position - (TSPPosition)this.Position) + 
                c2 * aleatorio.Next(0, 1) * ((TSPPosition)Swarm.gBest.Position - (TSPPosition)this.Position));
            while (this.Velocity.transpositions.Count > Map.CityCount)
                this.Velocity.transpositions.RemoveAt(this.Velocity.transpositions.Count - 1);
            this.Position = (TSPPosition)this.Position + (TSPVelocity)this.Velocity;
        }

        // Função que avalia o fitness da partícula e o atualiza
        public void Eval() {
            //TODO
            //calcula fitness:
            //somar as distancias entre as cidades na ordem em que aparecem em position... GetTravelDistance jah faz isso, porem usa o TravelPlan pra isso... 
            // se posicao atual melhor q a melhor entao atualiza a melhor
           
        }

        //Ha alguma garantia de que essas particulas aleatorias sejam circuitos hamiltonianos? 

        public static TSPParticle RandomGenerate(TSPParticleSwarm containingSwarm, TravellingSalesmanMap Map) {
            TSPParticle newParticle = new TSPParticle(containingSwarm, Map);
            newParticle.Velocity = TSPVelocity.RandomGenerate(Map);
            newParticle.Position = new TSPPosition(Aleatoriety.GetRandomIntegerSequencePermutation(1, Map.CityCount).ToList());
            newParticle.EvaluateSelf();
            return newParticle;
        }

        public void EvaluateSelf() {
            this.Fitness = GetTravelDistance();
            if ((pBest != null) && (Fitness > pBest.Fitness))
                return;
            else
                pBest = (Particle)this.Clone();
        }

        public double GetTravelDistance() {
            double travelDistance = 0.0;
            for (int i = 0; i < Map.CityCount; ++i) {
                if (i == Map.CityCount - 1)
                    travelDistance += (Map.GetDistanceBetween(((TSPPosition)Position).Route[i], ((TSPPosition)Position).Route[0]));
                else
                    travelDistance += (Map.GetDistanceBetween(((TSPPosition)Position).Route[i], ((TSPPosition)Position).Route[i + 1]));
            }
            return travelDistance;
        }

        public override object Clone() {
            TSPParticle p = new TSPParticle(this.Swarm, Map) {
                Velocity = (TSPVelocity)this.Velocity.Clone(),
                Position = (TSPPosition)this.Position.Clone(),
                Fitness = this.Fitness
            };
            p.pBest = p;
            return p;
        }
    }
}