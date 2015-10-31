using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Genetic_Algorithm_Commons.Problems.InstanceDescriptions;
using Genetic_Algorithm_Commons.Util;

namespace Genetic_Algorithm_Commons.Problems {
    public class CryptArithmeticProblem : ProblemBase {
        char[] charArray;
        string[] terms;
        string result;

        public CryptArithmeticProblem(CryptArithmeticInstanceDescription instanceDescriptor) {
            this.terms = instanceDescriptor.Terms;
            this.result = instanceDescriptor.Result;

            HashSet<char> charsInExpression = new HashSet<char>();
            foreach (string term in terms) {
                charsInExpression.UnionWith(term.ToCharArray());
            }
            charsInExpression.UnionWith(result.ToCharArray());
            charArray = charsInExpression.ToArray();
        }

        public override IndividualBase CreateRandomSolution() {
            IndividualBase cryptoSolution = new CryptArithmeticSolution(this);
            //EvaluateIndividual(cryptoSolution);
            return cryptoSolution;
        }

        public override void EvaluateIndividual(IndividualBase individual) {
            CryptArithmeticSolution cryptoSolution = (individual as CryptArithmeticSolution);
            long termsSum = 0;
            int charValue = -1;
            foreach (string term in terms) {
                long termCharSum = 0;
                long decimalBaseValTerm = 1;
                foreach (char c in term.ToCharArray().Reverse()) {
                    charValue = cryptoSolution.LettersValues[Array.IndexOf(charArray, c)];
                    termCharSum += (charValue * decimalBaseValTerm);
                    decimalBaseValTerm *= 10;
                }
                termsSum += termCharSum;
            }

            long resultCharSum = 0;
            long decimalBaseValResult = 1;
            foreach (char c in result.ToCharArray().Reverse()) {
                charValue = cryptoSolution.LettersValues[Array.IndexOf(charArray, c)];
                resultCharSum += (charValue * decimalBaseValResult);
                decimalBaseValResult *= 10;
            }
            long resultValue = resultCharSum;

            cryptoSolution.DifferenceInExpectedResult = (int)Math.Abs(resultValue - termsSum);
            individual = cryptoSolution;
        }

        public override string SerializeIndividual(IndividualBase individual) {
            StringBuilder SB = new StringBuilder();
            SB.AppendLine("Individual:");
            SB.AppendLine("\t" + "Value: " + individual.GetValueForObjective(MonoObjectiveGoal));
            CryptArithmeticSolution cryptoSolution = (individual as CryptArithmeticSolution);
            int charValue = -1;
            for (int idx = 0; idx < charArray.Length; ++idx) {
                charValue = cryptoSolution.LettersValues[idx];
                SB.AppendLine("\t" + charArray[idx] + ": " + charValue);
            }
            return SB.ToString();
        }

        public override void MutateIndividual(IndividualBase individual) {
            CryptArithmeticSolution cryptoSolution = (individual as CryptArithmeticSolution);
            int amountOfChars = charArray.Count();
            int firstPositionIdx = Aleatoriety.GetRandomInt(amountOfChars);
            int secondPositionIdx;
            do {
                secondPositionIdx = Aleatoriety.GetRandomInt(amountOfChars);
            } while (secondPositionIdx == firstPositionIdx);

            int tempValue = cryptoSolution.LettersValues[secondPositionIdx];
            cryptoSolution.LettersValues[secondPositionIdx] = cryptoSolution.LettersValues[firstPositionIdx];
            cryptoSolution.LettersValues[firstPositionIdx] = tempValue;
            individual = cryptoSolution;
        }

        public override void CiclicCrossover(IndividualBase parent1, IndividualBase parent2, out IndividualBase child1, out IndividualBase child2) {
            int[] L1 = (int[])(parent1 as CryptArithmeticSolution).LettersValues.Clone();
            int[] L2 = (int[])(parent2 as CryptArithmeticSolution).LettersValues.Clone();

            List<int> positionsToShare = new List<int>();

            int amountOfChars = charArray.Count();
            int itemIdx = Aleatoriety.GetRandomInt(amountOfChars);
            int tempValue;

            while (!positionsToShare.Contains(itemIdx)) {
                positionsToShare.Add(itemIdx);
                tempValue = L1[itemIdx];
                itemIdx = Array.IndexOf(L2, tempValue);
            }

            int tempVal;
            foreach (int position in positionsToShare) {
                tempVal = L2[position];
                L2[position] = L1[position];
                L1[position] = tempVal;
            }
            child1 = new CryptArithmeticSolution(this, L1);
            child2 = new CryptArithmeticSolution(this, L2);
        }

        public override void PMXCrossover(IndividualBase parent1, IndividualBase parent2, out IndividualBase child1, out IndividualBase child2) {
            throw new NotImplementedException();
        }

        public override void ValidateIndividual(IndividualBase individual) { }

        public override Objective DefineMonoObjetiveGeneticAlgorithmGoal() {
            return new Objective(typeof(CryptArithmeticSolution).GetProperty("DifferenceInExpectedResult"), Goal.Minimize, 0);
        }

        public override IEnumerable<Objective> DefineMultiObjetiveGeneticAlgorithmGoals() {
            return null;
        }

        public override string NewSerializeIndividual(IndividualBase individual) {
            StringBuilder SB = new StringBuilder();
            SB.AppendLine("Individual:");
            SB.AppendLine("\t" + "Value: " + individual.GetValueForObjective(MonoObjectiveGoal));
            CryptArithmeticSolution cryptoSolution = (individual as CryptArithmeticSolution);
            int charValue = -1;
            for (int idx = 0; idx < charArray.Length; ++idx)
            {
                charValue = cryptoSolution.LettersValues[idx];
                SB.AppendLine("\t" + charArray[idx] + ": " + charValue);
            }
            return SB.ToString();
        }
    }
}
