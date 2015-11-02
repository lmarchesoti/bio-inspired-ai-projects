require_relative 'individual'

class Population

	attr_accessor :individuals
	attr_accessor :population_size
	attr_accessor :reinsertion_method
	attr_accessor :reinsertion_args
	attr_accessor :selection_method
	attr_accessor :selection_args

	def initialize args = {}
		@population_size = args.fetch(:size, 0)
		@reinsertion_method = args.fetch(:reinsertion, 'best')
		@reinsertion_args = args.fetch(:reinsertion_args, {})
		@selection_method = args.fetch(:selection, 'tourney')
		@selection_args = args.fetch(:selection_args, {})

		@individuals = Array.new(@population_size) { Individual.new }
		@individuals.sort!
	end

	def select quantity = 1
		selected = []
		quantity.times do
			selected << (public_send(@selection_method, @selection_args))[0]
		end
		convert_select_to_output selected
	end

	def tourney args = {}
		tourney_size = args.fetch(:tourney_size, 1)
		@individuals.sample(tourney_size).sort
	end

	def roulette args = {}
		# IMPLEMENT
		raise "NOT YET IMPLEMENTED"
	end

	def reinsert! offspring
		public_send(@reinsertion_method, offspring, @reinsertion_args)
	end

	def best offspring, args = {}
		(@individuals += offspring.reverse).sort!
		@individuals = @individuals[0, @population_size]
	end

	def elitism offspring, args = {}
		# IMPLEMENT
		raise "NOT YET IMPLEMENTED"
	end

	private

		def convert_select_to_output samples
			samples.length == 1 ?
				samples.first
				:
				samples
		end
end