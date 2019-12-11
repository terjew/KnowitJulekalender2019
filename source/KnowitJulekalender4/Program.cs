using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace KnowitJulekalender4
{
    public class Canvas
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        private int[] array;

        public IEnumerable<int> Array => array;

        public Canvas(int width, int height, int initialValue = 0)
        {
            this.Width = width;
            this.Height = height;
            array = Enumerable.Repeat(initialValue, Width * Height).ToArray();
        }

        public int AddToPixel(int x, int y, int color)
        {
            var index = y * Width + x;
            var prev = array[index];
            array[index] += color;
            return prev;
        }

        public int DrawLine(int x, int y, int targetX, int targetY)
        {
            int dirX = Math.Sign(targetX - x);
            int t = 0;
            while (x != targetX)
            {
                t += AddToPixel(x, y, 1);
                x += dirX;
            }
            int dirY = Math.Sign(targetY - y);
            while (y != targetY)
            {
                t += AddToPixel(x, y, 1);
                y += dirY;
            }
            return t;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            TimeSpan elapsed = TimeSpan.Zero;
            int n = 1000;
            int t = 0;
            for (int i = 0; i < n; i++)
            {
                //warmup
                CalculateTime();
            }
            for (int i = 0; i < n; i++)
            {
                Stopwatch sw = Stopwatch.StartNew(); ;
                t = CalculateTime();
                elapsed += sw.Elapsed;
            }
            Console.WriteLine($"Sneglen brukte {t} minutter, programmet fullførte (i snitt) på {(elapsed / n).TotalMilliseconds} ms");
        }

        private static int CalculateTime()
        {
            int x = 0;
            int y = 0;
            Canvas canvas = new Canvas(1000, 1000, 1);

            var lines = File.ReadAllLines("./coords.csv");
            var trail = lines.Skip(1);

            int t = 0;
            foreach (var l in trail)
            {
                var pair = l.Split(",").Select(str => int.Parse(str)).ToArray();
                var tX = pair[0];
                var tY = pair[1];
                t += canvas.DrawLine(x, y, tX, tY);
                x = tX;
                y = tY;
            }
            return t;
        }
    }
}
