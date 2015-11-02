using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Genetic_Algorithm_Commons.Problems;
using Genetic_Algorithm_Commons.Problems.InstanceDescriptions;
using Genetic_Algorithm_Commons;

namespace Genetic_Algorithm_Commons_Test {
    /// <summary>
    /// Classe para Testes unitários referentes ao projeto 'Genetic Algorithm Commons'
    /// </summary>
    [TestClass]
    public class UnitTests {
        public UnitTests() {  }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext {
            get {
                return testContextInstance;
            }
            set {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void Individual_Equals_Test() {
            TaskSchedulingProblem Gauss18 = new TaskSchedulingProblem(TaskSchedulingInstanceDescription.Gauss18(2));

            TaskSchedulingSolution thisObj = new TaskSchedulingSolution(
                Gauss18,
                new int[2, 18] { 
                    {0, 5, 1, 2, 6, 10, 7, 3, 8, 11, 12, 4, 9, 13, 15, 16, 14, 17}, 
                    {1, 0, 0, 1, 1, 0, 0, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1}
                }
            );

            TaskSchedulingSolution thisObjAgain = new TaskSchedulingSolution(
                Gauss18,
                new int[2, 18] { 
                    {0, 5, 1, 2, 6, 10, 7, 3, 8, 11, 12, 4, 9, 13, 15, 16, 14, 17}, 
                    {1, 0, 0, 1, 1, 0, 0, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1}
                }
            );

            TaskSchedulingSolution thatObj = new TaskSchedulingSolution(
                Gauss18,
                new int[2, 18] { 
                    {0, 5, 1, 2, 6, 10, 7, 3, 8, 11, 12, 4, 9, 13, 15, 16, 14, 18}, 
                    {1, 0, 0, 1, 1, 0, 0, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1}
                }
            );
            Assert.AreEqual(thisObj, thisObjAgain);
            Assert.AreNotEqual(thisObj, thatObj);
        }

        [TestMethod]
        public void Problem_EvaluateIndividual_Test() {
            /*
            TaskSchedulingProblem Gauss18 = new TaskSchedulingProblem(TaskSchedulingInstanceDescription.Gauss18(2));

            TaskSchedulingSolution perfectSolution = new TaskSchedulingSolution(
                Gauss18,
                18,
                2,
                Gauss18.CommGraph,
                new List<TaskSchedulingSolution.SchedulingNode>() {
                    new TaskSchedulingSolution.SchedulingNode() { Task =  0, Processor = 1, ProcessorStatus = new int[2] },
                    new TaskSchedulingSolution.SchedulingNode() { Task =  5, Processor = 0, ProcessorStatus = new int[2] },
                    new TaskSchedulingSolution.SchedulingNode() { Task =  1, Processor = 0, ProcessorStatus = new int[2] },
                    new TaskSchedulingSolution.SchedulingNode() { Task =  2, Processor = 1, ProcessorStatus = new int[2] },
                    new TaskSchedulingSolution.SchedulingNode() { Task =  6, Processor = 1, ProcessorStatus = new int[2] },
                    new TaskSchedulingSolution.SchedulingNode() { Task =  10, Processor = 0, ProcessorStatus = new int[2] },
                    new TaskSchedulingSolution.SchedulingNode() { Task =  7, Processor = 0, ProcessorStatus = new int[2] },
                    new TaskSchedulingSolution.SchedulingNode() { Task =  3, Processor = 1, ProcessorStatus = new int[2] },
                    new TaskSchedulingSolution.SchedulingNode() { Task =  8, Processor = 1, ProcessorStatus = new int[2] },
                    new TaskSchedulingSolution.SchedulingNode() { Task =  11, Processor = 1, ProcessorStatus = new int[2] },
                    new TaskSchedulingSolution.SchedulingNode() { Task =  12, Processor = 0, ProcessorStatus = new int[2] },
                    new TaskSchedulingSolution.SchedulingNode() { Task =  4, Processor = 1, ProcessorStatus = new int[2] },
                    new TaskSchedulingSolution.SchedulingNode() { Task =  9, Processor = 1, ProcessorStatus = new int[2] },
                    new TaskSchedulingSolution.SchedulingNode() { Task =  13, Processor = 1, ProcessorStatus = new int[2] },
                    new TaskSchedulingSolution.SchedulingNode() { Task =  15, Processor = 1, ProcessorStatus = new int[2] },
                    new TaskSchedulingSolution.SchedulingNode() { Task =  16, Processor = 1, ProcessorStatus = new int[2] },
                    new TaskSchedulingSolution.SchedulingNode() { Task =  14, Processor = 1, ProcessorStatus = new int[2] },
                    new TaskSchedulingSolution.SchedulingNode() { Task =  17, Processor = 1, ProcessorStatus = new int[2] }
                }
            );

            Gauss18.EvaluateIndividual(perfectSolution);
            Assert.AreEqual(44.0, perfectSolution.MakeSpan);
            Assert.AreEqual(691.2, perfectSolution.SpentPower);
            */
        }

        [TestMethod]
        public void StaticProblem_EvaluateIndividual_Test() {
            TaskSchedulingProblem Gauss18 = new TaskSchedulingProblem(TaskSchedulingInstanceDescription.Gauss18(2));

            TaskSchedulingSolution solution = new TaskSchedulingSolution(
                Gauss18,
                new int[2, 18] { 
                    {0, 5, 1, 2, 6, 10, 7, 3, 8, 11, 12, 4, 9, 13, 15, 16, 14, 17}, 
                    {1, 0, 0, 1, 1, 0, 0, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1}
                }
            );

            Gauss18.ValidateIndividual(solution);
            Gauss18.EvaluateIndividual(solution);
            Assert.AreEqual(44.0, solution.MakeSpan);
            Assert.AreEqual(691.2, solution.SpentPower);

            solution = new TaskSchedulingSolution(
                Gauss18,
                new int[2, 18] { 
                    {8, 5, 1, 2, 6, 10, 7, 3, 0, 11, 12, 4, 9, 13, 15, 16, 14, 17}, 
                    {1, 0, 0, 1, 1, 0, 0, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1}
                }
            );

            Gauss18.ValidateIndividual(solution);
            Gauss18.EvaluateIndividual(solution);
            Assert.AreEqual(48.0, solution.MakeSpan);
            Assert.AreEqual(691.2, solution.SpentPower);

            TaskSchedulingSolution solutionWithDeadLock = new TaskSchedulingSolution(
                Gauss18,
                new int[2, 18] { 
                    {1, 10, 7, 8, 12, 0, 2, 6, 3, 11, 5, 4, 9, 13, 15, 16, 14, 17}, 
                    {0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1}
                }
            );

            Gauss18.ValidateIndividual(solutionWithDeadLock);
            Gauss18.EvaluateIndividual(solutionWithDeadLock);
        }
    }
}
