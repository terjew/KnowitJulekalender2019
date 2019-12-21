using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace KnowitJulekalender20
{
    class Workshop
    {
        private int[] workCount;
        private int lastElf;
        private int nextStep;

        private bool[] notPrime;

        static int[] cwOrder = { 1, 2, 3, 4, 0 };
        static int[] ccwOrder = { 4, 0, 1, 2, 3 };

        private int[] order;
        public Workshop()
        {
            workCount = new int[5];

            //initial state: First elf ( index 0) has performed the first task
            workCount[0] = 1;
            lastElf = 0;
            nextStep = 2;

            //starting direction is clockwise:
            order = cwOrder;
        }

        public int Run(int max)
        {
            notPrime = GeneratePrimeTruthTable(max);
            while (nextStep <= 1000740)
            {
                int currentElf = GetNextElf(nextStep);
                workCount[currentElf]++;
                lastElf = currentElf;
                nextStep++;
            }

            int? mostUsedElf = GetMostUsedElf();
            int? leastUsedElf = GetLeastUsedElf();

            return workCount[mostUsedElf.Value] - workCount[leastUsedElf.Value];
        }

        public int NextInLine(int current)
        {
            return order[current];
        }

        public int GetNextElf(int nextStep)
        {
            //1. if prime: assign to least used elf
            if (!notPrime[nextStep])
            {
                var leastUsed = GetLeastUsedElf();
                if (leastUsed != null)
                {
                    return leastUsed.Value;
                }
            }
            //2. if mod 28, switch direction
            if (nextStep % 28 == 0)
            {
                //clockwise *= -1;
                order = order == cwOrder ? ccwOrder : cwOrder;
                return NextInLine(lastElf);
            }
            //3: if even and next is most used, skip
            if (nextStep % 2 == 0)
            {
                var next = NextInLine(lastElf);

                if (IsMostUsed(next))
                {
                    return NextInLine(next);
                }
            }
            //4. if mod 7, select elf 4
            if (nextStep % 7 == 0)
            {
                return 4;
            }
            //5. assign to next elf in current direction
            else
            {
                return NextInLine(lastElf);
            }
        }

        private bool IsMostUsed(int elf)
        {
            var elfCount = workCount[elf];
            for (int i = 0; i < 5; i++)
            {
                if (i == elf || workCount[i] < elfCount) continue;
                return false;
            }
            return true;
        }

        private int? GetLeastUsedElf()
        {
            var smallest = workCount[0];
            var leastUsed = 0;
            bool shared = false;
            for (int i = 1; i < 5; i++)
            {
                var count = workCount[i];
                if (count < smallest)
                {
                    leastUsed = i;
                    smallest = count;
                    shared = false;
                }
                else if (count == smallest)
                {
                    shared = true;
                }
            }
            return shared ? (int?)null : leastUsed;
        }

        private int? GetMostUsedElf()
        {
            var largest = workCount[0];
            var mostUsed = 0;

            bool shared = false;
            for (int i = 1; i < 5; i++)
            {
                var count = workCount[i];
                if (count > largest)
                {
                    mostUsed = i;
                    largest = count;
                    shared = false;
                }
                else if (count == largest)
                {
                    shared = true;
                }
            }
            return shared ? (int?)null : mostUsed;
        }

        private static bool[] GeneratePrimeTruthTable(int maxPrime)
        {
            var maxSquareRoot = (int)Math.Sqrt(maxPrime);
            var eliminated = new bool[maxPrime + 1];

            for (int j = 2; j <= maxPrime; j += 2)
            {
                eliminated[j] = true;
            }

            for (int i = 3; i <= maxPrime; i+=2)
            {
                if (!eliminated[i])
                {
                    if (i <= maxSquareRoot)
                    {
                        for (int j = i * i; j <= maxPrime; j += i)
                        {
                            eliminated[j] = true;
                        }
                    }
                }
            }
            return eliminated;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            int count = 1;
            if (args.Length > 0) count = int.Parse(args[0]);
            using (new CodeTimer("programmet", null, count))
            {
                int diff = 0;
                for (int i = 0; i < count; i++)
                {
                    var workshop = new Workshop();
                    diff = workshop.Run(1000740);
                }
                Console.WriteLine($"Forskjell mellom mest og minst produktiv alv var {diff}");

            }
        }

    }
}
