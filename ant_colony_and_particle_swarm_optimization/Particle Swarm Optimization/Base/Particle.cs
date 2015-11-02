using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Particle_Swarm_Optimization.Base
{
    // Partícula do Enxame
    public abstract class Particle : ICloneable
    {
        // Enxame da Partícula
        public ParticleSwarm Swarm;
        // Posição
        public Position Position;
        // Velocidade
        public TSPVelocity Velocity;
        // Fitness
        public double Fitness;
        // Melhor posição da partícula até o momento
        protected Particle pBest;

        /*
        protected Particle(Particle particle)
        {
            this.Position = particle.Position;
            this.Velocity = particle.Velocity;
            this.Fitness = particle.Fitness;
        }
        */
        protected Particle(ParticleSwarm containingSwarm) {
            this.Swarm = containingSwarm;
        }
        /*
        public Particle(TSPParticleSwarm containingSwarm) {
            // TODO: Complete member initialization
            this.containingSwarm = containingSwarm;
        }
        */
        void Init()
        {
            //inicializar:
            //posição
            //velocidade
            //avaliar fitness
            //inicializar gbest
            //pbest
        }

        public void UpdatePbest()
        {
            if (Fitness > pBest.Fitness)
                pBest = (Particle)this.Clone();
        }

        //método que atualiza posição e velocidade
        public void UpdateSelf()
        {
            //TODO
            //atualizar posição e velocidade de acordo com o artigo
            /* COPIADO DO ARTIGO
                Vid = W* Vid + c1 * rand(.) * (Pid–Xid) + c2 * Rand(.) * (Pgd –Xid) (4)
                Xid = Xid + Vid (5)
                Onde:
                c1 e c2: são duas constantes positivas que correspondem às componentes cognitivas e sociais.
                rand(.) e Rand(.) - são duas funções aleatórias no intervalo [0,1].
                W: é o fator de inércia que determina a diversificação ou intensificação das partículas.
            */
            //this.position = ?;
            //this.speed = ?;
        }

        //função que avalia o fitness da partícula e o atualiza
        public void Eval()
        {
            //TODO
            //this.fitness = ?;
        }

        public abstract object Clone();
    }
}
