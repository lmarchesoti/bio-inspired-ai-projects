
using System.Reflection;
namespace Genetic_Algorithm_Commons {
    public class Objective {
        public Goal Goal { get; private set; }
        public PropertyInfo Attribute { get; private set; }
        public int Index { get; private set; }
        public double BigValueForMaximization { get; set; }

        public Objective(PropertyInfo attribute, Goal goal, int index) {
            this.Attribute = attribute;
            this.Goal = goal;
            this.Index = index;
        }
    }
}
