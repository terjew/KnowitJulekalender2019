using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KnowitJulekalender21
{
    class GraphNode
    {
        internal class Repository
        {
            ConcurrentDictionary<string, GraphNode> nodes = new ConcurrentDictionary<string, GraphNode>();
            public GraphNode Get(string name)
            {
                var node = nodes.GetOrAdd(name, (name) => new GraphNode(name));
                return node;
            }
        }

        public GraphNode Parent1 { get; private set; }
        public GraphNode Parent2 { get; private set; }
        public string Name { get; }

        public int Generation
        {
            get
            {
                return short.Parse(Name.Split(":")[0]);
            }
        }

        public int Index
        {
            get
            {
                return short.Parse(Name.Split(":")[1]);
            }
        }

        internal void AddAncestors(HashSet<GraphNode> set)
        {
            if (Parent1 != null && !set.Contains(Parent1))
            {
                set.Add(Parent1);
                Parent1.AddAncestors(set);
            }
            if (Parent2 != null && !set.Contains(Parent2))
            {
                set.Add(Parent2);
                Parent2.AddAncestors(set);
            }
        }

        public GraphNode(string name)
        {
            this.Name = name;
        }

        public void SetParents(GraphNode parent1, GraphNode parent2)
        {
            Parent1 = parent1;
            Parent2 = parent2;
        }

    }
}
