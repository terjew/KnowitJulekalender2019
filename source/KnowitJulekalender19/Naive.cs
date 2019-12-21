using Common;
using System;
using System.Linq;

namespace KnowitJulekalender19
{
    class Naive
    {
        public static long SumHiddenPalindromes(int start, int count)
        {
            return Enumerable.Range(start, count)
                .AsParallel()
                .Sum(l => IsHiddenPalindrome(l) ? l : 0L);
        }

        public static bool IsHiddenPalindrome(long l)
        {
            string s = l.ToString();
            if (IsPalindrome(s)) return false;
            var reverse = s.Reverse();
            var summed = l + (long.Parse(reverse));
            return IsPalindrome(summed.ToString());
        }

        private static bool IsPalindrome(string s)
        {
            var l = s.Length;
            var l2 = l / 2;
            for (int i = 0; i < l2; i++)
            {
                if (s[i] != s[l - i - 1]) return false;
            }
            return true;
        }

        public static void Test()
        {
            var alreadyPalindrome = 121;
            var hiddenPalindrome = 83;
            var notHiddenPalindrome = 49;
            if (!IsHiddenPalindrome(hiddenPalindrome) || IsHiddenPalindrome(notHiddenPalindrome) || IsHiddenPalindrome(alreadyPalindrome)) throw new Exception("IsHiddenPalindrome failed test");
        }


    }
}
