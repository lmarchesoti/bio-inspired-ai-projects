require_relative '../app/genetic_algorithm'

describe GeneticAlgorithm do
	
	before do
		@default_instance = 'send+more=money'
		@GA = GeneticAlgorithm.new population_size: 50, tourney_size: 3, crossover_rate: 80,
		 mutation_rate: 20, generations: 50, instance: @default_instance, reinsertion_method: 'best',
		 reinsertion_args: { derpiness: 30 }, selection_method: 'tourney', selection_args: { herpiness: 30 },
		 crossover: 'ciclic'
	end

	describe "basic functionality" do

		it { should respond_to :population }
		it { should respond_to :population_size }
		it { should respond_to :reinsertion_method }
		it { should respond_to :reinsertion_args }
		it { should respond_to :selection_method }
		it { should respond_to :selection_args }

		it { should respond_to :crossover_rate }
		it { should respond_to :offspring_size }

		it { should respond_to :mutation_rate }
		it { should respond_to :mutation_number }

		it { should respond_to :generations }

		it { should respond_to :instance }
		it { should respond_to :crossover_method }

	end

	describe "initialization" do

		it "needs to take an argument for population size" do
			expect { GeneticAlgorithm.new population_size: 50 }.to_not raise_error
		end

		it "needs to take an argument for crossover rate" do
			expect { GeneticAlgorithm.new crossover_rate: 80 }.to_not raise_error
		end

		it "should keep track of the crossover rate" do
			@GA.crossover_rate.should == 80
		end

		it "should default crossover_rate to 0" do
			GeneticAlgorithm.new.crossover_rate.should == 0
		end

		it "needs to take an argument for mutation rate" do
			expect { GeneticAlgorithm.new mutation_rate: 20 }.to_not raise_error
		end

		it "should keep track of mutation rate" do
			@GA.mutation_rate.should == 20
		end

		it "needs to take an argument for number of generations" do
			expect { GeneticAlgorithm.new generations: 15 }.to_not raise_error
		end

		it "should keep track of number of generations" do
			@GA.generations.should == 50
		end

		it "initializes instance as an argument" do
			@GA.instance.should == 'send+more=money'
		end

		it "defaults instance to 'default'" do
			GeneticAlgorithm.new.instance.should == 'default'
		end

		it "keeps track of reinsertion method" do
			@GA.reinsertion_method.should == 'best'
		end

		it "keeps track of args for reinsertion method" do
			@GA.reinsertion_args.should == {derpiness: 30}
		end

		it "keeps track of selection method" do
			@GA.selection_method.should == 'tourney'
		end

		it "keeps track of args for reinsertion method" do
			@GA.selection_args.should == {herpiness: 30}
		end

		it "keeps track of crossover method" do
			@GA.crossover_method.should == 'ciclic'
		end

	end

	describe "preparation phase" do

		before do
			@GA.reinsertion_method = 'elitism'
			@GA.selection_method = 'roulette'
			@GA.crossover_method = 'pmx'
			@GA.preparation_phase!
		end

		it "should initialize a population with the correct number of individuals" do
			@GA.population.individuals.length.should == 50
		end

		it "sets the correct value for offspring size" do
			@GA.offspring_size.should == 40
		end

		it "sets the correct value for mutation number" do
			@GA.mutation_number.should == 10
		end

		it "loads instance correctly" do
			Individual.fitness_mask.should == [[0, 1, 2, 3], [4, 5, 6, 1], [4, 5, 2, 1, 7]]
		end

		it "sets crossover method correctly" do
			Individual.crossover_method.should =='pmx'
		end

		it "creates population with correct reinsertion method" do
			@GA.population.reinsertion_method.should == 'elitism'
		end

		it "creates population with correct reinsertion args" do
			@GA.population.reinsertion_args.should == { derpiness: 30 }
		end

		it "initializes a population with correct selection method" do
			@GA.population.selection_method.should == 'roulette'
		end

		it "initializes a population with correct selection args" do
			@GA.population.selection_args.should == { herpiness: 30 }
		end
	end

	describe "crossover phase" do

		before do
			@GA.preparation_phase!
			@old_population = @GA.population.individuals.dup
			@GA.crossover_phase!
		end

		it { should respond_to :crossover_phase! }

		it "needs to modify the population with new individuals" do
			(@GA.population.individuals - @old_population).length.should > 18
		end

		it "needs to return a sorted population" do
			@GA.population.individuals.should == @GA.population.individuals.sort
		end

		it "needs to call reinsertion" do
			@GA.stub(:generate_offspring).and_return([])
			@GA.population.should_receive(:reinsert!).with([])
			@GA.crossover_phase!
		end

		describe "offspring generation" do

			before do
				@matching = GeneticAlgorithm.new population_size: 50, crossover_rate: 80,
					instance: @default_instance, tourney_size: 1, crossover: 'ciclic'
				@matching.preparation_phase!
			end

			it "should run as many times as the offspring size should be" do
				@matching.should_receive(:match_and_cross).exactly(20)
					.times.and_return([Individual.new, Individual.new])
				@matching.crossover_phase!
			end

			it "should generate an offspring according to crossover rate" do
				offspring_size = 40 # @matching.population.population_size * @matching.crossover_rate / 100
				@matching.generate_offspring.length.should == offspring_size
			end

			it "should cross 2 individuals at a time" do
				@matching.population.should_receive(:select).twice.and_return(Individual.new)
				@matching.match_and_cross
			end

			it "should return an array of individuals" do
				match = @matching.match_and_cross
				match.class.should == Array
			end

		end

	end

	describe "mutation phase" do

		before { @GA.preparation_phase! }

		it { should respond_to :mutation_phase! }

		it "should pick a different individual each time" do
			@GA.population.individuals.should_receive(:sample).with(no_args())
				.exactly(@GA.mutation_number).and_return(Individual.new)
			@GA.mutation_phase!
		end

		it "should modify as many individuals as the mutation rate" do
			@GA.population.individuals.stub(:sample).and_return(@GA.population.individuals[0])
			@GA.population.individuals[0].should_receive(:mutate!).exactly(@GA.mutation_number)
			@GA.mutation_phase!
		end

		it "should alter the population elements" do
			old_individual_reference = @GA.population.individuals[0]
			old_individual = old_individual_reference.dup
			@GA.population.individuals.stub(:sample).and_return(old_individual_reference)
			@GA.mutation_phase!
			old_individual.should_not == old_individual_reference
		end

		it "should keep the population sorted" do
			@GA.mutation_phase!
			@GA.population.individuals.dup.should == @GA.population.individuals.sort
		end

	end

	describe "each generation" do

		before { @GA.preparation_phase! }

		it { should respond_to :generation! }

		it "needs to run crossover phase once" do
			@GA.should_receive(:crossover_phase!).once
			@GA.generation!
		end

		it "needs to run mutation phase once" do
			@GA.should_receive(:mutation_phase!).once
			@GA.generation!
		end

	end

	describe "full run" do

		before { @GA.preparation_phase! }

		it { should respond_to :run! }

		it "prepares once" do
			@GA.should_receive(:preparation_phase!).once
			@GA.run!
		end

		it "needs to run for the correct number of generations" do
			@GA.should_receive(:generation!).exactly(@GA.generations)
			@GA.run!
		end

		it "needs to return the best individual" do
			@GA.run!.should == @GA.population.individuals.first
		end

	end

	describe "batch run" do

		before { @GA.preparation_phase! }

		it { should respond_to :batch }

		it "should run the correct number of times" do
			@GA.should_receive(:run!).exactly(5).times
			@GA.batch 5
		end

		it "needs to sum up the times the goal was achieved" do
			@GA.stub(:run!)
			@GA.stub(:goal?).and_return(true, true, false)
			@GA.batch(3).should == 2
		end

	end

	describe "goal" do

		before { @GA.preparation_phase! }

		it { should respond_to :goal? }

		it "needs to return 1 if the best individual fitness is 0" do
			@GA.population.individuals.first.fitness = 0
			@GA.goal?.should == true
		end

		it "needs to return 0 if the best individual fitness is not 0" do
			@GA.population.individuals.first.fitness = 8008
			@GA.goal?.should == false
		end
	end

end