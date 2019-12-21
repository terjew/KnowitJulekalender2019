using System;
using System.Diagnostics;

namespace KnowitJulekalender19
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch sw = Stopwatch.StartNew();
            long sum = 0;

            //for (int i = 0; i < 5; i++)
            {
                sum = Optimized.SumHiddenPalindromes(1, 123454321);
            }

            var elapsed = sw.Elapsed;
            Console.WriteLine($"Sum av skjulte palindromtall er {sum} ({elapsed.TotalMilliseconds} ms).");
        }

        
    }
}
