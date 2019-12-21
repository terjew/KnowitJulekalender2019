using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KnowitJulekalender17
{
    public class Program
    {
        public static void Main(string[] args)
        {
            uint num = 1000000;
            Stopwatch sw = Stopwatch.StartNew();
            int count = 0;
            int iterations = 100;

            for (int i = 0; i < iterations; i++)
            {
                //count = (int)Optimized.CountTriangleSquaresSinglethreaded(num);
                count = (int)Optimized.CountTriangleSquaresLinq(num);
                //count = (int)WithoutLinq.CountTriangleSquaresMultithreaded(num);
                //count = Naive.CountTriangleSquaresLinq(num);
            }
            var elapsed = sw.ElapsedMilliseconds;
            Console.WriteLine($"Av de første {num} triangeltall er {count} også kvadrater. (avg {elapsed / iterations} ms)");
        }
        
    }
}
