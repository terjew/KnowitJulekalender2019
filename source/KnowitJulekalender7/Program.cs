using System;
using System.Linq;

namespace KnowitJulekalender7
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach(var day in Enumerable.Range(2, 6))
            {
                Console.WriteLine($"Kode for dag {day} er {SpecialDivide(5897, day)}");
            }
        }

        static long SpecialDivide(long a, long x)
        {
            var y = SpecialDivide1OverX(x);
            var z = a * y;
            return PositiveRemainder(z, 27644437);
        }

        static int SpecialDivide1OverX(long x)
        {
            foreach (var yd in Enumerable.Range(2, 27644435))
            {
                var b = yd * x;
                var r = PositiveRemainder(b, 27644437);
                if (r == 1) return yd;
            }
            throw new InvalidOperationException("Cannot find yd");
        }

        static long PositiveRemainder(long a, long b)
        {
            var remain = a % b;
            if (remain < 0)
            {
                while (remain < 0) remain += 27644437;
            }
            return remain;
        }
    }
}
