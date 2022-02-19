using System;
using System.Collections.Generic;
using System.Linq;

namespace GeneticBoilerplate
{
    public static class Utils
    {
        public static readonly Random Rand = new Random();

        public static T Random<T>(this IEnumerable<T> list)
        {
            return list.ToArray().Random();
        }

        private static T Random<T>(this IReadOnlyList<T> array)
        {
            return array[Rand.Next(0, array.Count - 1)];
        }
    }
}