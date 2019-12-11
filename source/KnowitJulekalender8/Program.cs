using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KnowitJulekalender8
{
    class Program
    {
        private Dictionary<string, Func<int, int>> _functions;
        private List<List<string>> _wheels;
        private List<int> _wheelPositions;
        private int _credit;
        private bool _stopped;

        static void Main(string[] args)
        {
            var program = new Program(File.ReadAllLines("./wheels.txt"));
            program.RunSimulation(6);
            for (int i = 1; i <= 10; i++)
            {
                Console.WriteLine($"{i} mynter ga resultat {program.RunSimulation(i)}");
            }
        }

        public Program(string[] wheelStrings)
        { 
            this._functions = new Dictionary<string, Func<int, int>>();

            _functions.Add("PLUSS4", (i) => i + 4);
            _functions.Add("PLUSS101", (i) => i + 101);
            _functions.Add("MINUS9", (i) => i - 9);
            _functions.Add("MINUS1", (i) => i - 1);

            _functions.Add("REVERSERSIFFER", ReverserSiffer);
            _functions.Add("OPP7", Opp7);
            _functions.Add("GANGEMSD", GangeMsd);
            _functions.Add("DELEMSD", DeleMsd);
            _functions.Add("PLUSS1TILPAR", Pluss1TilPar);
            _functions.Add("TREKK1FRAODDE", Trekk1FraOdde);
            _functions.Add("ROTERPAR", Roterpar);
            _functions.Add("ROTERODDE", Roterodde);
            _functions.Add("ROTERALLE", Roteralle);
            _functions.Add("STOPP", Stopp);

            _wheels = wheelStrings.Select(w => w.Split(':')[1]).Select(rem => rem.Split(',').Select(s => s.Trim()).ToList()).ToList();

            //tests:
            if (ReverserSiffer(123) != 321 || ReverserSiffer(-123) != -321 || ReverserSiffer(12) != 21) throw new InvalidDataException();
        }

        public int RunSimulation(int credit)
        {
            _wheelPositions = Enumerable.Repeat(0, 10).ToList();
            _credit = credit;
            _stopped = false;

            while (!_stopped)
            {
                var wheelIndex = GetDigits(_credit).Last();
                var wheel = _wheels[wheelIndex];
                var wheelPos = _wheelPositions[wheelIndex];
                var func = _functions[wheel[wheelPos]];
                _credit = func(_credit);
                RotateWheel(wheelIndex);
            }
            return _credit;
        }

        private void RotateWheel(int wheelIndex)
        {
            _wheelPositions[wheelIndex] = (_wheelPositions[wheelIndex] + 1) % 4;

        }
        private List<int> GetDigits(int val)
        {
            var str = Math.Abs(val).ToString();
            return str.Select(c => (int)(c - '0')).ToList();
        }

        private int GetInt(IEnumerable<int> digits)
        {
            return int.Parse(string.Join("", digits));
        }

        private int Stopp(int arg)
        {
            _stopped = true;
            return arg;
        }

        private int Roteralle(int arg)
        {
            for(int i = 0; i < 10; i++)
            {
                RotateWheel(i);
            }
            return arg;
        }

        private int Roterodde(int arg)
        {
            foreach (int i in new[] { 1, 3, 5, 7, 9 })
            {
                RotateWheel(i);
            }
            return arg;
        }

        private int Roterpar(int arg)
        {
            foreach (int i in new[] { 0, 2, 4, 6, 8})
            {
                RotateWheel(i);
            }
            return arg;
        }

        private int Trekk1FraOdde(int arg)
        {
            var digits = GetDigits(arg);
            var newDigits = digits.Select(d => (d % 2 == 0) ? d : d - 1);
            return Math.Sign(arg) * GetInt(newDigits);
        }

        private int Pluss1TilPar(int arg)
        {
            var digits = GetDigits(arg);
            var newDigits = digits.Select(d => (d % 2 == 0) ? d + 1 : d);
            return Math.Sign(arg) * GetInt(newDigits);
        }

        private int GangeMsd(int arg)
        {
            var digits = GetDigits(arg);
            return arg * digits[0];
        }

        private int DeleMsd(int arg)
        {
            var digits = GetDigits(arg);
            return EuclidianDiv(arg, digits[0]);
        }

        private int EuclidianDiv(int a, int b)
        {
            return (a - (((a % b) + b) % b)) / b;
        }

        private int Opp7(int arg)
        {
            var digits = GetDigits(arg);
            while (digits.Last() != 7)
            {
                arg++;
                digits = GetDigits(arg);
            }
            return arg;
        }

        private int ReverserSiffer(int arg)
        {
            var digits = GetDigits(arg);
            digits.Reverse();
            return Math.Sign(arg) * GetInt(digits);
        }
    }
}
