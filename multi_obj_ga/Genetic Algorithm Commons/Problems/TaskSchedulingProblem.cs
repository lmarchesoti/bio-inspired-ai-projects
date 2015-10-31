using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Genetic_Algorithm_Commons.Problems.InstanceDescriptions;
using Genetic_Algorithm_Commons.Util;
using System.Reflection;


namespace Genetic_Algorithm_Commons.Problems {
    public class TaskSchedulingProblem : ProblemBase {
        public CommunicationGraph CommGraph { get; protected set; }
        public int TaskCount { get; set; }
        public int ProcessorCount { get; set; }
        const double k1 = 10;
        const double k2 = 0.1;

        class TaskIndexingNode {
            public int Task { get; set; }
            public int Index { get; set; }

            public TaskIndexingNode(int task, int index) {
                Task = task;
                Index = index;
            }
        };

        public TaskSchedulingProblem(TaskSchedulingInstanceDescription instanceDescriptor) {
            this.CommGraph = instanceDescriptor.CommGraph;
            this.TaskCount = instanceDescriptor.TaskCount;
            this.ProcessorCount = instanceDescriptor.ProcessorCount;
        }

        public override IndividualBase CreateRandomSolution() {
            IndividualBase schedulingSolution = new TaskSchedulingSolution(this);
            ValidateIndividual(schedulingSolution);
            // Random Solutions do not need Validation in this case.
            return schedulingSolution;
        }

        // TODO: Testing
        public override void ValidateIndividual(IndividualBase individual) {
            TaskSchedulingSolution schedulingSolution = (individual as TaskSchedulingSolution);

            List<List<TaskIndexingNode>> TasksGroupedByProcessor = GroupTasksByProcessor(schedulingSolution);

            for (int processorIdx = 0; processorIdx < ProcessorCount; ++processorIdx) {
                List<TaskIndexingNode> TasksInProcessor = TasksGroupedByProcessor[processorIdx];
                for (int probableDependantIdx = 0; probableDependantIdx < TasksInProcessor.Count; ++probableDependantIdx) {
                    int probableDependantTask = TasksInProcessor[probableDependantIdx].Task;
                    for (int probableDependencyIdx = probableDependantIdx + 1; probableDependencyIdx < TasksInProcessor.Count; ++probableDependencyIdx) {
                        int probableDependencyTask = TasksInProcessor[probableDependencyIdx].Task;
                        if(CommGraph.DependsOn(probableDependantTask, probableDependencyTask)) {
                            schedulingSolution.SwapGenesAt(TasksInProcessor[probableDependantIdx].Index, TasksInProcessor[probableDependencyIdx].Index);
                            TasksInProcessor.Swap(probableDependantIdx, probableDependencyIdx);
                            int Temp = TasksInProcessor[probableDependantIdx].Index;
                            TasksInProcessor[probableDependantIdx].Index = TasksInProcessor[probableDependencyIdx].Index;
                            TasksInProcessor[probableDependencyIdx].Index = Temp;
                            probableDependantIdx = -1;
                            break;
                        }
                    }
                }
            }
        }

        private List<List<TaskIndexingNode>> GroupTasksByProcessor(TaskSchedulingSolution schedulingSolution) {
            List<List<TaskIndexingNode>> tasksGroupedByProcessor = new List<List<TaskIndexingNode>>();

            for (int pCount = 0; pCount < ProcessorCount; ++pCount)
                tasksGroupedByProcessor.Add(new List<TaskIndexingNode>());

            for (int idx = 0; idx < TaskCount; ++idx) {
                int Processor = schedulingSolution.GeneticMaterial[1, idx];
                int Task = schedulingSolution.GeneticMaterial[0, idx];
                tasksGroupedByProcessor[Processor].Add(new TaskIndexingNode(Task, idx));
            }
            return tasksGroupedByProcessor;
        }

