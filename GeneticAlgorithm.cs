﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace GeneticBoilerplate
{
    public abstract class GeneticAlgorithm<T> where T : Individual
    {
        private readonly int _populationSize;
        private readonly int _mutationRate;
        private readonly int _selectionSize;
        private readonly int _iterationCount;
        private int _currentIteration;

        protected GeneticAlgorithm(int populationSize, int mutationRate, int selectionPercentage, int iterationCount)
        {
            _populationSize = populationSize;
            _mutationRate = mutationRate;
            _selectionSize = (int) (selectionPercentage / 100f * populationSize);
            _iterationCount = iterationCount;
        }

        public T Run()
        {
            var population = GeneratePopulation();

            _currentIteration = 0;
            do
            {
                var selectionPool = Select(population);
                population = BreedGeneration(selectionPool);
                _currentIteration += 1;
            }
            while (!ShouldGenerationTerminate(population));

            return OrderByFitness(population).First();
        }

        private IEnumerable<T> BreedGeneration(IEnumerable<T> selectionPool) =>
            ParallelEnumerable.Repeat(BreedIndividual(selectionPool), _populationSize);

        private T BreedIndividual(IEnumerable<T> selectionPool)
        {
            var father = selectionPool.Random();
            var mother = selectionPool.Random();

            var child = (T) father.Crossover(mother);

            if (StaticRandom.Rand(0, 100) < _mutationRate)
                child.Mutate();

            return child;
        }

        protected OrderedParallelQuery<T> OrderByFitness(IEnumerable<T> population) => population.AsParallel()
            .OrderByDescending(individual => individual.Fitness, FitnessComparer);


        private IEnumerable<T> GeneratePopulation() => ParallelEnumerable.Repeat(GenerateIndividual(), _populationSize);

        private IEnumerable<T> Select(IEnumerable<T> population) => OrderByFitness(population).Take(_selectionSize);

        protected abstract IComparer<IComparable> FitnessComparer { get; }
        protected abstract T GenerateIndividual();

        protected virtual bool ShouldGenerationTerminate(IEnumerable<T> population) =>
            _iterationCount >= _currentIteration;
    }
}