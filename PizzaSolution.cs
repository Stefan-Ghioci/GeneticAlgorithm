using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace GeneticBoilerplate
{
    public class PizzaGeneticAlgorithm : GeneticAlgorithm<Pizza>
    {
        private readonly List<string> _ingredients;
        private readonly ImmutableHashSet<Client> _clients;

        public PizzaGeneticAlgorithm(int populationSize, int mutationRate, int selectionPercentage,
            List<string> ingredients, ImmutableHashSet<Client> clients, int iterationCount) : base(
            populationSize, mutationRate, selectionPercentage, iterationCount)
        {
            _ingredients = ingredients;
            _clients = clients;
        }

        protected override IComparer<IComparable> FitnessComparer => new MaxFitnessComparer();

        private class MaxFitnessComparer : IComparer<IComparable>
        {
            public int Compare(IComparable? x, IComparable? y) => x!.CompareTo(y);
        }

        protected override Pizza GenerateIndividual()
        {
            var solutionIndex = Utils.Rand.Next(1, _ingredients.Count);
            var binary = Convert.ToString(solutionIndex, 2).PadLeft(_ingredients.Count, '0');

            return new Pizza(binary, _clients, _ingredients);
        }

        protected override bool ShouldGenerationTerminate(IEnumerable<Pizza> population) =>
            Equals(OrderByFitness(population).First().Fitness, _clients.Count) ||
            base.ShouldGenerationTerminate(population);
    }

    public class Pizza : Individual
    {
        private readonly ImmutableHashSet<Client> _clients;
        private readonly List<string> _ingredients;
        public string BinaryRepresentation { get; set; }

        public Pizza
        (
            string binaryRepresentation,
            ImmutableHashSet<Client> clients,
            List<string> ingredients
        )
        {
            BinaryRepresentation = binaryRepresentation;
            _clients = clients;
            _ingredients = ingredients;
        }

        protected override IComparable Evaluate()
        {
            var solution = ComputeSolution();
            return _clients.Count(client => client.Likes.IsSubsetOf(solution) && !client.Dislikes.Overlaps(solution));
        }

        private HashSet<string> ComputeSolution()
        {
            var solution = new HashSet<string>();
            for (var i = 0; i < BinaryRepresentation.Length; i++)
                if (Convert.ToBoolean(BinaryRepresentation[i]))
                    solution.Add(_ingredients[i]);
            return solution;
        }

        public override Individual Crossover(Individual other)
        {
            var otherPizza = (Pizza) other;

            var crossedBinary = "";
            for (var i = 0; i < BinaryRepresentation.Length; i++)
            {
                crossedBinary += i % 2 == 0 ? BinaryRepresentation[i] : otherPizza.BinaryRepresentation[i];
            }

            return new Pizza(crossedBinary, _clients, _ingredients);
        }

        public override void Mutate()
        {
            var flipIndex = Utils.Rand.Next(0, BinaryRepresentation.Length);

            var array = BinaryRepresentation.ToCharArray();
            array[flipIndex] = array[flipIndex] == '0' ? '1' : '0';

            BinaryRepresentation = new string(array);
        }
    }
}