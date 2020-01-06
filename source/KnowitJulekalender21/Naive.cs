using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace KnowitJulekalender21
{
    class Naive
    {
        private GraphNode.Repository repository;
        private Regex re = new Regex(@"(?<p1>\d+),(?<p2>\d+);", RegexOptions.Compiled);
        private List<HashSet<GraphNode>> generations;

        public Naive()
        {
            repository = new GraphNode.Repository();
        }

        HashSet<GraphNode> GetAncestors(GraphNode gen0Node)
        {
            HashSet<GraphNode> ancestors = new HashSet<GraphNode>();
            gen0Node.AddAncestors(ancestors);
            return ancestors;
        }

        public GraphNode FindFirstCommonAncestor()
        {
            var gen0Nodes = generations[0].ToArray();
            var common = gen0Nodes.
                AsParallel()
                .Select((n,i) =>
                {
                    if ((i % 100) == 0) Console.WriteLine(i);
                    return GetAncestors(n);
                })
                .Aggregate((a, b) =>
                {
                    a.IntersectWith(b);
                    return a;
                });
            return common.OrderBy(n => n.Generation).ThenBy(n => n.Index).First();
        }

        private GraphNode BuildGraphNode(Match match, int gen, int i)
        {
            var name = $"{gen}:{i}";
            var p1i = match.Groups["p1"];
            var p2i = match.Groups["p2"];
            var p1name = $"{gen + 1}:{p1i}";
            var p2name = $"{gen + 1}:{p2i}";
            var node = repository.Get(name);
            node.SetParents(repository.Get(p1name), repository.Get(p2name));
            return node;
        }

        public void ParseFile(string path)
        {
            var lines = File.ReadLines(path);
            var tuples = lines
                .Select((s, i) => ParseGeneration(s, i)).ToList();

            generations = new List<HashSet<GraphNode>>();
            foreach (var tuple in tuples.OrderBy(t => t.Item1))
            {
                generations.Add(tuple.Item2);
            }
        }

        Tuple<int, HashSet<GraphNode>> ParseGeneration(string line, int gen)
        {
            var matches = re.Matches(line);
            var generation = matches
                .AsParallel()
                .AsOrdered()
                .Select((m, i) => BuildGraphNode(m, gen, i));
            if (gen == 0) generation = generation.Where((item, index) => index % 2 != 0);

            var set = new HashSet<GraphNode>(generation);
            return new Tuple<int, HashSet<GraphNode>>(gen, set);
        }
    }
}
