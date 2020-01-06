using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace KnowitJulekalender22
{
    public class OptimizedByteArray
    {
        private byte[] forest;
        private int pos;
        private int last;

        public OptimizedByteArray(string forestPath) 
        {
            this.forest = File.ReadAllBytes(forestPath);
        }

        public int CalculateEarnings()
        {
            int forestLength = forest.Length;
            pos = 1;
            last = forestLength - 1;
            List<int> offsets = new List<int>();
            byte b;
            while ((b = forest[last - pos]) != '\n')
            {
                if (b == '#') offsets.Add(pos);
                pos++;
            }
            int numlines = forestLength / pos;
            var numTrees = offsets.Count;

            //walk the offsets 64 bytes at a time:
            int chunksize = 64;
            int chunks = offsets.Last() / chunksize;
            for (int chunk = 0; chunk < chunks; chunk++)
            {
                int startOffset = chunk * chunksize;
                int endOffset = startOffset + chunksize;
                
            }
            for (int line = 0; line < numlines; line++)
            {

            }

            return 0;
            //int segments = offsets.Sum(i => CountSegments(i)) / 3;
            //return segments * 40;
        }

        private int CountSegments(int offset)
        {
            int pos = last;
            int sum = 0;
            while (pos > 0)
            {
                sum += forest[pos - offset] - ' ';
                pos -= this.pos;
            }
            return sum;
        }
    }
}
