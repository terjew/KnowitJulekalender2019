using System.Collections.Generic;
using System.Linq;

namespace KnowitJulekalender19
{
    class Optimized
    {
        public static IEnumerable<long> Range(long start, long count)
        {
            long max = start + count;
            while (start < max) yield return start++;
        }

        public static long SumHiddenPalindromes(int start, int count)
        {
            long sum = Range(start, count)
                    .AsParallel()
                    .Sum(l => IsHiddenPalindrome(l));

            return sum;
        }

        public static long IsHiddenPalindrome(long l)
        {
            var reversed = ReverseDigits(l);
            if (l == reversed) return 0;

            var sum = l + reversed;
            var sumReversed = ReverseDigits(sum);
            if (sum == sumReversed) return l;
            return 0;
        }

        public static long ReverseDigits(long l)
        {
            long reversed = 0;
            long rest = l;
            while (rest > 0)
            {
                reversed *= 10;
                reversed += (rest % 10);
                rest /= 10;
            }

            return reversed;
        }

    }
}
