require_relative '../app/population'
require_relative 'selection_method_spec'

describe Population do

	before do
		Individual.load_instance 'send+more=money'
		@population = Population.new size: 50, tourney_size: 3, reinsertion: 'best'
		@offspring = Population.new(size: 15).individuals
	end

	describe "basic functionality" do

		it { should respond_to :individuals }
		it { should respond_to :population_size }
		it { should respond_to :reinsertion_method }
		it { should respond_to :selection_method }

	end

	describe "initialization" do 

		it "should default to empty" do
			Population.new.individuals.size.should == 0
		end

		it "should make use of specified size" do
			Population.new(size: 15).individuals.length.should == 15
		end

		it "should record the population size as max" do

			Population.new(size: 15).population_size.should == 15

		end

		it "should create a variety of individuals" do
			pop = Population.new size: 15
			pop.individuals.size.should == pop.individuals.uniq.size
		end

		it "should default size to 0" do
			Population.new.population_size.should == 0
		end

		it "should initialize a sorted array" do
			array = Population.new(size: 15).individuals
			array.should == array.sort
		end

		it "records the reinsertion method" do
			Population.new(reinsertion: 'best').reinsertion_method.should == 'best'
		end

		it "defaults reinsertion method to best" do
			Population.new.reinsertion_method.should == 'best'
		end

		it "defaults reinsertion args to {}" do
			Population.new.reinsertion_args.should == {}
		end

		it "records selection method" do
			Population.new(selection: 'roulette').selection_method.should == 'roulette'
		end

		it "records selection args" do
			Population.new(selection_args: { wakalaka: 15 }).selection_args.should == { wakalaka: 15 }
		end

		it "defaults selection method to tourney" do
			Population.new.selection_method.should == 'tourney'
		end

		it "defaults selection args to {}" do
			Population.new.selection_args.should == {}
		end

	end

	describe "selection" do

		it { should respond_to :select }

		describe "arguments" do

			it { should respond_to :selection_args }

			it "sends selection args to method along with quantity" do
				@population.selection_args = { chumbalumba: 30, kakaka: 20 }
				@population.should_receive(:tourney).with(chumbalumba: 30, kakaka: 20).and_return([Individual.new])
				@population.select
			end

		end

		describe "methods" do

			subject { @population }

			it "redirects to whatever method is stored" do
				subject.selection_method = 'xingalinga'
				subject.should_receive(:xingalinga).with({}).once.and_return([Individual.new])
				subject.select
			end

			describe "tourney support" do

				it_should_behave_like "selection method", 'tourney', { tourney_size: 3 }

				it "should fetch tourney size from args and work with it" do
					10.times do |n|
						@population.selection_args = { tourney_size: n }
						@population.individuals.should_receive(:sample).with(n).and_return([])
						@population.select
					end
				end

			describe "roulette support" do

				it { should respond_to :roulette }

				# IMPLEMENT

				it "raises an exception" do
					expect { @population.roulette }.to raise_error
				end

				# it_should_behave_like "selection method", 'roulette'

			end

			end

		end

		it "should invoke the selection method once per quantity" do
			@population.should_receive(:tourney).exactly(5).and_return([Individual.new])
			@population.select 5
		end

		describe "correct output" do

			it { should_not respond_to :convert_select_to_output}

			it "should default to return 1 individual" do
				@population.select.class.should == Individual
			end

			it "should return an array of specified quantity" do
				set = @population.select(3)
				set.class.should == Array
				set.count.should == 3
			end
		end

	end

	describe "reinsertion" do

		before do
			@population.reinsert! @offspring
		end

		it { should respond_to :reinsert! }

		it "should not exceed max population size" do

			@population.individuals.length.should == @population.population_size

		end

		describe "arguments" do

			it { should respond_to :reinsertion_args }

			it "sends reinsertion args to method along with offspring" do
				@population.reinsertion_args = { chumbalumba: 30, kakaka: 20 }
				@population.should_receive(:best).with([], chumbalumba: 30, kakaka: 20)
				@population.reinsert! []
			end

		end

		describe "methods" do

			before do
				@population.individuals = []
			end

			subject { @population }

			it "redirects to whatever method is stored" do
				subject.reinsertion_method = 'xingalinga'
				subject.should_receive(:xingalinga).with([], {}).once
				subject.reinsert! []
			end

			describe "best support" do

				before(:each) do
					@old_individuals = @population.individuals
					@population.best @offspring
				end

				it { should respond_to :best }

				it "should keep individuals ordered" do
					@population.individuals.dup.should == @population.individuals.sort
				end

				it "should only keep the best individuals" do

					truncated_reinsertion = (@old_individuals + @offspring).sort
					truncated_reinsertion = truncated_reinsertion[0, @population.population_size]
					@population.individuals.should == truncated_reinsertion

				end

			end

			describe "elitism support" do

				it { should respond_to :elitism }

				# IMPLEMENT

				it "raises an exception" do
					expect { @population.elitism [] }.to raise_error
				end

			end

		end

	end

end