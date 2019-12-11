using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace KnowitJulekalender9
{
    class Program
    {
        static void Main(string[] args)
        {
            int runs = 30;
            long sum = 0;

            if (45 != IfKrampusDirect(45) || 0 != IfKrampusDirect(1000)) throw new InvalidOperationException("test feilet");

            for (int i = 0; i < runs; i++)
            {
                sum = SumKrampus("./krampus.txt");
            }

            Stopwatch sw = Stopwatch.StartNew();
            for (int i = 0; i < runs; i++)
            {
                sum = SumKrampus("./krampus.txt");
            }
            var elapsed = sw.Elapsed;

            Console.WriteLine($"Summen av krampustall i listen er {sum} (gjennomsnittlig kjøretid var {elapsed.TotalMilliseconds / runs} ms)");
            Console.ReadKey();
        }

        private static long SumKrampus(string path)
        {
            return File.ReadLines(path)
                .AsParallel()
                .Select(l => long.Parse(l))
                .Select(n => IfKrampusDirect(n))
                .Sum();

        }

        private static long IfKrampus(long v)
        {
            var digits = (v * v).ToString();
            for (int i = 1; i < digits.Length - 1; i++)
            {
                var a = long.Parse(digits.Substring(0, i));
                var b = long.Parse(digits.Substring(i));
                if (b != 0 && a + b == v) return v;
            }
            return 0;
        }

        private static long IfKrampusDirect(long v)
        {
            long p = v * v;
            long i = 10;
            long a, b;
            do
            {
                a = p / i;
                b = p % i;
                if (b > 0 && a + b == v) return v;
                i *= 10;
            } while (a > 0);

            return 0;
        }


        private static long IfKrampusNum(long v)
        {
            if (v <= 0) return 0;
            var digits = ToDigits(v * v);
            for (int i = 1; i < digits.Length - 1; i++)
            {
                var a = ToLong(digits.Take(i));
                var b = ToLong(digits.Skip(i));
                if (a != 0 && b != 0 && a + b == v) return v;
            }
            return 0;
        }

        private static byte[] ToDigits(long val)
        {
            int nDigits = 1 + Convert.ToInt32(Math.Floor(Math.Log10(val)));
            byte[] digits = new byte[nDigits];
            int index = nDigits - 1;
            while (val > 0)
            {
                byte digit = (byte)(val % 10);
                digits[index] = digit;
                val = val / 10;
                index = index - 1;
            }
            return digits;
        }

        private static long ToLong(IEnumerable<byte> digits)
        {
            long val = 0;
            foreach (var digit in digits)
            {
                val *= 10;
                val += digit;
            }
            return val;
        }
    }
}
