using Common;
using System;
using System.Linq;
using System.Numerics;
using System.Threading;

namespace KnowitJulekalender23
{
    class Program
    {
        private static readonly bool[] prime;
        static Program()
        {
            prime = GeneratePrimeTruthTable(100, false);
        }
        static void Main(string[] args)
        {
            int count = 1;

            int result = 0;
            //using (new CodeTimer("enkelttrådet beregning (SIMD)", null, count))
            //{
            //    for (int i = 0; i < count; i++) result = SolveSinglethreadedSIMD();
            //}
            //Console.WriteLine($"Count: {result}");
            using (new CodeTimer("enkelttrådet beregning (optimalisert)", null, count))
            {
                for (int i = 0; i < count; i++) result = SolveRangeOptimized(2, 100000);
            }
            Console.WriteLine($"Count: {result}");
            return;
            using (new CodeTimer("enkelttrådet beregning", null, count))
            {
                for (int i = 0; i < count; i++) result = SolveSinglethreaded();
            }
            Console.WriteLine($"Count: {result}");
            using (new CodeTimer("flertrådet beregning", null, count))
            {
                for (int i = 0; i < count; i++) result = SolveMultithreaded();
            }
            Console.WriteLine($"Count: {result}");
            using (new CodeTimer("flertrådet beregning (eksplisitt tråding)", null, count))
            {
                for (int i = 0; i < count; i++) result = SolveMultithreadedExplicit(98765432);
            }
            Console.WriteLine($"Count: {result}");
            using (new CodeTimer("flertrådet beregning (eksplisitt tråding, stort datasett)", null, count))
            {
                for (int i = 0; i < count; i++) result = SolveMultithreadedExplicit(int.MaxValue - 256);
            }
            Console.WriteLine($"Count: {result}");
        }

        static int SolveSinglethreadedSIMD()
        {
            int count = 0;
            var numData = Vector<int>.Count; //should be 8

            Vector<int> vPowers = new Vector<int>(new[] { 1, 10, 100, 1000, 10000, 100000, 1000000, 10000000 }, 0);
            Vector<int> v10 = new Vector<int>(new[] { 10, 10, 10, 10, 10, 10, 10, 10 }, 0);
            Vector<int> v1 = new Vector<int>(new[] { 1, 1, 1, 1, 1, 1, 1, 1}, 0);

            int digitSum = 1;

            for (int i = 2; i < 98765433; i++)
            {
                if (i % 10 != 0) digitSum++;
                else
                {
                    Vector<int> number = new Vector<int>(i);
                    var divided = Vector.Divide(number, vPowers);

                    //calculate modulo 10:
                    var d10 = Vector.Divide(divided, v10);
                    var m10 = Vector.Multiply(d10, v10);
                    var res = Vector.Subtract(divided, m10);

                    //Add all the digits together:
                    digitSum = Vector.Dot(res, v1);
                }
                if (prime[digitSum] && i % digitSum == 0)
                {
                    count += 1;
                }

            }
            return count;
        }


        static int SolveSinglethreaded()
        {
            int count = 0;
            for (int i = 2; i < 98765432; i++)
            {
                count += IfHarshadtPrime(i);
            }
            return count;
        }

        static int SolveMultithreaded()
        {
            int count = Enumerable.Range(2, 98765432)
                    .AsParallel()
                    .Sum(i => IfHarshadtPrime(i));
            return count;
        }

        static int SolveRange(int start, int end)
        {
            bool[] notPrime = GeneratePrimeTruthTable(72);
            int count = 0;
            for (int i = start; i < end; i++)
            {
                count += IfHarshadtPrimeWithTable(i, notPrime);
            }
            return count;
        }



        static int SolveRangeOptimized(int start, int end)
        {
            var largestPossibleSum = (int)(Math.Round(Math.Log10(end) + 1) * 9);
            bool[] prime = GeneratePrimeTruthTable(largestPossibleSum, false);
            int count = 0;
            int digitsum = 0;
            int rest = start;
            while (rest > 0)
            {
                digitsum += rest % 10;
                rest /= 10;
            }
            if (prime[digitsum] && start % digitsum == 0)
            {
                count += 1;
            }

            for (int i = start + 1; i < end; i++)
            {
                if (i % 10 != 0) digitsum++;
                else
                {
                    digitsum = 0;
                    rest = i;
                    while (rest > 0)
                    {
                        digitsum += rest % 10;
                        rest /= 10;
                    }
                }
                if (prime[digitsum] && i % digitsum == 0)
                {
                    count += 1;
                }
            }
            return count;
        }

        static int SolveMultithreadedExplicit(int max)
        {
            int numthreads = 12;
            int partitionsize = (max - 2) / numthreads;
            int start = 2;
            Thread[] threads = new Thread[numthreads];
            int[] sums = new int[numthreads];
            for (int threadNo = 0; threadNo < numthreads; threadNo++)
            {
                int threadBegin = start;
                int threadEnd = Math.Min(start + partitionsize, max);
                int currentThread = threadNo;
                var thread = new Thread(new ThreadStart(() =>
                {                    
                    sums[currentThread] = SolveRangeOptimized(threadBegin, threadEnd);
                }));
                thread.Start();
                threads[threadNo] = thread;
                start += partitionsize;
            }

            int sum = 0;
            for (int threadNo = 0; threadNo < numthreads; threadNo++)
            {
                threads[threadNo].Join();
                sum += sums[threadNo];
            }
            return sum;
        }

        private static int IfHarshadtPrimeWithTable(int number, bool[] notPrime)
        {
            int rest = number;
            int digitsum = 0;
            while (rest > 0)
            {
                digitsum += rest % 10;
                rest /= 10;
            }
            if (!notPrime[digitsum] && number % digitsum == 0)
            {
                return 1;
            }
            return 0;
        }


        private static int IfHarshadtPrime(int number)
        {
            int rest = number;
            int digitsum = 0;
            while(rest > 0)
            {
                digitsum += rest % 10;
                rest /= 10;
            }
            if (prime[digitsum] && number % digitsum == 0)
            {
                return 1;
            }
            return 0;
        }

        private static bool[] GeneratePrimeTruthTable(int maxPrime, bool inverted = true)
        {
            var maxSquareRoot = (int)Math.Sqrt(maxPrime);
            var eliminated = new bool[maxPrime + 1];
            eliminated[1] = true;
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
            if (inverted)
            {
                return eliminated;
            }
            else
            {
                return eliminated.Select(b => !b).ToArray();
            }
        }

    }
}
