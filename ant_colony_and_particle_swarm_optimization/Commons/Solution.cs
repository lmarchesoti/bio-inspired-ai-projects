using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commons
{
    public class TSPSolution : ICloneable
    {
        public List<int> TravelPlan;
        public double Fitness;
        static TravellingSalesmanMap problem;

        public TSPSolution(List<int> steps, TravellingSalesmanMap problema)
        {
            this.TravelPlan = steps;
            problem = problema;
            Evaluate();
        }

        public TSPSolution()
        {
            this.Fitness = double.MaxValue;
            this.TravelPlan = new List<int>();
        }

        //método que avalia uma solução
        public void Evaluate() {
            double travelDistance = 0.0;
            for (int i = 0; i < problem.CityCount; ++i)
                travelDistance += problem.GetDistanceBetween(TravelPlan[i], TravelPlan[(i+1)%problem.CityCount]);
            this.Fitness = travelDistance;
        }

        //função que compara duas soluções
        public bool BetterThan(TSPSolution other)
        {
            return this.Fitness < other.Fitness;
        }

        public object Clone() {
            TSPSolution s = new TSPSolution(TravelPlan.TakeWhile(C => true).ToList(), problem);
            return s;
        }
    }
}
