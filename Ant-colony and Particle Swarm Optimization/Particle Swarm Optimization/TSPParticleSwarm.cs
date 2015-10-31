using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commons;
using Particle_Swarm_Optimization.Base;

namespace Particle_Swarm_Optimization {
    public class TSPParticleSwarm : ParticleSwarm {
        public TravellingSalesmanMap Map { get; set; }

        public TSPParticleSwarm(TravellingSalesmanMap TSP, int particleCount) : base(particleCount) {
            this.Map = TSP;
        }

        public void InitializeParticles() {
            Particles.Clear();

            TSPParticle firstParticle = TSPParticle.RandomGenerate(this, Map);
            Particles.Add(firstParticle);
            gBest = (TSPParticle)firstParticle.Clone();

            // Inicializar cada Partícula
            for (int pCount = 1; pCount < ParticleCount; ++pCount) {
                TSPParticle newParticle = TSPParticle.RandomGenerate(this, Map);
                Particles.Add(newParticle);
                if (newParticle.Fitness < gBest.Fitness) {
                    gBest = (TSPParticle)newParticle.Clone();
                }
            }
        }
    }
}
