using Common;
using System;

namespace KnowitJulekalender21
{
    class Program
    {
        static void Main(string[] args)
        {
            int count = 1;
            if (args.Length > 0) count = int.Parse(args[0]);
            using (new CodeTimer("programmet", null, count))
            {
                for (int i = 0; i < count; i++)
                {
                    var solver = new Solution();
                    solver.ParseFile("./generations.txt");
                    var first = solver.FindFirstCommonAncestor();
                    Console.WriteLine($"First common ancestor was {first.gen}:{first.index}");
                }

            }
        }
    }
}
