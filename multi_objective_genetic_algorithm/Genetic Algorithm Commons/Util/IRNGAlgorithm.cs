using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Genetic_Algorithm_Commons.Util {
    public interface IRNGAlgorithm {
        int Next(int exclusiveUpperBound);
        int Next(int inclusiveLowerBound, int exclusiveUpperBound);
    }
}
