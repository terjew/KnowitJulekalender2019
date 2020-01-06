using Common;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace KnowitJulekalender21
{
    internal class Elf
    {
        public short father;
        public short mother;
        public short gen;
        public short index;

        public Elf(short gen, short index, short father, short mother)
        {
            this.gen = gen;
            this.index = index;
            this.father = father;
            this.mother = mother;
        }

        public int NumDescendants { get { return descendants.Count; } }

        private HashSet<short> descendants = new HashSet<short>();
        private object lockObj = new object();

        public void RegisterChild(Elf child)
        {
            lock (lockObj)
            {
                descendants.UnionWith(child.descendants);
            }
        }
        public void RegisterGen0Child(Elf child)
        {
            lock (lockObj)
            {
                descendants.Add(child.index);
            }
        }
    }

    class Solution
    {
        private IEnumerable<string> fileLines;

        private Elf BuildElf(string pair, short gen, short index)
        {
            var parentStrings = pair.Split(",");
            var father = short.Parse(parentStrings[0]);
            var mother = short.Parse(parentStrings[1]);
            return new Elf(gen, index, father, mother);
        }

        public void ParseFile(string path)
        {
            fileLines = File.ReadLines(path);
        }

        internal Elf FindFirstCommonAncestor()
        {
            var lineEnumerator = fileLines.GetEnumerator();
            lineEnumerator.MoveNext();
            var firstLine = lineEnumerator.Current;
            var gen0Elves = firstLine.EnumerateSegments(';')
                .AsParallel()
                .Select((pair, i) => BuildElf(pair, 0, (short)i))
                .Where((elf, i) => i % 2 != 0)
                .ToList();

            Parallel.ForEach(gen0Elves, (elf) =>
            {
                elf.RegisterGen0Child(elf);
            });

            List<Elf> previousGeneration = gen0Elves;
            int gen0Count = gen0Elves.Count;

            short gen = 1;
            while(lineEnumerator.MoveNext())
            {
                var line = lineEnumerator.Current;

                var elves = line.EnumerateSegments(';')
                    .AsParallel()
                    .Select((pair, i) => BuildElf(pair, gen, (short)i));

                List<Elf> currentGeneration = elves.ToList();

                Parallel.ForEach(previousGeneration, (elf) =>
                {
                    currentGeneration[elf.mother].RegisterChild(elf);
                    currentGeneration[elf.father].RegisterChild(elf);
                });

                var common = currentGeneration.AsParallel().FirstOrDefault(e => e.NumDescendants == gen0Count);
                if (common != null)
                {
                    return currentGeneration.OrderBy(e => e.index).First(e => e.NumDescendants == gen0Count);
                }
                previousGeneration = currentGeneration;
                gen++;
            }
            throw new Exception("couldn't find ancestor");
        }


    }
}
