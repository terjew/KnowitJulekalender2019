using System;
using System.Collections.Generic;
using System.Linq;

namespace KnowitJulekalender12
{
    class Program
    {
        static void Main(string[] args)
        {
            int hardest = 0;
            for (int i = 1000; i < 10000; i++)
            {
                var steps = CountSteps(i);
                if (steps == 7)
                {
                    hardest++;
                }
            }
            Console.WriteLine($"hardest: {hardest}");
        }

        public static int CountSteps(int start)
        {
            if (GetDigits(start).Distinct().Count() == 1) return 0;
            int steps = 0;
            int num = start;
            while (num != 6174)
            {
                num = Kaprekar(num);
                steps++;
            }
            return steps;
        }

        public static int Kaprekar(int num)
        {
            var digits = GetDigits(num);
            var lowest = GetInt(digits.OrderBy(d => d));
            var highest = GetInt(digits.OrderByDescending(d => d));
            return highest - lowest;
        }

        private static List<int> GetDigits(int val)
        {
            var str = Math.Abs(val).ToString("D4");
            return str.Select(c => (int)(c - '0')).ToList();
        }

        private static int GetInt(IEnumerable<int> digits)
        {
            return int.Parse(string.Join("", digits));
        }

    }
}
