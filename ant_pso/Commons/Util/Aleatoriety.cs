using System;
using System.Collections.Generic;
using System.Linq;
using Commons.Util.RNGAlgorithms;

namespace Commons.Util {
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

        public static double GetRandomDouble() {
            return Generator.NextDouble();
        }

        public static IEnumerable<int> GetRandomIntegerSequencePermutation(int inclusiveLowerBound, int exclusiveUpperBound) {
            return Enumerable.Range(inclusiveLowerBound, exclusiveUpperBound).OrderBy(n => Guid.NewGuid());
        }

        public static Tuple<int, int> GetTwoDiferentValuesInRange(int inclusiveLowerBound, int exclusiveUpperBound) {
            int value1 = Generator.Next(inclusiveLowerBound, exclusiveUpperBound);
            int value2 = Generator.Next(inclusiveLowerBound, exclusiveUpperBound);
            while (value2 == value1) {
                value2 = Generator.Next(inclusiveLowerBound, exclusiveUpperBound);
            }
            return new Tuple<int, int>(value1, value2);
        }
    }
}
