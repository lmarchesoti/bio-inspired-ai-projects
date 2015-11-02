shared_examples "selection method" do |method, args|

	describe "selection method" do
	
		before do
			@population = Population.new(size: 50, selection: method, selection_args: args)
		end

		it "should be random" do
			addiction = 0
			200.times do
				if @population.select == @population.select
					addiction += 1
				end
			end
			addiction.should < 13
		end

		it "should pick n random individuals" do
			(2..6).each do |n|
				@population.select(n).length.should == n
			end
		end

		it "should return a single individual when n = 1" do
			@population.select(1).class.should == Individual
		end

	end
end