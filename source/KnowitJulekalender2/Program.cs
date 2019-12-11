using System;
using System.IO;
using System.Linq;

namespace KnowitJulekalender2
{
    class Coordinate
    {
        public int Y { get; private set; }
        public int X { get; private set; }

        public Coordinate(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
    class Program
    {

        static int[] elevations;
        static void Main(string[] args)
        {
            var worldtxt = File.ReadAllLines("./world.txt");
            var width = worldtxt[0].Length;
            var height = worldtxt.Length;
            elevations = new int[width];

            for (int y = 0; y < height; y++)
            {
                var line = worldtxt[y];
                
                for (int x = 0; x < width; x++)
                {
                    if (elevations[x] != 0) continue;
                    if (line[x] == '#')
                    {
                        elevations[x] = height - y;
                    }
                }
            }

            Console.WriteLine($"Min: {elevations.Min()}, Max: {elevations.Max()}");

            int totalWater = 0;

            var leftPeakPos = findNextPeak(0, 1);
            var rightPeakPos = findNextPeak(elevations.Length - 1, -1);

            //Start at lowest peak, fill water towards other peak until we are back at peak altitude:
            while (leftPeakPos.X < rightPeakPos.X)
            {
                if (leftPeakPos.Y < rightPeakPos.Y)
                {
                    var otherPos = crossLake(leftPeakPos, 1);
                    totalWater += measureLake(leftPeakPos, otherPos);
                    leftPeakPos = findNextPeak(otherPos.X, 1);
                } 
                else
                {
                    var otherPos = crossLake(rightPeakPos, -1);
                    totalWater += measureLake(otherPos, rightPeakPos);
                    rightPeakPos = findNextPeak(otherPos.X, -1);
                }
            }
        }

        static Coordinate crossLake(Coordinate start, int direction)
        {
            int startX = start.X;
            int startY = start.Y;

            int pos = startX + direction;
            while (elevations[pos] < startY) pos += direction;
            return new Coordinate(pos, elevations[pos]);
        }

        static int measureLake(Coordinate left, Coordinate right)
        {
            int lakeElevation = Math.Min(left.Y, right.Y);
            int sum = 0;
            for (int i = left.X + 1; i < right.X; i++)
            {
                sum += lakeElevation - elevations[i];
            }
            return sum;
            //return elevations.Skip(left.X).Take(right.X - left.X - 1).Select(elevation => left.Y - elevation).Sum();
        }

        static Coordinate findNextPeak(int startPos, int direction)
        {
            int pos = startPos;
            int max = elevations[pos];
            while (pos < elevations.Length && pos >= 0)
            {
                var h = elevations[pos];
                if (h >= max)
                {
                    max = h;
                    pos += direction;
                }
                else
                {
                    return new Coordinate(pos - direction, max);
                }
            }
            return null;
        }
    }
}
