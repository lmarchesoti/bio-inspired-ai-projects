require_relative 'population'

class GeneticAlgorithm

	# POPULATION
	attr_accessor :population
	attr_accessor :population_size
	# REINSERTION
	attr_accessor :reinsertion_method
	attr_accessor :reinsertion_args
	# SELECTION
	attr_accessor :selection_method
	attr_accessor :selection_args
	# CROSSOVER
	attr_accessor :crossover_rate
	attr_reader :offspring_size
	attr_accessor :crossover_method
	# MUTATION
	attr_accessor :mutation_rate
	attr_reader :mutation_number
	# NUMBER OF GENERATIONS
	attr_accessor :generations
	# PROBLEM INSTANCE
	attr_accessor :instance

	def initialize args = {}
		@population_size = args.fetch(:population_size, 0)
		@crossover_rate = args.fetch(:crossover_rate, 0)
		@mutation_rate = args.fetch(:mutation_rate, 0)
		@generations = args.fetch(:generations, 0)
		@instance = args.fetch(:instance, 'default')
		@reinsertion_method = args.fetch(:reinsertion, 'best')
		@reinsertion_args = args.fetch(:reinsertion_args, {})
		@selection_method = args.fetch(:selection, 'tourney')
		@selection_args = args.fetch(:selection_args, {})
		@crossover_method = args.fetch(:crossover, '')
	end

	def crossover_phase!
		offspring = generate_offspring
		@population.reinsert! offspring
	end

	def generate_offspring
		offspring = []
		while offspring.length < @offspring_size do
			offspring += match_and_cross
		end
		return offspring[0, @offspring_size]
	end

	def mutation_phase!
		@mutation_number.times do
			@population.individuals.sample.mutate!
		end
		@population.individuals.sort!
	end

	def match_and_cross
		(@population.select).cross @population.select
	end

	def generation!
		crossover_phase!
		mutation_phase!
	end

	def run!
		preparation_phase!
		@generations.times do
			generation!
			if goal?
				break
			end
		end
		@population.individuals.first
	end

	def preparation_phase!
		@offspring_size = @population_size * @crossover_rate / 100
		@mutation_number = @population_size * @mutation_rate / 100
		Individual.load_instance @instance
		Individual.crossover_method = @crossover_method
		@population = Population.new size: @population_size,
		 reinsertion: @reinsertion_method, reinsertion_args: @reinsertion_args,
		 selection: @selection_method, selection_args: @selection_args
	end

	def batch iterations
		final = 0
		iterations.times do
			run!
			if goal?
				final += 1
			end
		end
		final
	end

	def goal?
		@population.individuals.first.fitness == 0
	end

	def inspect
		puts "Genetic Algorithm"
		puts "Population size: " + @population_size.to_s
		puts "Reinsertion method: " + @reinsertion_method
		puts "Reinsertion arguments: " + @reinsertion_args.to_s
		puts "Selection method: " + @selection_method
		puts "Selection arguments: " + @selection_args.to_s
		puts "Crossover rate: " + @crossover_rate.to_s
		puts "Crossover method: " + @crossover_method
		puts "Mutation rate: " + @mutation_rate.to_s
		puts "Number of Generations: " + @generations.to_s
		puts "Problem instance: " + @instance
	end

	def default
		File.open(File.absolute_path('app/default_parameters.rb')) { |f|
			eval(f.read)
		}
	end

end