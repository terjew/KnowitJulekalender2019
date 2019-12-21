using System;
using System.Collections.Generic;
<<<<<<< HEAD
using System.Globalization;
=======
>>>>>>> 61f0352fa6ec45d934291ad6f19d6db59be4cc04
using System.Linq;
using System.Text;

namespace Common
{
    public static class Extensions
    {
<<<<<<< HEAD
        public static IEnumerable<string> GraphemeClusters(this string s)
        {
            var enumerator = StringInfo.GetTextElementEnumerator(s);
            while (enumerator.MoveNext())
            {
                yield return (string)enumerator.Current;
            }
        }

        public static string ReverseGraphemeClusters(this string s)
        {
            return string.Join("", s.GraphemeClusters().Reverse().ToArray());
        }

        public static string Reverse(this string s)
        {
            return string.Join("", s.Cast<char>().Reverse());
=======
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

>>>>>>> 61f0352fa6ec45d934291ad6f19d6db59be4cc04
        }
    }
}
