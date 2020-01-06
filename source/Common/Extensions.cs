using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Common
{
    public static class Extensions
    {
        public static IEnumerable<string> EnumerateSegments(this string s, char c)
        {
            int l = s.Length;
            int i = 0, j = s.IndexOf(c, 0, l);
            if (j == -1) // No such substring
            {
                yield return s; // Return original and break
                yield break;
            }

            while (j != -1)
            {
                if (j - i > 0) // Non empty? 
                {
                    yield return s.Substring(i, j - i); // Return non-empty match
                }
                i = j + 1;
                j = s.IndexOf(c, i, l - i);
            }

            if (i < l) // Has remainder?
            {
                yield return s.Substring(i, l - i); // Return remaining trail
            }
        }


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
        }

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