        public override void EvaluateIndividual(IndividualBase individual) {
            REDO:

            TaskSchedulingSolution schedulingSolution = individual as TaskSchedulingSolution;
            schedulingSolution.SpentPower = 0;
            double pCommunication = 0;
            int pTask = 0;

            bool[] scheduledTasks = new bool[TaskCount];
            int remainingTasks = TaskCount;
            int[] processorsSchedullingTimeForTask = new int[TaskCount];
            schedulingSolution.ProcessorStatus = new int[ProcessorCount];
            int currentAllocationCost;

            List<List<TaskIndexingNode>> tasksGroupedByProcessor = GroupTasksByProcessor(schedulingSolution);

            bool deadLockOcurred;
            do {
                for (int currentProcessor = 0; currentProcessor < ProcessorCount; ++currentProcessor) {
                    if (tasksGroupedByProcessor[currentProcessor].Count != 0) {
                        int nextTaskForProcessor = tasksGroupedByProcessor[currentProcessor][0].Task;
                        IEnumerable<int> notScheduledTaskDependencies = CommGraph.GetDirectDependencies(nextTaskForProcessor).Where(Dep => scheduledTasks[Dep] == false);
                        bool readyForScheduling = (notScheduledTaskDependencies.Count() == 0);
                        if (readyForScheduling) {
                            int currentTask = nextTaskForProcessor;
                            currentAllocationCost = schedulingSolution.ProcessorStatus[currentProcessor];
                            foreach (int dependencyTask in CommGraph.GetDirectDependencies(currentTask)) {
                                int idx = 0;
                                while (schedulingSolution.GeneticMaterial[0, idx] != dependencyTask)
                                    ++idx;
                                int dependencyProcessor = schedulingSolution.GeneticMaterial[1, idx];
                                if (dependencyProcessor != currentProcessor) {
                                    pCommunication += Math.Pow(CommGraph.GetCommunicationCost(dependencyTask, currentTask), 2);
                                    int dependencyCost = processorsSchedullingTimeForTask[dependencyTask] + CommGraph.GetCommunicationCost(dependencyTask, currentTask);
                                    if (dependencyCost > currentAllocationCost)
                                        currentAllocationCost = dependencyCost;
                                }
                            }
                            currentAllocationCost += CommGraph.GetEdgeCost(currentTask);
                            schedulingSolution.ProcessorStatus[currentProcessor] = currentAllocationCost;
                            processorsSchedullingTimeForTask[currentTask] = currentAllocationCost;
                            pTask += CommGraph.GetEdgeCost(currentTask);
                            scheduledTasks[currentTask] = true;
                            tasksGroupedByProcessor[currentProcessor].RemoveAt(0);
                            remainingTasks--;
                            currentProcessor = -1;
                        }
                    }
                }
                if (remainingTasks != 0) {
                    deadLockOcurred = true;

                    // Tratamento de DeadLock
                    int chosenSourceProcessor;
                    do {
                        chosenSourceProcessor = Aleatoriety.GetRandomInt(ProcessorCount);
                    } while (tasksGroupedByProcessor[chosenSourceProcessor].Count == 0);
                    int chosenDestProcessor;
                    do {
                        chosenDestProcessor = Aleatoriety.GetRandomInt(ProcessorCount);
                    } while (chosenSourceProcessor == chosenDestProcessor);
                    int taskToBeTransfered = tasksGroupedByProcessor[chosenSourceProcessor].First().Task;
                    int idx = 0;
                    while (schedulingSolution.GeneticMaterial[0, idx] != taskToBeTransfered)
                        ++idx;
                    schedulingSolution.GeneticMaterial[1, idx] = chosenDestProcessor;
                    ValidateIndividual(schedulingSolution);
                    goto REDO;
                }
                else
                    deadLockOcurred = false;
            } while (deadLockOcurred);
            // Houve DeadLock

            schedulingSolution.MakeSpan = schedulingSolution.ProcessorStatus.Max();
            schedulingSolution.SpentPower = (pTask * k1) + (pCommunication * k2);
        }

