using System;

namespace GeneticBoilerplate
{
    public abstract class Individual
    {
        private IComparable? _fitness;

        public IComparable Fitness => _fitness ??= Evaluate();

        protected abstract IComparable Evaluate();
        public abstract Individual Crossover(Individual other);
        public abstract void Mutate();
    }
}