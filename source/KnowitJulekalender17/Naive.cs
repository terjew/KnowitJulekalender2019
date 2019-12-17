using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KnowitJulekalender17
{
    class Naive
    {
        public static int CountTriangleSquaresLinq(uint count)
        {
            return EnumerateTriangleNumbers(count)
                .AsParallel()
                .Sum(t => CountIfSquareRotated(t));
        }

        private static int CountIfSquareRotated(long triangle)
        {
            if (IsSquare(triangle)) return 1;
            var digits = triangle.ToString().Select(c => c - '0').ToArray();
            int num = digits.Length;
            for (int i = 1; i < num; i++)
            {
                var rotatedDigits = digits.Skip(i).Take(num - i).Concat(digits.Take(i));
                var rotated = long.Parse(string.Join("", rotatedDigits));
                if (IsSquare(rotated)) return 1;
            }
            return 0;
        }

        private static bool IsSquare(long number)
        {
            var root = (long)(Math.Sqrt(number) + 0.5);
            return root * root == number;
        }

        private static IEnumerable<long> EnumerateTriangleNumbers(uint count)
        {
            for (long i = 0; i < count; i++)
            {
                yield return (i * (i + 1)) / 2;
            }
        }
    }
}