        public override string SerializeIndividual(IndividualBase individual) {
            TaskSchedulingSolution schedulingSolution = (individual as TaskSchedulingSolution);

            List<List<TaskIndexingNode>> tasksGroupedByProcessor = GroupTasksByProcessor(schedulingSolution);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Individual:");
            sb.AppendLine("\t" + "Value: " + individual.GetValueForObjective(MonoObjectiveGoal));
            for (int processorIdx = 0; processorIdx < ProcessorCount; ++processorIdx) {
                sb.Append("\t" + "P" + processorIdx + ": " + "[");
                foreach (int task in tasksGroupedByProcessor[processorIdx].Select(TIN => TIN.Task)) {
                    sb.Append(" " + task + " |");
                }
                sb.Append("| " + schedulingSolution.ProcessorStatus[processorIdx] + " ]");
                sb.Append(Environment.NewLine);
            }
            return sb.ToString();
        }

        public override string NewSerializeIndividual(IndividualBase individual) {
            TaskSchedulingSolution schedulingSolution = (individual as TaskSchedulingSolution);

            List<List<TaskIndexingNode>> tasksGroupedByProcessor = GroupTasksByProcessor(schedulingSolution);

            StringBuilder sb = new StringBuilder();
            sb.Append("MakeSpan: " + individual.GetValueForObjective(MultiObjectiveGoal.First()) + " _ ");
            sb.Append("Potencia: " + individual.GetValueForObjective(MultiObjectiveGoal.Last()) + " _ ");

            for (int idx = 0; idx < TaskCount - 1; ++idx)
                sb.Append(" " + schedulingSolution.GeneticMaterial[0, idx] + " (" + schedulingSolution.GeneticMaterial[1, idx] + ") -");
            sb.Append(" " + schedulingSolution.GeneticMaterial[0, TaskCount - 1] + " (" + schedulingSolution.GeneticMaterial[1, TaskCount - 1] + ")");
            return sb.ToString();
        }

        public override void MutateIndividual(IndividualBase individual) {
            TaskSchedulingSolution schedulingSolution = (individual as TaskSchedulingSolution);
            int indexA = Aleatoriety.GetRandomInt(TaskCount);
            int indexB;
            do {
                indexB = Aleatoriety.GetRandomInt(TaskCount);
            } while (indexB == indexA);
            schedulingSolution.SwapGenesAt(indexA, indexB);
            schedulingSolution.SwapProcessorAt(indexA);
        }
        /*
        public override void CiclicCrossover(IndividualBase parent1, IndividualBase parent2, out IndividualBase child1, out IndividualBase child2) {
            List<StaticTaskSchedulingSolution.SchedulingNode> L1 = new List<StaticTaskSchedulingSolution.SchedulingNode>();
            foreach(StaticTaskSchedulingSolution.SchedulingNode schedulingNode in (parent1 as StaticTaskSchedulingSolution).Scheduling) {
                L1.Add(new StaticTaskSchedulingSolution.SchedulingNode() { Task = schedulingNode.Task, Processor = schedulingNode.Processor });
            }

            List<StaticTaskSchedulingSolution.SchedulingNode> L2 = new List<StaticTaskSchedulingSolution.SchedulingNode>();
            foreach (StaticTaskSchedulingSolution.SchedulingNode schedulingNode in (parent2 as StaticTaskSchedulingSolution).Scheduling) {
                L2.Add(new StaticTaskSchedulingSolution.SchedulingNode() { Task = schedulingNode.Task, Processor = schedulingNode.Processor });
            }

            List<int> positionsToShare = new List<int>();

            int itemIdx = Aleatoriety.GetRandomInt(TaskCount);
            int tempValue;

            while (!positionsToShare.Contains(itemIdx)) {
                positionsToShare.Add(itemIdx);
                tempValue = L1[itemIdx].Task;
                itemIdx = L2.FindIndex(N => N.Task == tempValue);
            }

            StaticTaskSchedulingSolution.SchedulingNode tempVal;
            foreach (int position in positionsToShare) {
                tempVal = L2[position];
                L2[position] = L1[position];
                L1[position] = tempVal;
            }

            child1 = new StaticTaskSchedulingSolution(this, TaskCount, ProcessorCount, CommGraph, L1);
            child2 = new StaticTaskSchedulingSolution(this, TaskCount, ProcessorCount, CommGraph, L2);
            //ValidateIndividual(child1);
            //ValidateIndividual(child2);
        }
        */

