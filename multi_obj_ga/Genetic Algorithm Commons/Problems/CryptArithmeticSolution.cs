using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Genetic_Algorithm_Commons.Util;

namespace Genetic_Algorithm_Commons.Problems {
    public class CryptArithmeticSolution : IndividualBase {
        public int[] LettersValues;
        public int DifferenceInExpectedResult { get; set; }

        public CryptArithmeticSolution(ProblemBase problem) : base(problem) {
            int amountOfChars = Enumerable.Range(0, 10).Count();
            LettersValues = Aleatoriety.GetRandomIntegerSequencePermutation(0, amountOfChars).ToArray();
        }

        public CryptArithmeticSolution(ProblemBase problem, int[] lettersValues) : base(problem) {
            this.LettersValues = lettersValues;
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
            CryptArithmeticSolution cryptoSolution = (obj as CryptArithmeticSolution);
            return LettersValues.SequenceEqual(cryptoSolution.LettersValues);
        }
    }
}
