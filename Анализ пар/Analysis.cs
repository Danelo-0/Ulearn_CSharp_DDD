using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Delegates.PairsAnalysis
{
    public static class Analysis
    {
        public static IEnumerable<Tuple<T, T>> Pairs<T>(this IEnumerable<T> enumerable)
        {
            var enumerator = enumerable.GetEnumerator();
            if (!enumerator.MoveNext())
                throw new InvalidOperationException();

            T current = enumerator.Current;
            int count = 0;

            while(enumerator.MoveNext())
            {
                yield return Tuple.Create(current, enumerator.Current);
                current = enumerator.Current;
                count++;
            }

            if (count == 0)
                throw new InvalidOperationException();
        }

        public static int MaxIndex<T>(this IEnumerable<T> enumerable)
            where T : IComparable<T>
        {
            int count = 0;
            int bestIndex = 0;
            T max = default(T);

            foreach (var item in enumerable)
            {
                if (count == 0 || item.CompareTo(max) > 0)
                {
                    max = item;
                    bestIndex = count;
                }
                count++;
            }

            if (count == 0)
                throw new InvalidOperationException();

            return bestIndex;
        }

        public static int FindMaxPeriodIndex(params DateTime[] data)
        {        
            return data
                .Pairs()
                .Select(pair => (pair.Item2 - pair.Item1).TotalSeconds)
                .MaxIndex();
        }

        public static double FindAverageRelativeDifference(params double[] data)
        {
            var relativeDifference = data
                .Pairs()
                .Select(pair => (pair.Item2 - pair.Item1) / pair.Item1).ToArray();
            return relativeDifference.Sum() / relativeDifference.Count();               
        }  
    }
}
