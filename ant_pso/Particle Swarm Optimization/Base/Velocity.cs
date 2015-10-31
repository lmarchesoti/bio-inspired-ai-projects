using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Particle_Swarm_Optimization.Base {
    public abstract class Velocity : ICloneable {
        public abstract object Clone();
    }
}
