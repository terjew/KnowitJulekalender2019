using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Julekalender1
{
    class Program
    {
        static void Main(string[] args)
        {
            var sheep = File.ReadAllText("./sau.txt").Split(",").Select(i => int.Parse(i));
            var sheepStack = new Stack<int>(sheep.Reverse());

            int day = 0;
            int supply = 0;
            int dragonSize = 50;
            int daysHungry = 0;

            while (daysHungry < 5)
            {
                ++day;
                if (sheepStack.Any()) supply += sheepStack.Pop();
                if (supply >= dragonSize)
                {
                    supply -= dragonSize;
                    ++dragonSize;
                    daysHungry = 0;
                }
                else
                {
                    supply = 0;
                    --dragonSize;
                    ++daysHungry;
                }
            }
            Console.WriteLine($"Population survived {day - 1} days");
        }
    }
}
