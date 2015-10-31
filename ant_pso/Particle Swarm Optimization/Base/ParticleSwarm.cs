using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Particle_Swarm_Optimization.Base {
    public abstract class ParticleSwarm {
        public int ParticleCount { get; set; }
        public List<Particle> Particles { get; protected set; }
        public Particle gBest { get; set; }

        public ParticleSwarm(int particleCount) {
            this.ParticleCount = particleCount;
            Particles = new List<Particle>(particleCount);
        }
    }
}
