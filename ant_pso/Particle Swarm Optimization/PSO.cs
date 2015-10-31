using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commons;

namespace Particle_Swarm_Optimization
{
    public class PSO
    {
        //grafo com as estruturas a serem utilizadas pelo pso
        PsoProblem psoProblem;
        //enxame
        public TSPParticleSwarm Swarm { get; protected set; }
        //posição da solução candidata
        TSPPosition candidate;
        //melhor indivíduo
        //TSPParticle gBest;

        int? MaxSteps;

        private TravellingSalesmanMap TSP;
        //public int ParticleCount { get; private set; }
        private double? TargetTravelDistance { get; set; }

        PSO()
        {
            //TODO
            //DEFINIR AS VARIÁVEIS
            //inicializar população
            //posição da solução candidata(default)
            //custo da função da solução candidata(default)
            //velocidade da partícula(default)
            //índice para a melhor posição global(default)
            //posição de melhor localização da i-ésima partícula(default)
            //melhor fitness local visitado pela i-ésima partícula(default)
        }

        public PSO(TravellingSalesmanMap TSP, int particleCount, int? maxSteps = null, double? targetTravelDistance = null) {
            this.TSP = TSP;
            this.Swarm = new TSPParticleSwarm(TSP, particleCount);
            this.MaxSteps = maxSteps;
            this.TargetTravelDistance = targetTravelDistance;
        }

        public Object Run()
        {
            this.Swarm.InitializeParticles();

		    int steps = 0;
		    Boolean solutionWasFound = false;

	        while(!solutionWasFound)
	        {
	            if((MaxSteps == null) || ((MaxSteps != null) && (steps < MaxSteps))) {
                    // Para cada partícula
                    foreach (TSPParticle particle in Swarm.Particles)
                    {
                        // Atualizar velocidade e posição de acordo com as equações do artigo
                        particle.UpdateSelf();

                        // Avaliar o fitness
                        particle.EvaluateSelf();

                        // Atualizar pBest caso a posição seja melhor que a anterior
                        //particle.UpdatePbest();
                    }

	                // Encontrar novo gBest
                    foreach (TSPParticle particle in Swarm.Particles)
                        if (particle.Fitness < Swarm.gBest.Fitness) {
                            Swarm.gBest = (TSPParticle)particle.Clone();
                        }

                    if ((TargetTravelDistance != null) && (Swarm.gBest.Fitness <= TargetTravelDistance))
                        solutionWasFound = true;

                    ++steps;
	            } else {
	                solutionWasFound = true;
	            }
		    }

            // Retornar a solução
            return SolutionFromParticle((TSPParticle)Swarm.gBest);
        }

        TSPSolution SolutionFromParticle(TSPParticle particle)
        {
            return new TSPSolution(((TSPPosition)particle.Position).Route, TSP);
        }
    }
}
