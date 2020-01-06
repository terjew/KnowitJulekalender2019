using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace KnowitJulekalender22
{
    class Program
    {
        static void Main(string[] args)
        {
            int count = 1000;
            int earnings = 0;
            var program = new Program();
            var path = "./forest.txt";
            using (new CodeTimer("innlesing og utregning (regex)", null, 10))
            {
                for (int i = 0; i < 10; i++)
                {
                    var naive = new Naive(path);
                    earnings = naive.CalculateEarnings();
                }
                Console.WriteLine($"Bonden tjener {earnings} kr");
            }
            //using (new CodeTimer("kun innlesing (byte[])", null, count))
            //{
            //    for (int i = 0; i < count; i++)
            //    {
            //        var optimized = new OptimizedByteArray(path);
            //    }
            //}
            //using (new CodeTimer("innlesing og utregning (stream)", null, count))
            //{
            //    for (int i = 0; i < count; i++)
            //    {
            //        var stream = new OptimizedStream(path);
            //        earnings = stream.CalculateEarningsAsync().Result;
            //    }
            //    Console.WriteLine($"Bonden tjener {earnings} kr");
            //}
            using (new CodeTimer("innlesing og utregning (byte[])", null, count))
            {
                for (int i = 0; i < count; i++)
                {
                    var optimized = new OptimizedByteArray(path);
                    earnings = optimized.CalculateEarnings();
                }
                Console.WriteLine($"Bonden tjener {earnings} kr");
            }
            using (new CodeTimer("innlesing og utregning (memory mapped)", null, count))
            {
                for (int i = 0; i < count; i++)
                {
                    var optimized = new OptimizedMemoryMapped(path);
                    earnings = optimized.CalculateEarnings();
                }
                Console.WriteLine($"Bonden tjener {earnings} kr");
            }

        }
    }
}
