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

                var generation = new List<Individual>();
                for (var i = 0; i < _populationSize; i++)
                {
                    var father = selectionPool.Random();
                    var mother = selectionPool.Random();

                    var child = father.Crossover(mother);

                    if (Utils.Rand.Next(0, 100) < _mutationRate)
                        child.Mutate();
                }

                population = generation;
            } while (!ShouldGenerationTerminate(population));

            return population.OrderByDescending(individual => individual.Fitness).First();
        }

        private IEnumerable<Individual> GeneratePopulation() =>
            Enumerable.Repeat(GenerateIndividual(), _populationSize);

        private IEnumerable<Individual> Select(IEnumerable<Individual> population) =>
            population.OrderByDescending(individual => individual.Fitness).Take(_selectionSize);

        protected abstract Individual GenerateIndividual();
        protected abstract bool ShouldGenerationTerminate(IEnumerable<Individual> population);
    }
}