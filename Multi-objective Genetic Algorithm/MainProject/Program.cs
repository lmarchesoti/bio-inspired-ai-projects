using System;
using System.Linq;
using Genetic_Algorithm_Commons;
using Genetic_Algorithm_Commons.Problems;
using Genetic_Algorithm_Commons.Problems.InstanceDescriptions;
using Mono_Objective_Genetic_Algorithm;
using Mono_Objective_Genetic_Algorithm.Crossover_Methods;
using Mono_Objective_Genetic_Algorithm.Reinsertion_Methods;
using Mono_Objective_Genetic_Algorithm.Selection_Methods;
using System.Collections.Generic;
using Multi_Objective_Genetic_Algorithm;
using Visual_Solutions_Plotter;

namespace MainProject {
    class Program {
        static void Main(string[] args) {
            ProblemBase mainProblem;
            //MONO
            
            //mainProblem = new CryptArithmeticProblem(CryptArithmeticInstanceDescription.CocaColaOasis);
            mainProblem = new CryptArithmeticProblem(CryptArithmeticInstanceDescription.SendMoreMoney);
            List<IndividualBase> paretoSolutions;
            string convergenceReport = "";
            paretoSolutions = MonoObjectiveGeneticAlgorithmTest(mainProblem, convergenceReport);
            foreach(IndividualBase i in paretoSolutions){
                Console.Write(i.ToString());
            }
            Console.ReadKey();
            
            /*
            //MULTI
            mainProblem = new TaskSchedulingProblem(TaskSchedulingInstanceDescription.ClassExample(2));
            //mainProblem = new TaskSchedulingProblem(TaskSchedulingInstanceDescription.Gauss18(2));
            
            string convergenceReport = string.Empty;

            IEnumerable<IndividualBase> paretoSolutions;

            //paretoSolutions = SPEA2MultiExecution(1, mainProblem, 20);
            paretoSolutions = NSGA2Test(mainProblem);

            
            IEnumerable<Tuple<double, double>> dots =
                from i in paretoSolutions
                select new Tuple<double, double> (i.GetValueForObjective(mainProblem.MultiObjectiveGoal.First()), i.GetValueForObjective(mainProblem.MultiObjectiveGoal.Last()));

            int minX = 0;
            int maxX = 50;
            int minY = 0;
            int maxY = 400;

            Plotter.DisplayMapping(minX, maxX, minY, maxY, dots);
               */
                //.MainForm(minX, ma)
            /*
            foreach (IndividualBase pareto in paretoSolutions) {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Tree15-4proc-5ex-10mut.txt", true)) {
                    file.WriteLine(pareto.ToString());
                }  
            }
            */
        }

        private static List<IndividualBase> MonoObjectiveGeneticAlgorithmTest(ProblemBase mainProblem, string convergenceReport)
        {
            MonoObjectiveGeneticAlgorithm AG = new MonoObjectiveGeneticAlgorithm(
                mainProblem,
                new Tour(3),
                new Ciclic(),
                new BestAmongstAll(100),
                200,
                200,
                30);
            List<IndividualBase> retorno = new List<IndividualBase>();
            retorno.Add(AG.Execute());
            return retorno;
        }

        private static IEnumerable<IndividualBase> SPEA2MultiExecution(int executionCount, ProblemBase problem, int populationSize) {
            MultiObjectiveGeneticAlgorithm SPEA2 = new NSGA2(
               problem,
               populationSize,
               200,
               200,
               10);

            Population_MultiObjective_AG Aggregator = new Population_MultiObjective_AG(problem);

            for (int i = 0; i < executionCount; ++i) {
                IEnumerable<IndividualBase> executionResult = SPEA2.Execute();
                Aggregator.AddRange(executionResult);
            }

            IEnumerable<IndividualBase> nonDominatedFront = Aggregator.FastNonDominatedSort().First().Content;
            IEnumerable<IndividualBase> firstItemsByMakeSpan = (from i in nonDominatedFront.Cast<TaskSchedulingSolution>()
                                                                group i by i.MakeSpan into g
                                                                select g.First()).OrderBy(I => I.MakeSpan);

            return firstItemsByMakeSpan;
        }

        private static IEnumerable<IndividualBase> SPEA2Test(ProblemBase mainProblem) {
            MultiObjectiveGeneticAlgorithm SPEA2 = new SPEA2(
               mainProblem,
               200,
               200,
               400,
               10);

            return SPEA2.Execute();
        }

        private static IEnumerable<IndividualBase> NSGA2Test(ProblemBase mainProblem) {
            MultiObjectiveGeneticAlgorithm NSGA2 = new NSGA2(
               mainProblem,
               200,
               200,
               400,
               2);

            return NSGA2.Execute();
        }

        private static List<IndividualBase> MonoObjectiveGeneticAlgorithmTest(ProblemBase mainProblem, out string convergenceReport) {
            MonoObjectiveGeneticAlgorithm AG = new MonoObjectiveGeneticAlgorithm(
                mainProblem,
                new Tour(1),
                new PMX(),
                new Elitist(30),
                200,
                200,
                15);
            List<IndividualBase> retorno = new List<IndividualBase>();
            retorno.Add(AG.Execute(100, out convergenceReport));
            return retorno;
        }
    }
}
