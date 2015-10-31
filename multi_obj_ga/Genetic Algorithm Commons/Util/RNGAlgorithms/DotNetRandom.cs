using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Genetic_Algorithm_Commons.Util.RNGAlgorithms {
    public class DotNetRandom : IRNGAlgorithm {
        static Random RandomSeed = new Random();

        public int Next(int exclusiveUpperBound) {
            return RandomSeed.Next(exclusiveUpperBound);
        }

        public int Next(int inclusiveLowerBound, int exclusiveUpperBound) {
            return RandomSeed.Next(inclusiveLowerBound, exclusiveUpperBound);
        }
    }
}
