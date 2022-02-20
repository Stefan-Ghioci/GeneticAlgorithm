using System;
using System.Collections.Immutable;

namespace GeneticBoilerplate
{
    internal static class Program
    {
        private static void Main()
        {
            string[] ingredients = {"basil", "cheese", "peppers", "pineapple", "mushrooms", "tomatoes"};
            var clients = ImmutableHashSet.Create
            (
                new Client
                (
                    ImmutableHashSet.Create<string>("cheese", "peppers"),
                    ImmutableHashSet<string>.Empty
                ),
                new Client
                (
                    ImmutableHashSet.Create<string>("basil"),
                    ImmutableHashSet.Create<string>("pineapple")
                ),
                new Client
                (
                    ImmutableHashSet.Create<string>("mushrooms", "tomatoes"),
                    ImmutableHashSet.Create<string>("basil")
                )
            );

            var solution = new PizzaGeneticAlgorithm(10, 20, 50, ingredients, clients, 10).Run();

            Console.WriteLine(solution.BinaryRepresentation);
            Console.WriteLine(solution.Fitness);
        }
    }
}