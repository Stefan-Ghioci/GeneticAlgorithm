using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace GeneticBoilerplate
{
    public static class StaticRandom
    {
        private static int _seed = Environment.TickCount;

        private static readonly ThreadLocal<Random> ThreadRandom =
            new(() => new Random(Interlocked.Increment(ref _seed)));

        public static int Rand() => ThreadRandom.Value!.Next();

        public static int Rand(int maxValue) => ThreadRandom.Value!.Next(maxValue);

        public static int Rand(int minValue, int maxValue) => ThreadRandom.Value!.Next(minValue, maxValue);
    }

    public static class Utils
    {
        public static T Random<T>(this IEnumerable<T> list)
        {
            return list.ToArray().Random();
        }

        private static T Random<T>(this IReadOnlyList<T> array)
        {
            return array[StaticRandom.Rand(0, array.Count - 1)];
        }
    }
}