using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace KnowitJulekalender14
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 0; i < 10; i++)
            {
                Optimized();
            }
            //Naive();
        }

        public static unsafe void Optimized()
        {
            Stopwatch sw = Stopwatch.StartNew();
            byte[] alphabet = { 2, 3, 5, 7, 11 };
            int sequenceLength = 217532235;
            double averageSize = alphabet.Average(b => (int)b);
            int requiredMem = (int)(sequenceLength / (averageSize * 0.99)); //ta høyde for 1% avvik fra snittet
            int stopWriting = requiredMem - alphabet.Max();


            byte* sequenceptr = (byte*)Marshal.AllocHGlobal(requiredMem * sizeof(byte));
            int alphabetLength = alphabet.Length;

            int counter = 0;

            sequenceptr[0] = alphabet[0];
            sequenceptr[1] = alphabet[0];
            counter++;
            int end = 2;

            int valueIndex = 1;
            int numSevens = 0;
            {
                fixed (byte* alphabetptr = alphabet)
                {
                    while (end < stopWriting)
                    {
                        var num = (int)sequenceptr[counter];
                        byte value = alphabetptr[valueIndex];

                        byte* target = sequenceptr + end;

                        if (num == 7) goto WRITE7;
                        if (num == 5) goto WRITE5;
                        if (num == 3) goto WRITE3;
                        if (num == 2) goto WRITE2;
                        *(target + 10) = value;
                        *(target + 9) = value;
                        *(target + 8) = value;
                        *(target + 7) = value;
                    WRITE7:
                        *(target + 6) = value;
                        *(target + 5) = value;
                    WRITE5:
                        *(target + 4) = value;
                        *(target + 3) = value;
                    WRITE3:
                        *(target + 2) = value;
                    WRITE2:
                        *(target + 1) = value;
                        *target = value;
                        end += num;
                        if (value == 7) numSevens += num;
                        counter++;
                        valueIndex++;
                        if (valueIndex >= alphabetLength) valueIndex = 0;
                    }
                    while (end < sequenceLength)
                    {
                        var num = (int)sequenceptr[counter];
                        byte value = alphabetptr[valueIndex];

                        end += num;
                        if (value == 7) numSevens += num;
                        counter++;
                        valueIndex++;
                        if (valueIndex >= alphabetLength) valueIndex = 0;
                    }
                }
            }

            var elapsed = sw.Elapsed;
            Console.WriteLine($"Sum av syvere i sekvensen er {numSevens * 7} (brukte {elapsed.TotalMilliseconds} ms)");
        }

        public static void Naive()
        {
            Stopwatch sw = Stopwatch.StartNew();
            byte[] sequence = new byte[217532235];
            byte[] alphabet = { 2, 3, 5, 7, 11 };
            int alphabetLength = alphabet.Length;

            int counter = 0;

            sequence[0] = alphabet[0];
            sequence[1] = alphabet[0];
            counter++;
            int end = 2;

            int valueIndex = 1;
            int numSevens = 0;
            while (end < sequence.Length)
            {
                var num = (int)sequence[counter];
                byte value = alphabet[valueIndex];
                for (int i = 0; i < num; i++)
                {
                    sequence[end + i] = value;
                }
                end += num;
                if (value == 7) numSevens += num;
                counter++;
                valueIndex++;
                if (valueIndex >= alphabetLength) valueIndex = 0;
            }

            var sum = numSevens * 7;
            var elapsed = sw.Elapsed;
            Console.WriteLine($"Sum av syvere i sekvensen er {sum} (brukte {elapsed.TotalMilliseconds} ms)");
        }
    }
}
