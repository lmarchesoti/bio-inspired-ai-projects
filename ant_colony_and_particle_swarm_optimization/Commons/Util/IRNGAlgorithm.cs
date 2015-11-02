using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commons.Util {
    public interface IRNGAlgorithm {
        int Next(int exclusiveUpperBound);
        int Next(int inclusiveLowerBound, int exclusiveUpperBound);
        double NextDouble();
    }
}
