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
            int rounds = max / 28;
            var mainEnd = (rounds - 1) * 28;

            //head
            while (nextStep < 28)
            {
                lastElf = GetNextElfFull(nextStep++);
                workCount[lastElf]++;
            }

            //main
            while (nextStep < mainEnd)
            {
                lastElf = GetNextElf28(nextStep++); //0
                workCount[lastElf]++;
                lastElf = GetNextElf1(nextStep++);  //1
                workCount[lastElf]++;
                lastElf = GetNextElf2(nextStep++);  //2
                workCount[lastElf]++;
                lastElf = GetNextElf1(nextStep++);  //3
                workCount[lastElf]++;
                lastElf = GetNextElf2(nextStep++);  //4
                workCount[lastElf]++;
                lastElf = GetNextElf1(nextStep++);  //5
                workCount[lastElf]++;
                lastElf = GetNextElf2(nextStep++);  //6
                workCount[lastElf]++;
                lastElf = GetNextElf7(nextStep++);  //7
                workCount[lastElf]++;
                lastElf = GetNextElf2(nextStep++);  //8
                workCount[lastElf]++;
                lastElf = GetNextElf1(nextStep++);  //9
                workCount[lastElf]++;
                lastElf = GetNextElf2(nextStep++);  //10
                workCount[lastElf]++;
                lastElf = GetNextElf1(nextStep++);  //11
                workCount[lastElf]++;
                lastElf = GetNextElf2(nextStep++);  //12
                workCount[lastElf]++;
                lastElf = GetNextElf1(nextStep++);  //13
                workCount[lastElf]++;
                lastElf = GetNextElf14(nextStep++); //14
                workCount[lastElf]++;
                lastElf = GetNextElf1(nextStep++);  //15
                workCount[lastElf]++;
                lastElf = GetNextElf2(nextStep++);  //16
                workCount[lastElf]++;
                lastElf = GetNextElf1(nextStep++);  //17
                workCount[lastElf]++;
                lastElf = GetNextElf2(nextStep++);  //18
                workCount[lastElf]++;
                lastElf = GetNextElf1(nextStep++);  //19
                workCount[lastElf]++;
                lastElf = GetNextElf2(nextStep++);  //20
                workCount[lastElf]++;
                lastElf = GetNextElf7(nextStep++);  //21
                workCount[lastElf]++;
                lastElf = GetNextElf2(nextStep++);  //22
                workCount[lastElf]++;
                lastElf = GetNextElf1(nextStep++);  //23
                workCount[lastElf]++;
                lastElf = GetNextElf2(nextStep++);  //24
                workCount[lastElf]++;
                lastElf = GetNextElf1(nextStep++);  //25
                workCount[lastElf]++;
                lastElf = GetNextElf2(nextStep++);  //26
                workCount[lastElf]++;
                lastElf = GetNextElf1(nextStep++);  //27
                workCount[lastElf]++;
            }

            //tail
            while (nextStep <= 1000740)
            {
                lastElf = GetNextElfFull(nextStep++);
                workCount[lastElf]++;
            }

            int? mostUsedElf = GetMostUsedElf();
            int? leastUsedElf = GetLeastUsedElf();

            return workCount[mostUsedElf ?? 0] - workCount[leastUsedElf ?? 0];
        }

        public int NextInLine(int current)
        {
            return order[current];
        }

        public int GetNextElf1(int nextStep)
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
            return NextInLine(lastElf);
        }

        public int GetNextElf2(int nextStep)
        {
            var next = NextInLine(lastElf);
            if (IsMostUsed(next))
            {
                return NextInLine(next);
            }
            return NextInLine(lastElf);
        }

        public int GetNextElf7(int nextStep)
        {
            return 4;
        }

        public int GetNextElf14(int nextStep)
        {
            var next = NextInLine(lastElf);
            if (IsMostUsed(next))
            {
                return NextInLine(next);
            }
            return 4;
        }

        public int GetNextElf28(int nextStep)
        {
            //clockwise *= -1;
            order = order == cwOrder ? ccwOrder : cwOrder;
            return NextInLine(lastElf);
        }

        public int GetNextElfFull(int nextStep)
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

            for (int j = 4; j <= maxPrime; j += 2)
            {
                eliminated[j] = true;
            }

            for (int i = 3; i <= maxPrime; i += 2)
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
            int count = 1000;
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
