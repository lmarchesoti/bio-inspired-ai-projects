using System.Collections.Generic;
using Genetic_Algorithm_Commons.Util;
using System.Linq;
using System.Collections;

namespace Genetic_Algorithm_Commons.Problems {
    public class TaskSchedulingSolution : IndividualBase {
        public int[,] GeneticMaterial { get; set; }
        public int[] ProcessorStatus { get; set; }

        public double MakeSpan { get; set; }
        public double SpentPower { get; set; } 

        public TaskSchedulingSolution(ProblemBase problem) : base(problem) {
            TaskSchedulingProblem schedulingProblem = (Problem as TaskSchedulingProblem);

            this.GeneticMaterial = new int[2, schedulingProblem.TaskCount];

            IEnumerable<int> randomTaskPermutation = Aleatoriety.GetRandomIntegerSequencePermutation(0, schedulingProblem.TaskCount);

            int idx = 0;
            foreach (int task in randomTaskPermutation) {
                this.GeneticMaterial[0, idx] = task;
                this.GeneticMaterial[1, idx] = Aleatoriety.GetRandomInt(schedulingProblem.ProcessorCount);
                ++idx;
            }
        }

        public TaskSchedulingSolution(ProblemBase problem, int[,] geneticMaterial) : base(problem) {
            TaskSchedulingProblem schedulingProblem = (Problem as TaskSchedulingProblem);

            this.GeneticMaterial = geneticMaterial;
        }

        internal void SwapGenesAt(int idx1, int idx2) {
            int[] tempGene = new int[2] { GeneticMaterial[0, idx1], GeneticMaterial[1, idx1] };
            GeneticMaterial[0, idx1] = GeneticMaterial[0, idx2];
            GeneticMaterial[1, idx1] = GeneticMaterial[1, idx2];
            GeneticMaterial[0, idx2] = tempGene[0];
            GeneticMaterial[1, idx2] = tempGene[1];
        }

        public void SwapProcessorAt(int index) {
            int mutationNodeProcessor = GeneticMaterial[1, index];
            int differentProcessor;
            do {
                differentProcessor = Aleatoriety.GetRandomInt(0, (Problem as TaskSchedulingProblem).ProcessorCount);
            } while (differentProcessor == mutationNodeProcessor);
            GeneticMaterial[1, index] = differentProcessor;
        }

        #region IComparable Members

        public override int CompareTo(object obj) {
            double valor = (obj as IndividualBase).GetFitnessForObjective(Problem.MonoObjectiveGoal);
            if (valor > GetFitnessForObjective(Problem.MonoObjectiveGoal))
                return -1;
            if (valor == GetFitnessForObjective(Problem.MonoObjectiveGoal))
                return 0;
            return 1;
        }

        #endregion

        public override bool Equals(object obj) {
            TaskSchedulingSolution scheduling = (obj as TaskSchedulingSolution);
            IEnumerator enumThis = GeneticMaterial.GetEnumerator();
            IEnumerator enumThat = scheduling.GeneticMaterial.GetEnumerator();
            while (enumThis.MoveNext() && enumThat.MoveNext()) {
                if (!int.Equals(enumThis.Current, enumThat.Current))
                    return false;
            }
            if (enumThis.MoveNext() == false && enumThat.MoveNext() == false)
                return true;
            return false;
        }
    }
}
