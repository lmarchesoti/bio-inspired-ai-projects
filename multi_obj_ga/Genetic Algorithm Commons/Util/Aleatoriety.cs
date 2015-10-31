using System;
using System.Collections.Generic;
using System.Linq;
using Genetic_Algorithm_Commons.Util.RNGAlgorithms;

namespace Genetic_Algorithm_Commons.Util {
    public static class Aleatoriety {
        static IRNGAlgorithm Generator = new MersenneTwister();
        //static RNGAlgorithm Generator = new DotNetRandom();

        public static MersenneTwister MT = new MersenneTwister();

        public static int GetRandomInt(int inclusiveLowerBound, int exclusiveUpperBound) {
            return Generator.Next(inclusiveLowerBound, exclusiveUpperBound);
        }

        public static int GetRandomInt(int exclusiveUpperBound) {
            return Generator.Next(exclusiveUpperBound);
        }

        public static IEnumerable<int> GetRandomIntegerSequencePermutation(int inclusiveLowerBound, int exclusiveUpperBound) {
            return Enumerable.Range(inclusiveLowerBound, exclusiveUpperBound).OrderBy(n => Guid.NewGuid());
        }
    }
}
