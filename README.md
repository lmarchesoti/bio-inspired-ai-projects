# bio-inspired-ai-projects

This is a collection of bio-inspired AI projects I took as an undergrad. They are mainly intended as a backup, but anyone in need is welcome to benefit from them. Specific descriptions follow in turn.

---

## task_scheduling_genetic_algorithm
Genetic Algorithm for Task Scheduling Problem

This is a c++ genetic algorithm aimed at solving the task scheduling problem.

**Compilation**

Make targets include:

*compile*

*run*: runs single instance. Running in non-batch mode gives additional info about the best subject found.

*batch*: runs in batch mode. Don't forget to activate batch mode in the *include/parameters.h* file and to check *lib/batch.sh* file for preferences. Returns only the best solution score.

*clean*

*all*

---

## genetic_algorithm_framework
Attempt at a Genetic Algorithm Framework in Ruby

Supports different reinsertion, selection and crossover methods, as well as loading individual instances from text files and a parameter configuration through the *app/default_parameters.rb* file. Example implemented problem is crypto-arithmetic.

#### Supported methods

**reinsertion**: best, elitism

**selection**: tourney, roulette

**crossover**: cyclic, pmx

---

I'm not a .NET guy, but I contributed heavily in this project for an Artificial Intelligence course taken together with the Master's program as a grad student. These involve some pretty advanced algorithms in biologically-inspired artificial intelligence.

## multi_objective_genetic_algorithm

Multi-objective Genetic Algorithm

This project includes implementations for mono-objective crypto-arithmetic and multi-objective task scheduling using NSGA2 and SPEA2 approaches.

---

## ant_colony_and_particle_swarm_optimization

Ant Colony and Particle Swarm Optimization for Traveling Salesman Problem

This project compares both methods in solving the TSP.
