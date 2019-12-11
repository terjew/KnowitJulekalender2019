using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace KnowitJulekalender10
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch sw = Stopwatch.StartNew();
            var text = File.ReadAllText("./logg.txt");
            Regex re = new Regex(@"^(?<month>\w{3}) (?<day>\d+)\:$\n(?:\s*\*\s(?:(?:(?<tannkrem>\d+) ml t\w*)|(?:(?<sjampo>\d+) ml s\w*)|(?:(?<tp>\d+) meter t\w*)\n)){3}", RegexOptions.Multiline);

            var entries = re.Matches(text)
                .Cast<Match>()
                .Select(m => {
                    Func<string, string> group = (name) => m.Groups[name].Value;
                    return new
                    {
                        Date = DateTime.Parse($"2018-{group("month")}-{group("day")}"),
                        Tannkrem = int.Parse(group("tannkrem")),
                        Sjampo = int.Parse(group("sjampo")),
                        ToalettPapir = int.Parse(group("tp")),
                    };
                })
                .ToList();

            var tuber = entries.Sum(e => e.Tannkrem) / 125;
            var flasker = entries.Sum(e => e.Sjampo) / 300;
            var ruller = entries.Sum(e => e.ToalettPapir) / 25;

            var sjampoSøndag = entries.Where(e => e.Date.DayOfWeek == DayOfWeek.Sunday).Sum(e => e.Sjampo);
            var tpOnsdag = entries.Where(e => e.Date.DayOfWeek == DayOfWeek.Wednesday).Sum(e => e.ToalettPapir);

            var result = tuber * flasker * ruller * sjampoSøndag * tpOnsdag;
            var elapsed = sw.Elapsed;
            Console.WriteLine($"Produktet er {result} (tid brukt er {elapsed.TotalMilliseconds} ms");
            Console.ReadKey();
        }
    }
}
