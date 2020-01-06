using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace KnowitJulekalender22
{
    class OptimizedStream
    {
        private string path;

        public OptimizedStream(string path)
        {
            this.path = path;
        }

        public async Task<int> CalculateEarningsAsync()
        {
            int length;
            int lineWidth;

            const int BUFFERSIZE = 65536;
            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, BUFFERSIZE, FileOptions.SequentialScan))
            {
                length = (int)stream.Length;
                //measure first line:
                lineWidth = await MeasureLine(stream);
            }

            byte[] lastLine = new byte[lineWidth];
            List<int> offsets = new List<int>();
            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, FileOptions.RandomAccess))
            {
                stream.Seek(-lineWidth, SeekOrigin.End);
                stream.Read(lastLine, 0, lineWidth);
#if TEST
                int i = 0; 
                while (i < lineWidth)
                {
                    var offset = Array.IndexOf(lastLine, (byte)'#', i+1);
                    if (offset > 0)
                    {
                        offsets.Add(offset);
                        i = offset;
                    }
                    else
                    {
                        break;
                    }
                }
#else
                for (int i = 0; i < lineWidth; i++)
                {
                    if (lastLine[i] == '#') offsets.Add(i);
                }
#endif
            }

            return 0;
            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, BUFFERSIZE, FileOptions.SequentialScan))
            {
                return await ReadAllLinesDummy(stream, lineWidth, length, offsets);
            }


            //using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, BUFFERSIZE, FileOptions.SequentialScan))
            //{
            //    return 40 * await CountSegments(stream, length, lineWidth, lastLine);
            //}
        }

        private async Task<int> ReadAllLinesDummy(FileStream stream, int lineWidth, int length, List<int> offsets)
        {
            int CHUNKSIZE = lineWidth;
            byte[] front = new byte[CHUNKSIZE];
            byte[] back = new byte[CHUNKSIZE];
            Task<int> readTask;

            int numLines = length / lineWidth;
            readTask = stream.ReadAsync(back, 0, lineWidth);
            int line;
            for (line = 0; line < numLines - 10; line++)
            {
                await readTask;
                //swap buffers:
                var tmp = front;
                front = back;
                back = tmp;
                readTask = stream.ReadAsync(back, 0, lineWidth);

                //front buffer is available for reading:
                foreach (var offset in offsets)
                //for (int i = 0; i < lineWidth; i++)
                {
                    byte b = front[offset];
                }
                line++;
            }
            return line;
        }

        static async Task<int> CountSegments(FileStream stream, int filesize, int lineWidth, byte[] lastLine)
        {
            byte[] buffer1 = new byte[lineWidth];
            byte[] buffer2 = new byte[lineWidth];

            //queue buffer read before finding trunks in last line:
            Task<int> task1;
            Task<int> task2;
            task2 = stream.ReadAsync(buffer2, 0, lineWidth);

            int numLines = filesize / lineWidth;

            //find the positions of trunks in the last line:
            List<int> offsets = new List<int>();
            List<int> heights = new List<int>();

            for (int i = 0; i < lineWidth; i++)
            {
                if (lastLine[i] == '#') offsets.Add(i);
            }

            int line = 0;
            while (line < numLines)
            {
                //FIXME: adjust the read requests to only read up to the last remaining tree to be found?
                var toCheck = offsets.ToArray();
                task1 = stream.ReadAsync(buffer1, 0, lineWidth);
                await task2;
                foreach(var offset in toCheck)
                {
                    if (buffer2[offset] == '#')
                    {
                        heights.Add(numLines - line);
                        offsets.Remove(offset);
                    }
                }
                if (offsets.Count == 0) break;
                line++;

                toCheck = offsets.ToArray();
                task2 = stream.ReadAsync(buffer2, 0, lineWidth);                
                await task1;
                foreach (var offset in offsets.ToArray())
                {
                    if (buffer1[offset] == '#')
                    {
                        heights.Add(numLines - line);
                        offsets.Remove(offset);
                    }
                }
                if (offsets.Count == 0) break;
                line++;
            }

            int sum = 0;
            foreach (var height in heights) sum += height;
            return sum;
        }

        static async Task<int> MeasureLine(FileStream stream)
        {
            int len = 0;
            const int BUFSIZE = 8192;

            byte[] buffer1 = new byte[BUFSIZE];
            byte[] buffer2 = new byte[BUFSIZE];
            int read1;
            int read2;

            Task<int> task1;
            Task<int> task2;

            read2 = stream.Read(buffer2, 0, BUFSIZE);
            while (read2 > 0)
            {
                //queue buffer 1
                task1 = stream.ReadAsync(buffer1, 0, BUFSIZE);
                //process buffer 2
                for (int i = 0; i < read2; i++)
                {
                    if (buffer2[i] == '\n') return len + i + 1;
                }
                len += read2;

                //queue buffer 2
                task2 = stream.ReadAsync(buffer2, 0, BUFSIZE);
                //process buffer 1
                read1 = await task1;
                for (int i = 0; i < read1; i++)
                {
                    if (buffer1[i] == '\n') return len + i + 1;
                }
                len += read1;

                read2 = await task2;
            }
            return -1;
        }
    }
}
