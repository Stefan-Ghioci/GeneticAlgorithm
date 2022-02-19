using System;
using System.Collections.Generic;
using System.Linq;

namespace GeneticBoilerplate
{
    public abstract class GeneticAlgorithm
    {
        private readonly int _populationSize;
        private readonly int _mutationRate;
        private readonly int _selectionSize;

        protected GeneticAlgorithm(int populationSize, int mutationRate, int selectionPercentage)
        {
            _populationSize = populationSize;
            _mutationRate = mutationRate;
            _selectionSize = (int) (selectionPercentage / 100f * populationSize);
        }

        public Individual Run()
        {
            var population = GeneratePopulation();

            do
            {
                var selectionPool = Select(population);
                population = BreedGeneration(selectionPool);
            }
            while (!ShouldGenerationTerminate(population));

            return OrderByFitness(population).First();
        }

        private IEnumerable<Individual> BreedGeneration(IEnumerable<Individual> selectionPool) =>
            ParallelEnumerable.Repeat(BreedIndividual(selectionPool), _populationSize);

        private Individual BreedIndividual(IEnumerable<Individual> selectionPool)
        {
            var father = selectionPool.Random();
            var mother = selectionPool.Random();

            var child = father.Crossover(mother);

            if (Utils.Rand.Next(0, 100) < _mutationRate)
                child.Mutate();

            return child;
        }

        private OrderedParallelQuery<Individual> OrderByFitness(IEnumerable<Individual> population) =>
            population.AsParallel().OrderByDescending(individual => individual.Fitness, FitnessComparer);


        private IEnumerable<Individual> GeneratePopulation() =>
            ParallelEnumerable.Repeat(GenerateIndividual(), _populationSize);

        private IEnumerable<Individual> Select(IEnumerable<Individual> population) =>
            OrderByFitness(population).Take(_selectionSize);

        protected abstract IComparer<IComparable> FitnessComparer { get; }
        protected abstract Individual GenerateIndividual();
        protected abstract bool ShouldGenerationTerminate(IEnumerable<Individual> population);
    }
}