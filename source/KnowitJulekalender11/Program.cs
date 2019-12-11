using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace KnowitJulekalender11
{
    class Program
    {
        private static int iceCounter = 0;
        private static bool goingUp = false;
        static Dictionary<char, Func<int>> values = new Dictionary<char, Func<int>>()
        {
            {'G', () => -27 },
            {'I', () => (++iceCounter)*12},
            {'A', () => -59},
            {'S', () => -212},
            {'F', () =>
                {
                    goingUp = !goingUp;
                    return goingUp ? -70 : 35;
                }
            }
        };

        private static int CalculateBreakDistance(long velocity, Stream stream)
        {
            int km = 0;
            char c;
            while ((c = (char)stream.ReadByte()) != -1)
            {
                km++;
                velocity += values[c]();
                if (c != 'I') iceCounter = 0;
                if (velocity < 0) return km;
            }
            return -1;
        }

        static void Main(string[] args)
        {
            Stopwatch sw = Stopwatch.StartNew();
            int km = 0;

            int count = 100;
            for (int i = 0; i < count; i++)
            {
                using (var stream = File.OpenRead("./terreng.txt"))
                {
                    km = CalculateBreakDistance(10703437, stream);
                }
            }
            var elapsed = sw.Elapsed;
            Console.WriteLine($"Sleden stoppet etter {km} km (gjennomsnittlig tidsbruk var {elapsed.TotalMilliseconds / count} ms)");
            Console.ReadKey();
        }
    }
}
