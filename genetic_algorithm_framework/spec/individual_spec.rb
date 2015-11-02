require_relative '../app/individual'

describe Individual do

	before do
		Individual.load_instance 'send+more=money'
		@individual = Individual.new
		@stub = Individual.new
		@stub.content = [0, 1, 3, 7, 5, 9, 6, 2, 4, 8]
		@stub.calculate_fitness!
		@mate = @stub.dup
		@mate.content = [8, 3, 6, 2, 7, 5, 4, 9, 1, 0]
		@mate.calculate_fitness!
	end

	describe "basic methods" do

		it { should respond_to :fitness }

		it { should respond_to :content }

		it { should respond_to :cross }
		
		it { should respond_to :mutate! }

		it { should respond_to :calculate_fitness! }

		it { should respond_to :generate_crossover_mask }

		it { should respond_to :generate_argument }

		it { Individual.should respond_to :fitness_mask }

		it { Individual.should respond_to :crossover_method }

	end

	describe "crypto arithmetics" do

		subject { @individual }

		describe "instance loader" do

			it "loads instances on relative files" do

				Individual.load_instance 'coca+cola=oasis'
				Individual.fitness_mask.should == [[0, 1, 0, 2], [0, 1, 3, 2], [1, 2, 4, 5, 4]]

			end

		end

		describe "fitness mask" do

			it "defaults to array of 3 empty arrays" do

				Individual.fitness_mask = nil
				Individual.fitness_mask.should == [[], [], []]

			end

		end

		describe "content" do

			it "should not initialize equal individuals" do
				subject.content.should_not == Individual.new.content
			end

			it "should have elements 1..9" do
				subject.content.length.should be 10
				(0..9).each do |i|
					subject.content.count(i).should == 1
				end
			end

		end

		describe "fitness calculation" do

			it "should have fitness initialized" do
				subject.fitness.should_not be_nil
			end

			it "should follow the fitness mask" do
				@stub.fitness.should == 53214
			end

			describe "argument generation" do
				
				it "should generate operands based on the fitness mask" do
					@stub.generate_argument([3, 5, 2]).should == 793
				end
			end

		end

		describe "crossover" do

			describe "methods" do

				it "redirects to whatever method is stored" do
					Individual.crossover_method = 'xingalinga'
					p = Individual.new
					@stub.should_receive(:xingalinga).with(p).once
					@stub.cross p
				end

				describe "ciclic support" do

					before do
						Individual.crossover_method = 'ciclic'
						@stub.stub(:rand).and_return(1)
						@children = @stub.cross @mate
					end

					it { should respond_to :ciclic }

					it "should return 2 individuals" do
						@children.count.should == 2
					end

					it "should return valid individuals" do
						@children.each do |child|
							child.should be_valid
						end
					end

					it "needs to behave like ciclic crossover" do
						@children[0].content.should == [0, 3, 6, 7, 5, 9, 4, 2, 1, 8]
						@children[0].fitness.should == 53322
						@children[1].content.should == [8, 1, 3, 2, 7, 5, 6, 9, 4, 0]
						@children[1].fitness.should == 59626
					end

				end

				describe "pmx support" do

					it { should respond_to :pmx }

					# IMPLEMENT

					it "raises an exception" do
						expect { @stub.pmx Individual.new }.to raise_error
					end

				end

			end

		end

		describe "crossover mask generator" do

			before do
				@stub.stub(:rand).and_return(1)
				@mask = @stub.generate_crossover_mask(@mate)
			end

			it "should return a mask within bounds" do
				@mate.generate_crossover_mask(@stub).length.should <= 10
			end

			it "should return a ciclic mask" do
				@mask.should == [1, 2, 6, 8]
			end
		end

		describe "mutation" do

			before do
				@mutation = @individual.dup
				@mutation.mutate!
			end

			describe "should modify the individual" do

				it "content should be modified" do
					@mutation.content.should_not == @individual.content
				end

				it "fitness should be updated" do
					@individual.fitness.should_not == @mutation.fitness
				end

				it "should be random" do
					count = 0;
					500.times do
						alternative_mutation = @individual.dup
						alternative_mutation.mutate!
						if alternative_mutation.content == @mutation.content
							count += 1
						end
					end
					count.should < 80
				end

				it "should swap two positions" do
					new_mutation = @stub
					new_mutation.content.stub(:sample).and_return([3, 7])
					new_mutation.mutate!
					new_mutation.content.should == [0, 1, 3, 2, 5, 9, 6, 7, 4, 8]
				end

			end

			it "should return a valid individual" do
				@mutation.should be_valid
			end

		end

		describe "individual validation" do

			let(:valid_individual) { @stub }
			let(:invalid_individual) { @individual }

			it "should accept valid individuals" do
				valid_individual.should be_valid
			end

			it "should check duplicated numbers" do
				invalid_individual.content = [1, 1, 3, 3, 9, 8, 1, 9, 0]
				invalid_individual.should_not be_valid
			end

			it "should check content size" do
				invalid_individual.content = [0, 2, 3]
				invalid_individual.should_not be_valid
			end

			it "should check fitness value" do
				invalid_individual.fitness = 382
				invalid_individual.should_not be_valid
			end

		end

		describe "comparison" do

			it "should compare < correctly" do
				comparison = @stub.dup
				comparison.fitness = 5
				(comparison <=> @stub).should == -1
			end

			it "should compare == correctly" do
				comparison = @stub.dup
				(comparison <=> @stub).should == 0
			end

			it "should compare > correctly" do
				comparison = @stub.dup
				comparison.fitness = 80080
				(comparison <=> @stub).should == 1
			end
		end

	end

end