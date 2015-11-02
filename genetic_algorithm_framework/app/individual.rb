class Individual

	include Comparable

	attr_accessor :content # = 0..9
	attr_accessor :fitness
	# Global fitness mask and crossover method
	class << self
		attr_writer :fitness_mask
		attr_accessor :crossover_method

		def fitness_mask
			@fitness_mask || [[], [], []]
		end

		def load_instance name
			load File.absolute_path('app/instance/' + name + '.rb')
		end
	end

	def initialize args = 0
		@content = (0..9).to_a.shuffle!
		calculate_fitness!
	end

	def calculate_fitness!
		arg1 = generate_argument self.class.fitness_mask.first
		arg2 = generate_argument self.class.fitness_mask[1]
		result = generate_argument self.class.fitness_mask.last
		@fitness = (arg1 + arg2 - result).abs
	end

	def generate_argument mask
		argument = []
		mask.each do |item|
			argument << @content[item]
		end
		return argument.join.to_i
	end

	def cross mate
		public_send(Individual.crossover_method, mate)
	end

	def ciclic mate
		child1 = self.dup
		child2 = mate.dup
		mask = generate_crossover_mask mate
		mask.each do |current|
			child1.content[current] = mate.content[current]
			child2.content[current] = self.content[current]
		end
		child1.calculate_fitness!
		child2.calculate_fitness!
		children = [child1, child2]
	end

	def pmx mate
		# IMPLEMENT
		raise "NOT YET IMPLEMENTED"
	end

	def generate_crossover_mask mate
		mask = [rand(10)]
		begin
			mask = mask + [self.content.index(mate.content[mask.last])]
		end until mask.last == mask.first
		mask.pop
		return mask
	end

	def mutate!
		idx = @content.sample(2)
		@content[idx[0]], @content[idx[1]] = @content[idx[1]], @content[idx[0]]
		calculate_fitness!
	end

	def valid?
		if @content.length != 10
			return false
		end
		(0..9).each do |factor|
			if @content.count(factor) != 1
				return false
			end
		end
		if @fitness != self.calculate_fitness!
			return false
		end
		return true
	end

	def initialize_copy source
		super
		@content = @content.dup
	end

	def <=>(other)
		self.fitness <=> other.fitness
	end

end