        public override Objective DefineMonoObjetiveGeneticAlgorithmGoal() {
            return new Objective(typeof(TaskSchedulingSolution).GetProperty("MakeSpan"), Goal.Minimize, 0);
        }

        public override IEnumerable<Objective> DefineMultiObjetiveGeneticAlgorithmGoals() {
            Objective firstObjective = new Objective(typeof(TaskSchedulingSolution).GetProperty("MakeSpan"), Goal.Minimize, 0);
            Objective secondObjective = new Objective(typeof(TaskSchedulingSolution).GetProperty("SpentPower"), Goal.Minimize, 1);
            return new Objective[] {firstObjective, secondObjective};
        }

        public override void CiclicCrossover(IndividualBase parent1, IndividualBase parent2, out IndividualBase child1, out IndividualBase child2) {
            throw new NotImplementedException();
        }

        public override void PMXCrossover(IndividualBase parent1, IndividualBase parent2, out IndividualBase child1, out IndividualBase child2) {
            int[,] parent1Scheduling = (parent1 as TaskSchedulingSolution).GeneticMaterial;
            int[,] parent2Scheduling = (parent2 as TaskSchedulingSolution).GeneticMaterial;

            int startIdxInclusive = Aleatoriety.GetRandomInt(TaskCount - 1);
            int endIdxExclusive = Aleatoriety.GetRandomInt(startIdxInclusive + 1, TaskCount + 1);

            int[,] L1 = new int[2, TaskCount];
            int[,] L2 = new int[2, TaskCount];

            int L1idx;
            int L2idx;

            for (int idx = 0; idx < TaskCount; ++idx) {
                if ((idx < startIdxInclusive) || (idx >= endIdxExclusive)) {
                    int L1CopyFromTask = parent1Scheduling[0, idx];
                    int L1CopyFromProcessor = parent1Scheduling[1, idx];
                    while ((L1idx = IndexOfTaskInInterval(L1CopyFromTask, startIdxInclusive, endIdxExclusive, parent2Scheduling)) != -1) {
                        L1CopyFromTask = parent1Scheduling[0, L1idx];
                        L1CopyFromProcessor = parent1Scheduling[1, L1idx];
                    }
                    L1[0, idx] = L1CopyFromTask;
                    L1[1, idx] = L1CopyFromProcessor;

                    int L2CopyFromTask = parent2Scheduling[0, idx];
                    int L2CopyFromProcessor = parent2Scheduling[1, idx];
                    while ((L2idx = IndexOfTaskInInterval(L2CopyFromTask, startIdxInclusive, endIdxExclusive, parent1Scheduling)) != -1) {
                        L2CopyFromTask = parent2Scheduling[0, L2idx];
                        L2CopyFromProcessor = parent2Scheduling[1, L2idx];
                    }
                    L2[0, idx] = L2CopyFromTask;
                    L2[1, idx] = L2CopyFromProcessor;
                }
                else {
                    L1[0, idx] = parent2Scheduling[0, idx];
                    L1[1, idx] = parent2Scheduling[1, idx];
                    L2[0, idx] = parent1Scheduling[0, idx];
                    L2[1, idx] = parent1Scheduling[1, idx];
                }
            }

            child1 = new TaskSchedulingSolution(this, L1);
            child2 = new TaskSchedulingSolution(this, L2);
        }

        private int IndexOfTaskInInterval(int L1CopyFromTask, int startIdxInclusive, int endIdxExclusive, int[,] parent2Scheduling) {
            for(int idx = startIdxInclusive; idx < endIdxExclusive; ++idx) {
                if(parent2Scheduling[0, idx] == L1CopyFromTask)
                    return idx;
            }
            return -1;
        }
    }
}
