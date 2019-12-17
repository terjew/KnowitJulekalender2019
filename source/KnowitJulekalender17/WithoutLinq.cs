using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KnowitJulekalender17
{
    class WithoutLinq
    {
        class CalculationThread
        {
            private uint start;
            private uint end;

            public uint count = 0;

            public CalculationThread(uint start, uint end)
            {
                this.start = start;
                this.end = end;
            }

            public void Calculate()
            {
                count = (uint)CountTriangleSquares((long)start, (long)end);
            }
        }

        public static uint CountTriangleSquares(long start, long end)
        {
            int count = 0;
            for (long j = start; j < end; j++)
            {
                long triangle = (j * (j + 1)) / 2;
                count += Optimized.CountIfSquareRotated(triangle);
            }
            return (uint)count;
        }

        public static uint CountTriangleSquaresMultithreadedTasks(uint num)
        {
            const int numThreads = 16;
            uint perThread = num / numThreads;
            Task<uint>[] tasks = new Task<uint>[numThreads];

            for (uint i = 0; i < numThreads; i++)
            {
                uint start = perThread * i;
                uint end = start + perThread;
                tasks[i] = Task.Run(() => CountTriangleSquares(start, end));
            }
            Task.WaitAll(tasks);

            uint sum = 0;
            for (uint i = 0; i < numThreads; i++)
            {
                sum += tasks[i].Result;
            }
            return sum;
        }

        public static uint CountTriangleSquaresMultithreaded(uint num)
        {
            const int numThreads = 16;
            uint perThread = num / numThreads;
            CalculationThread[] calcs = new CalculationThread[numThreads];
            Thread[] threads = new Thread[numThreads];

            for (uint i = 0; i < numThreads; i++)
            {
                uint start = perThread * i;
                uint end = start + perThread;
                var calc = new CalculationThread(start, end);
                calcs[i] = calc;
                var thread = new Thread(new ThreadStart(calc.Calculate));
                threads[i] = thread;
                thread.Start();
            }
 
            uint sum = 0;
            for (uint i = 0; i < numThreads; i++)
            {
                threads[i].Join();
                sum += calcs[i].count;
            }
            return sum;
        }
    }
}
