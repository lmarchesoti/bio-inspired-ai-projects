using Genetic_Algorithm_Commons;

namespace Genetic_Algorithm_Commons.Problems.InstanceDescriptions {
    public class CryptArithmeticInstanceDescription : ProblemInstanceDescriptionBase {
        public string[] Terms { get; private set; }
        public string Result { get; private set; }

        public CryptArithmeticInstanceDescription(string[] terms, string result) {
            this.Terms = terms;
            this.Result = result;
            base.OptimalIndividualValue = null;
        }

        public static CryptArithmeticInstanceDescription CocaColaOasis {
            get {
                return new CryptArithmeticInstanceDescription(
                    new string[] { "COCA", "COLA" },
                    "OASIS"                    
                    );
            }
        }

        public static CryptArithmeticInstanceDescription SendMoreMoney {
            get {
                return new CryptArithmeticInstanceDescription(
                    new string[] { "SEND", "MORE" },
                    "MONEY"
                    );
            }
        }
    }
}
