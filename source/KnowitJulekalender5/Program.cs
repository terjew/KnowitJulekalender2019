using System;
using System.Text;

namespace KnowitJulekalender5
{
    class Program
    {
        static void Main(string[] args)
        {
            var scrambled = "tMlsioaplnKlflgiruKanliaebeLlkslikkpnerikTasatamkDpsdakeraBeIdaegptnuaKtmteorpuTaTtbtsesOHXxonibmksekaaoaKtrssegnveinRedlkkkroeekVtkekymmlooLnanoKtlstoepHrpeutdynfSneloietbol";
            //scrambled = "oepHlpslainttnotePmseormoTtlst";

            var t1 = ExchangeTriplets(scrambled);
            var t2 = PairwiseSwap(t1);
            var t3 = SwitchHalves(t2);
            Console.WriteLine(t3);
        }

        private static string SwitchHalves(string t2)
        {
            return t2.Substring(t2.Length / 2) + t2.Substring(0, t2.Length / 2);
        }

        private static string PairwiseSwap(string t1)
        {
            StringBuilder sb = new StringBuilder();
            for(int i = 0; i < t1.Length / 2; i++)
            {
                sb.Append(t1[2 * i + 1]);
                sb.Append(t1[2 * i]);
            }
            return sb.ToString();
        }

        private static string ExchangeTriplets(string scrambled)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < scrambled.Length / 3; i++)
            {
                sb.Append(scrambled.Substring(scrambled.Length - (i * 3) - 3, 3));
            }
            return sb.ToString();
        }
    }
}
