using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public static class Extensions
    {
        public static IEnumerable<IEnumerable<T>> Split<T>(this T[] array, T separator)
        {
            var comparer = EqualityComparer<T>.Default;

            int start = 0;
            while (start < array.Length)
            {
                int count = 0;
                while (start + count < array.Length && !comparer.Equals(array[start + count], separator))
                {
                    count++;
                }
                yield return array.Skip(start).Take(count);
                start += count + 1;
            }

        }
    }
}
