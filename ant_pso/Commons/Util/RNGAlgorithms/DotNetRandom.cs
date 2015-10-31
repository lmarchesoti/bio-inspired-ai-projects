using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commons.Util.RNGAlgorithms {
    public class DotNetRandom : IRNGAlgorithm {
        static Random RandomSeed = new Random();

        public int Next(int exclusiveUpperBound) {
            return RandomSeed.Next(exclusiveUpperBound);
        }

        public int Next(int inclusiveLowerBound, int exclusiveUpperBound) {
            return RandomSeed.Next(inclusiveLowerBound, exclusiveUpperBound);
        }

        public double NextDouble() {
            return RandomSeed.NextDouble();
        }
    }
}
