using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace KnowitJulekalender22
{
    class Naive
    {
        public Naive(string path)
        {
            this.forestPath = path;
        }
        static Regex re = new Regex(@"#+", RegexOptions.Compiled);

        private string forestPath;

        int CountTrunksRegex(string l)
        {
            return re.Matches(l).Count;
        }

        public int CountTreeSegmentsRegex(IEnumerable<string> lines)
        {
            return lines.Sum(l => CountTrunksRegex(l));
        }

        public int CalculateEarnings()
        {
            return CountTreeSegmentsRegex(File.ReadLines(forestPath)) * 40;
        }


    }
}
