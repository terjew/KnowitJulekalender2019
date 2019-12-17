using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace KnowitJulekalender15
{
    public class Vector
    {
        public float X;
        public float Y;
        public float Z;
        public IEnumerable<float> Components 
        { 
            get
            {
                yield return X;
                yield return Y;
                yield return Z;
            }
        }

        public string ToObj()
        {
            return "v " + string.Join(" ", Components.Select(c => c.ToString(CultureInfo.InvariantCulture)));
        }
    }

    public class Triangle
    {
        public Vector P1;
        public Vector P2;
        public Vector P3;
        public IEnumerable<Vector> Coordinates
        {
            get
            {
                yield return P1;
                yield return P2;
                yield return P3;
            }
        }

        public Triangle(string line)
        {
            var components = line.
                Split(",", StringSplitOptions.RemoveEmptyEntries).
                Select(c => float.Parse(c, CultureInfo.InvariantCulture)).
                ToArray();

            P1 = new Vector()
            {
                X = components[0],
                Y = components[1],
                Z = components[2],
            };
            P2 = new Vector()
            {
                X = components[3],
                Y = components[4],
                Z = components[5],
            };
            P3 = new Vector()
            {
                X = components[6],
                Y = components[7],
                Z = components[8],
            };
        }

        public string ToObj()
        {
            var coords = string.Join("\n", Coordinates.Select(c => c.ToObj()));
            return coords + "\nf -3 -2 -1\n";
        }

        public double SignedVolume()
        {
            var v321 = P3.X * P2.Y * P1.Z;
            var v231 = P2.X * P3.Y * P1.Z;
            var v312 = P3.X * P1.Y * P2.Z;
            var v132 = P1.X * P3.Y * P2.Z;
            var v213 = P2.X * P1.Y * P3.Z;
            var v123 = P1.X * P2.Y * P3.Z;
            return (1.0f / 6.0f) * (-v321 + v231 + v312 - v132 - v213 + v123);
        }
    }

    public class Model
    {
        public Model(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                string line = null;
                while ((line = reader.ReadLine()) != null) {
                    Triangles.Add(new Triangle(line));
                }
            }
        }

        public string ToObj()
        {
            var triangles = string.Join("\n", Triangles.Select(t => t.ToObj()));
            return triangles;
        }

        public List<Triangle> Triangles = new List<Triangle>();

        public double Volume()
        {
            var signedVolume = Triangles.AsParallel().Sum(t => t.SignedVolume());
            return Math.Abs(signedVolume);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch sw = Stopwatch.StartNew();
            string path = Path.GetFullPath("./model.csv.zip");
            using (var archive = ZipFile.OpenRead(path))
            {
                foreach (var entry in archive.Entries)
                {
                    var model = new Model(entry.Open());
                    var volume = model.Volume();
                    var elapsed = sw.Elapsed;
                    Console.WriteLine($"Volume of model is {volume / 1000} (spent {elapsed.TotalMilliseconds} ms)");

                    //var obj = model.ToObj();
                    //Console.WriteLine(obj);
                }
            }
        }
    }
}
