using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Diagnostics;

namespace KnowitJulekalender6
{
    class Program
    {
        static void DecodeImage(string inPath, string outPath)
        {
            using (var image = Image.Load<Rgba32>(inPath))
            {
                var decoded = image.Clone();
                Rgba32 previous = new Rgba32(0);

                for (int y = 0; y < image.Height; y++)
                {
                    for (int x = 0; x < image.Width; x++)
                    {
                        var current = image[x, y];
                        var xor = current.Rgba ^ previous.Rgba;
                        decoded[x, y] = new Rgba32(xor);
                        previous = current;
                    }
                }
                decoded.Save(outPath);
            }
        }

        static void Main(string[] args)
        {
            int n = 100;
            for (int i = 0; i < n; i++)
            {
                DecodeImage("./mush.png", "./decoded.png");
            }
            Stopwatch sw = Stopwatch.StartNew();
            for (int i = 0; i < n; i++)
            {
                DecodeImage("./mush.png", "./decoded.png");
            }
            var elapsed = sw.Elapsed;
            Console.WriteLine($"Finished {n} runs, average runtime was {elapsed.TotalMilliseconds / n} ms");
            Console.ReadKey();
        }
    }
}
