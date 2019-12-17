//#define SQRT_INLINE
//#define SQRT_FAST
using System;
using System.Collections.Generic;
using System.Linq;

namespace KnowitJulekalender17
{

    public class Optimized
    {

        public static int CountTriangleSquaresSinglethreaded(uint num)
        {
            int count = 0;
            for (long j = 0; j < num; j++)
            {
                long triangle = (j * (j + 1)) / 2;
                count += CountIfSquareRotated(triangle);
            }
            return count;
        }

        public static int CountTriangleSquaresLinq(uint count)
        {
            return EnumerateTriangleNumbers(count)
                .AsParallel()
                .Sum(t => CountIfSquareRotated(t));
        }

        public static int CountIfSquareRotated(long number)
        {
            if (IsSquareFast64(number)) return 1;

            int numDigits = 1;

            //count digits and find the first:
            long firstdigit = number;
            long powTen = 1;
            if (firstdigit >= 100000000)
            {
                firstdigit /= 100000000;
                powTen *= 100000000;
                numDigits += 8;
            }
            if (firstdigit >= 10000)
            {
                firstdigit /= 10000;
                powTen *= 10000;
                numDigits += 4;
            }
            if (firstdigit >= 100)
            {
                firstdigit /= 100;
                powTen *= 100;
                numDigits += 2;
            }
            if (firstdigit >= 10)
            {
                firstdigit /= 10;
                powTen *= 10;
                numDigits += 1;
            }

            long rotated = number;
            for (int i = 1; i < numDigits; i++)
            {
                rotated = ((rotated * 10) + firstdigit) - (firstdigit * powTen * 10);

#if SQRT_INLINE
                if ((0x202021202030213 & (1L << (int)(rotated & 63))) > 0)
                {
                    long t = (long)Math.Round(Math.Sqrt(rotated));
                    bool result = t * t == rotated;
                    if (result) return 1;
                }
#elif SQRT_FAST
                if (IsSquareFast64(rotated)) return 1;                
#else
                if (IsSquare(rotated)) return 1;
#endif
                firstdigit = rotated / powTen;
            }
            return 0;
        }

        private static bool IsSquare(long number)
        {
            var root = (long)(Math.Sqrt(number) + 0.5);
            return root * root == number;
        }

        private static bool IsSquareFast64(long n)
        {
            if ((0x202021202030213 & (1L << (int)(n & 63))) > 0)
            {
                long t = (long)Math.Round(Math.Sqrt(n));
                bool result = t * t == n;
                return result;
            }
            return false;
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
