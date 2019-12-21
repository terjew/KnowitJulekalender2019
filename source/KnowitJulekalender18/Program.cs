using Common;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

namespace KnowitJulekalender18
{
    class StarWarsName : IEquatable<StarWarsName>
    {
        public static void LoadNames(string[] lines)
        {
            var parts = lines.Split("---").ToArray();
            FirstNamesMale = parts[0].ToArray();
            FirstNamesFemale = parts[1].ToArray();
            LastNames = parts[2].ToArray();
            Suffixes = parts[3].ToArray();
        }

        static string[] FirstNamesMale  ;
        static string[] FirstNamesFemale;
        static string[] LastNames;
        static string[] Suffixes;

        public string Firstname => gender == 0 ? FirstNamesMale[firstNameIndex] : FirstNamesFemale[firstNameIndex];
        public string Lastname => LastNames[lastNameIndex];
        public string Suffix => Suffixes[suffixIndex];

        private uint gender;
        private uint firstNameIndex;
        private uint lastNameIndex;
        private uint suffixIndex;

        public override string ToString()
        {
            return $"{Firstname} {Lastname}{Suffix}";
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as StarWarsName);
        }

        public bool Equals([AllowNull] StarWarsName other)
        {
            return other != null &&
                   gender == other.gender &&
                   firstNameIndex == other.firstNameIndex &&
                   lastNameIndex == other.lastNameIndex &&
                   suffixIndex == other.suffixIndex;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(gender, firstNameIndex, lastNameIndex, suffixIndex);
        }

        public static bool operator==(StarWarsName a, StarWarsName b)
        {
            return ReferenceEquals(a, b) || (a?.Equals(b) ?? false);
        }

        public static bool operator!=(StarWarsName a, StarWarsName b)
        {
            return !(a == b);
        }

        public StarWarsName(string firstName, string lastname, string genderStr)
        {
            gender = genderStr == "M" ? 0U : 1U;

            uint firstNamesCount = (uint)(gender == 0 ? FirstNamesMale.Length : FirstNamesFemale.Length);
            firstNameIndex = (uint)firstName.Sum(c => c) % firstNamesCount;

            uint len = (uint)lastname.Length;
            var lastHalfCount = len / 2;
            var firstHalfCount = len - lastHalfCount;

            lastNameIndex = (uint)(lastname.Substring(0, (int)firstHalfCount).ToLower().Sum(c => c - 'a' + 1)) % (uint)LastNames.Length;

            var lastChars = lastname.Skip((int)firstHalfCount);
            long product = lastChars.First();
            foreach(var c in lastChars.Skip(1))
            {
                product *= c;
            }

            if (gender == 0)
            {
                product *= firstName.Length;
            }
            else
            {
                product *= (firstName.Length + lastname.Length);
            }

            var productDigitsDescending = product.ToString().Select(c => c - '0').OrderByDescending(i => i);
            suffixIndex = (uint)(ulong.Parse(string.Join("", productDigitsDescending)) % (uint)Suffixes.Length);
        }

    }
    class Program
    {
        private static StarWarsName CreateName(string line)
        {
            var components = line.Split(",");
            return new StarWarsName(components[0], components[1], components[2]);
        }

        static void Main(string[] args)
        {
            Stopwatch sw = Stopwatch.StartNew();
            StarWarsName.LoadNames(File.ReadAllLines("./names.txt"));

            var names = File.ReadLines("./employees.csv").Skip(1);

            var mostPopular = names
                .AsParallel()
                .Select(l => CreateName(l))
                .GroupBy(n => n)
                .OrderByDescending(g => g.Count())
                .First()
                .First();

            var elapsed = sw.Elapsed;
            Console.WriteLine($"Most popular name was {mostPopular} ({elapsed.TotalMilliseconds} ms)");
        }
    }
}
